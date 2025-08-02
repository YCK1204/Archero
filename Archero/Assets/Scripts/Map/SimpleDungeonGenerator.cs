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
        // 0. CurPosition�� StartPos�� ����
        // 1. CurPosition�� �� ����
        // 2. ���� �� ���� Direction �����¿� �������� ����
        // 3. ���� �� ��ġ�� CurPosition + Direction * RoomSpacing���� ����
        // 4. ���� �� ��ġ�� �̹� �����ϴ� �� ��ġ�� ��ġ�� �ʴ��� Ȯ��
        // 5. ��ġ�� ������ �� ��ġ�� �߰��ϰ�, ��ġ�� 2������ ���ư�
        // 6. 2-5���� CorridorCount��ŭ �ݺ�
        // 7. �� ��ġ�� ������� ���� ����
        // 8. �� ��ġ�� ������� �ٴ� Ÿ���� ����
        // 9. �� ��ġ�� ������� ���� Ÿ���� ����
        // 10. �ٴ� Ÿ���� ������� �� Ÿ���� ����


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
