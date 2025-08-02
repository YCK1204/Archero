using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField]
    private int CorridorCount = 5;
    [SerializeField]
    protected SimpleRandomWalkSO SimpleRandomWalkData = null;
    [SerializeField]
    private int RoomSpacing = 50;
    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }
    private void CorridorFirstGeneration()
    {
        // 0. CurPosition를 StartPos로 설정
        // 1. CurPosition에 맵 생성
        // 2. 다음 맵 방향 Direction 상하좌우 랜덤으로 설정
        // 3. 다음 맵 위치를 CurPosition + Direction * RoomSpacing으로 설정
        // 4. 다음 맵 위치가 이미 존재하는 맵 위치와 겹치지 않는지 확인
        // 5. 겹치지 않으면 맵 위치를 추가하고, 겹치면 2번으로 돌아감
        // 6. 2-5번을 CorridorCount만큼 반복
        // 7. 맵 위치를 기반으로 방을 생성
        // 8. 방 위치를 기반으로 바닥 타일을 생성
        // 9. 맵 위치를 기반으로 복도 타일을 생성
        // 10. 바닥 타일을 기반으로 벽 타일을 생성


        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();

        Vector2Int curPos = StartPos;
        Vector2Int direction = Direction2D.GetRandomCardinalDirection();

        List<Vector2Int> roomCenterPositions = new List<Vector2Int>();
        List<Vector2Int> mapToMapDirections = new List<Vector2Int>();
        roomCenterPositions.Add(curPos);

        for (int i = 0; i < CorridorCount; i++)
        {
            var nextRoomPos = FindNonOverlappingMapPosition(curPos, roomCenterPositions);
            roomCenterPositions.Add(nextRoomPos.Item1);
            mapToMapDirections.Add(nextRoomPos.Item2);
            curPos = nextRoomPos.Item1;
        }

        List<HashSet<Vector2Int>> roomPositions = CreateRooms(roomCenterPositions);
        foreach (var room in roomPositions)
        {
            foreach (var pos in room)
                floorPos.Add(pos);
        }

        TilemapVisualizer.PaintFloorTiles(floorPos);
        List<HashSet<Vector2Int>> corridors = CreateCorridors(roomPositions, mapToMapDirections);
        foreach (var corridor in corridors)
        {
            foreach (var pos in corridor)
                floorPos.Add(pos);
            Debug.Log($"corridor length: {corridor.Count}");
        }
        TilemapVisualizer.PaintFloorTiles(floorPos);
        TilemapVisualizer.GenerateWalls(floorPos);
    }
    private Tuple<Vector2Int, Vector2Int> FindNonOverlappingMapPosition(Vector2Int mapPos, List<Vector2Int> roomPositions)
    {
        Vector2Int nextMapPosition = mapPos;
        Vector2Int direction = Direction2D.GetRandomCardinalDirection();

        while (roomPositions.Contains(nextMapPosition))
        {
            direction = Direction2D.GetRandomCardinalDirectionExcluding(direction);
            nextMapPosition = mapPos + direction * RoomSpacing;
        }
        return new Tuple<Vector2Int, Vector2Int>(nextMapPosition, direction);
    }
    private List<HashSet<Vector2Int>> CreateRooms(List<Vector2Int> roomCenterPositions)
    {
         List<HashSet<Vector2Int>> roomPositions = new  List<HashSet<Vector2Int>>();

        foreach (var pos in roomCenterPositions)
        {
            var roomFloor = RunRandomWalk(pos);
            roomPositions.Add(roomFloor);
        }
        return roomPositions;
    }
    private List<HashSet<Vector2Int>> CreateCorridors(List<HashSet<Vector2Int>> roomPositions, List<Vector2Int> directions)
    {
        List<HashSet<Vector2Int>> corridors = new List<HashSet<Vector2Int>>();

        for (int i = 0; i < directions.Count; i++)
        {
            var curRoomPositions = roomPositions[i];
            var nextRoomPositions = roomPositions[i + 1];
            Vector2Int direction = directions[i];
            var corridor = CreateCorridor(curRoomPositions, nextRoomPositions, direction);
            corridors.Add(corridor);
        }
        return corridors;
    }
    Vector2Int FindStartCorridorPosition(HashSet<Vector2Int> roomPositions, Vector2Int direction)
    {
        Vector2Int centerPosition = roomPositions.First();

        var candidates = roomPositions
            .Where(pos => (direction.x != 0 && pos.y == centerPosition.y) ||
                          (direction.y != 0 && pos.x == centerPosition.x))
            .OrderBy(pos => (direction.x != 0) ? pos.x : pos.y);

        Vector2Int curRoomBorderPosOfDirection = direction switch
        {
            var d when d == Vector2Int.left => candidates.First(),
            var d when d == Vector2Int.right => candidates.Last(),
            var d when d == Vector2Int.up => candidates.Last(),
            var d when d == Vector2Int.down => candidates.First(),
        };

        int y = (direction.y != 0) ? (curRoomBorderPosOfDirection.y + centerPosition.y) / 2 : centerPosition.y;
        int x = (direction.x != 0) ? (curRoomBorderPosOfDirection.x + centerPosition.x) / 2 : centerPosition.x;
        Vector2Int corridorStartPos = new Vector2Int(x, y);

        while (corridorStartPos != curRoomBorderPosOfDirection)
        {
            if (roomPositions.Contains(corridorStartPos))
                break;
            corridorStartPos += direction;
        }

        return corridorStartPos;
    }
    Vector2Int FindEndCorridorPosition(Vector2Int direction, HashSet<Vector2Int> nextRoomPositions)
    {
        Vector2Int nextRoomCenterPos = nextRoomPositions.First();

        var candidates = nextRoomPositions
            .Where(pos => (direction.x != 0 && pos.y == nextRoomCenterPos.y) ||
                          (direction.y != 0 && pos.x == nextRoomCenterPos.x))
            .OrderBy(pos => (direction.x != 0) ? pos.x : pos.y);

        Vector2Int nextRoomBorderPosOfDirection = direction switch
        {
            var d when d == Vector2Int.left => candidates.Last(),
            var d when d == Vector2Int.right => candidates.First(),
            var d when d == Vector2Int.up => candidates.First(),
            var d when d == Vector2Int.down => candidates.Last(),
        };
            
        int y = (direction.y != 0) ? (nextRoomBorderPosOfDirection.y + nextRoomCenterPos.y) / 2 : nextRoomCenterPos.y;
        int x = (direction.x != 0) ? (nextRoomBorderPosOfDirection.x + nextRoomCenterPos.x) / 2 : nextRoomCenterPos.x;
        Vector2Int corridorEndPos = new Vector2Int(x, y);

        while (corridorEndPos != nextRoomBorderPosOfDirection)
        {
            if (nextRoomPositions.Contains(corridorEndPos))
                break;
            corridorEndPos += (-direction);
        }

        return corridorEndPos;
    }
    HashSet<Vector2Int> CreateCorridor(HashSet<Vector2Int> curRoomPositions, HashSet<Vector2Int> nextRoomPositions, Vector2Int direction)
    {
        Vector2Int curPos = FindStartCorridorPosition(curRoomPositions, direction);
        Vector2Int endPos = FindEndCorridorPosition(direction, nextRoomPositions);

        HashSet<Vector2Int> corridorPositions = new HashSet<Vector2Int>();

        while (curPos != endPos)
        {
            corridorPositions.Add(curPos);
            curPos += direction;
        }
        corridorPositions.Add(endPos);
        return corridorPositions;
    }
    protected HashSet<Vector2Int> RunRandomWalk(Vector2Int pos)
    {
        var curPos = pos;
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();

        for (int i = 0; i < SimpleRandomWalkData.Iterations; i++)
        {
            var path = ProceduralGenerationAlgorihms.SimpleRandomWalk(curPos, SimpleRandomWalkData.WalkLength);
            floorPos.UnionWith(path);
            if (SimpleRandomWalkData.StartRandomlyEachIteration)
                curPos = floorPos.ElementAt(Random.Range(0, floorPos.Count));
        }
        return floorPos;
    }
}
