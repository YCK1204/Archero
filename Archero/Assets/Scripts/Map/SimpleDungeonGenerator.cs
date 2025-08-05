using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class Corridor
{
    public Vector2Int StartPosition;
    public Vector2Int EndPosition;
    public Door StartDoor;
    public Door EndDoor;
    public HashSet<Vector2Int> Positions;
}
public class Map
{
    public HashSet<Vector2Int> Positions;
    public Vector2Int CenterPosition;
    public Corridor Corridor;
}

public class SimpleDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField]
    protected SimpleRandomWalkSO SimpleRandomWalkData = null;

    protected List<Map> _mapData = new List<Map>();
    public List<Map> GetMapData { get { return _mapData; } }
    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }
    protected void CorridorFirstGeneration()
    {
        // 0. CurPosition를 StartPos로 설정
        // 1. CurPosition에 맵 생성
        // 2. 다음 맵 방향 Direction 상하좌우 랜덤으로 설정
        // 3. 다음 맵 위치를 CurPosition + Direction * RoomSpacing으로 설정
        // 4. 다음 맵 위치가 이미 존재하는 맵 위치와 겹치지 않는지 확인
        // 5. 겹치지 않으면 맵 위치를 추가하고, 겹치면 2번으로 돌아감
        // 6. 2-5번을 SimpleRandomWalkData.CorridorCount만큼 반복
        // 7. 맵 위치를 기반으로 방을 생성
        // 8. 복도 시작 끝점을 현재 맵, 다음 맵 Border위치로 설정
        // 9. 복도 양옆 min, max + 3 확보(dfs로 입출구로부터 맵의 중심점까지 유효성 검사) 중복된 칸 삭제
        // 10. 9로 확보하며 유효한 복도 입출구 설정
        // 11. 삭제되면서 생겼을 수 있는 섬같은 공간 삭제(bfs로 현재 위치로부터 맵까지 이동 가능한지 유효성 검사)
        // 12. 복도 생성

        if (SimpleRandomWalkData.CorridorWidth % 2 != 1)
        {
            Debug.LogError("SimpleRandomWalkData.CorridorWidth 값은 반드시 홀수여야 합니다.");
            return;
        }

        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();

        Vector2Int curPos = StartPos;
        Vector2Int direction = Direction2D.GetRandomCardinalDirection();

        List<Vector2Int> roomCenterPositions = new List<Vector2Int>();
        List<Vector2Int> mapToMapDirections = new List<Vector2Int>();
        roomCenterPositions.Add(curPos);

        for (int i = 0; i < SimpleRandomWalkData.CorridorCount; i++)
        {
            var nextRoomPos = FindNonOverlappingMapPosition(curPos, roomCenterPositions);
            roomCenterPositions.Add(nextRoomPos.Item1);
            mapToMapDirections.Add(nextRoomPos.Item2);
            curPos = nextRoomPos.Item1;
        }

        List<HashSet<Vector2Int>> roomPositions = CreateRooms(roomCenterPositions);
        List<Corridor> corridors = CreateCorridors(roomPositions, mapToMapDirections, roomCenterPositions);
        RemoveIsland(roomPositions, roomCenterPositions);

        for (int i = 0; i <= SimpleRandomWalkData.CorridorCount; i++)
        {
            Map data = new Map();
            data.CenterPosition = roomCenterPositions[i];
            data.Positions = new HashSet<Vector2Int>(roomPositions[i]);
            if (i < SimpleRandomWalkData.CorridorCount)
            {
                data.Corridor = corridors[i];
            }
            else
            {
                data.Corridor = null; // 마지막 맵은 복도가 없음
            }

            _mapData.Add(data);
        }

        foreach (var corridor in corridors)
        {
            foreach (var pos in corridor.Positions)
                floorPos.Add(pos);
        }
        foreach (var room in roomPositions)
        {
            foreach (var pos in room)
                floorPos.Add(pos);
        }
        TilemapVisualizer.PaintFloorTiles(floorPos);
        TilemapVisualizer.GenerateWalls(floorPos);
    }
    void RemoveIsland(List<HashSet<Vector2Int>> roomPositions, List<Vector2Int> roomCenterPositions)
    {
        for (int i = 0; i < roomPositions.Count; i++)
        {
            var room = roomPositions[i];
            var centerPos = roomCenterPositions[i];
            var connectedRegions = FindConnectedRegions(room);

            if (connectedRegions.Count == 1)
                continue;

            foreach (var region in connectedRegions)
            {
                if (region.Contains(centerPos) == false)
                {
                    foreach (var pos in region)
                    {
                        room.Remove(pos);
                    }
                }
            }
        }
    }

    private Tuple<Vector2Int, Vector2Int> FindNonOverlappingMapPosition(Vector2Int mapPos, List<Vector2Int> roomPositions)
    {
        Vector2Int nextMapPosition = mapPos;
        Vector2Int direction = Direction2D.GetRandomCardinalDirection();

        while (roomPositions.Contains(nextMapPosition))
        {
            direction = Direction2D.GetRandomCardinalDirectionExcluding(direction);
            nextMapPosition = mapPos + direction * SimpleRandomWalkData.RoomSpacing;
        }
        return new Tuple<Vector2Int, Vector2Int>(nextMapPosition, direction);
    }
    private List<HashSet<Vector2Int>> CreateRooms(List<Vector2Int> roomCenterPositions)
    {
        List<HashSet<Vector2Int>> roomPositions = new List<HashSet<Vector2Int>>();

        foreach (var pos in roomCenterPositions)
        {
            var roomFloor = RunRandomWalk(pos);
            LimitMapToBounds(pos, roomFloor);
            roomPositions.Add(roomFloor);
        }
        return roomPositions;
    }
    void LimitMapToBounds(Vector2Int center, HashSet<Vector2Int> positions)
    {
        int xMin = center.x - DungeonWidth / 2;
        int xMax = center.x + DungeonWidth / 2;
        int yMin = center.y - DungeonHeight / 2;
        int yMax = center.y + DungeonHeight / 2;

        positions.RemoveWhere(pos => pos.x < xMin || pos.x > xMax || pos.y < yMin || pos.y > yMax);
    }
    private List<Corridor> CreateCorridors(List<HashSet<Vector2Int>> roomPositions, List<Vector2Int> directions, List<Vector2Int> roomCenterPositions)
    {
        List<Corridor> corridors = new List<Corridor>();

        for (int i = 0; i < directions.Count; i++)
        {
            var curRoomPositions = roomPositions[i];
            var nextRoomPositions = roomPositions[i + 1];
            Vector2Int direction = directions[i];
            var curRoomCenterPos = roomCenterPositions[i];
            var nextRoomCenterPos = roomCenterPositions[i + 1];

            var corridor = CreateCorridor(curRoomPositions, nextRoomPositions, direction, curRoomCenterPos, nextRoomCenterPos);
            corridors.Add(corridor);
        }
        return corridors;
    }
    Vector2Int FindCorridorStartPosition(HashSet<Vector2Int> roomPositions, Vector2Int direction)
    {
        Vector2Int centerPosition = roomPositions.First();

        var candidates = roomPositions
            .Where(pos => (direction.x != 0 && pos.y == centerPosition.y) ||
                          (direction.y != 0 && pos.x == centerPosition.x))
            .OrderBy(pos => (direction.x != 0) ? pos.x : pos.y);

        return direction switch
        {
            var d when d == Vector2Int.left => candidates.First(),
            var d when d == Vector2Int.right => candidates.Last(),
            var d when d == Vector2Int.up => candidates.Last(),
            var d when d == Vector2Int.down => candidates.First(),
        };
    }
    Vector2Int FindCorridorEndPosition(Vector2Int direction, HashSet<Vector2Int> nextRoomPositions)
    {
        Vector2Int nextRoomCenterPos = nextRoomPositions.First();

        var candidates = nextRoomPositions
            .Where(pos => (direction.x != 0 && pos.y == nextRoomCenterPos.y) ||
                          (direction.y != 0 && pos.x == nextRoomCenterPos.x))
            .OrderBy(pos => (direction.x != 0) ? pos.x : pos.y);

        return direction switch
        {
            var d when d == Vector2Int.left => candidates.Last(),
            var d when d == Vector2Int.right => candidates.First(),
            var d when d == Vector2Int.up => candidates.First(),
            var d when d == Vector2Int.down => candidates.Last(),
        };
    }
    Corridor CreateCorridor(HashSet<Vector2Int> curRoomPositions,
                                        HashSet<Vector2Int> nextRoomPositions,
                                        Vector2Int direction,
                                        Vector2Int curRoomCenterPos,
                                        Vector2Int nextRoomCenterPos)
    {
        Vector2Int startPos = FindCorridorStartPosition(curRoomPositions, direction);
        Vector2Int endPos = FindCorridorEndPosition(direction, nextRoomPositions);

        startPos = EnsureCorridorStartPosition(startPos, curRoomCenterPos, curRoomPositions, -direction);
        endPos = EnsureCorridorEndPosition(endPos, nextRoomCenterPos, nextRoomPositions, direction);
        HashSet<Vector2Int> corridorPositions = new HashSet<Vector2Int>();
        Vector2Int curPos = startPos;
        bool horizontal = (endPos - startPos).x != 0;
        Corridor corridor = new Corridor
        {
            StartPosition = startPos,
            EndPosition = endPos,
            StartDoor = null,
            EndDoor = null,
            Positions = corridorPositions
        };

        while (curPos != endPos)
        {
            corridorPositions.Add(curPos);
            if (curPos != startPos)
            {
                for (int i = 1; i <= SimpleRandomWalkData.CorridorWidth / 2; i++)
                {
                    if (horizontal)
                    {
                        corridorPositions.Add(curPos + Vector2Int.up * i);
                        corridorPositions.Add(curPos + Vector2Int.down * i);
                    }
                    else
                    {
                        corridorPositions.Add(curPos + Vector2Int.left * i);
                        corridorPositions.Add(curPos + Vector2Int.right * i);
                    }
                }
            }
            curPos += direction;

        }
        curPos = startPos + direction;
        while (curPos != endPos)
        {
            RemoveCorridorSide(curRoomPositions, curPos, horizontal);
            RemoveCorridorSide(nextRoomPositions, curPos, horizontal);
            curPos += direction;
        }

        corridorPositions.Add(endPos);
        return corridor;
    }

    Vector2Int EnsureCorridorEndPosition(Vector2Int curPos, Vector2Int endPos, HashSet<Vector2Int> curRoomPositions, Vector2Int direction)
    {
        Vector2Int startPos = curPos;

        bool horizontal = direction.x != 0;
        while (curPos != endPos)
        {
            RemoveCorridorSide(curRoomPositions, curPos, horizontal);
            if (CanReach(curPos + direction, endPos, curRoomPositions) == true)
                break;
            curPos += direction;
        }
        return curPos;
    }

    Vector2Int EnsureCorridorStartPosition(Vector2Int curPos, Vector2Int endPos, HashSet<Vector2Int> curRoomPositions, Vector2Int direction)
    {
        Vector2Int startPos = curPos;

        bool horizontal = direction.x != 0;
        while (curPos != endPos)
        {
            RemoveCorridorSide(curRoomPositions, curPos, horizontal);
            if (CanReach(curPos + direction, endPos, curRoomPositions) == true)
                break;

            curPos += direction;
        }
        return curPos;
    }
    void RemoveCorridorSide(HashSet<Vector2Int> curRoomPositions, Vector2Int pos, bool horizontal)
    {
        curRoomPositions.Remove(pos);
        if (horizontal)
        {
            for (int i = 1; i <= SimpleRandomWalkData.CorridorWidth / 2; i++)
            {
                curRoomPositions.Remove(pos + Vector2Int.up * i);
                curRoomPositions.Remove(pos + Vector2Int.up * (i + 2));
                curRoomPositions.Remove(pos + Vector2Int.down * i);
                curRoomPositions.Remove(pos + Vector2Int.down * (i + 2));
            }
            curRoomPositions.Remove(pos + Vector2Int.up * ((SimpleRandomWalkData.CorridorWidth / 2) + 1));
            curRoomPositions.Remove(pos + Vector2Int.down * ((SimpleRandomWalkData.CorridorWidth / 2) + 1));
        }
        else
        {
            for (int i = 1; i <= SimpleRandomWalkData.CorridorWidth / 2; i++)
            {
                curRoomPositions.Remove(pos + Vector2Int.left * i);
                curRoomPositions.Remove(pos + Vector2Int.left * (i + 2));
                curRoomPositions.Remove(pos + Vector2Int.right * i);
                curRoomPositions.Remove(pos + Vector2Int.right * (i + 2));
            }
            curRoomPositions.Remove(pos + Vector2Int.left * ((SimpleRandomWalkData.CorridorWidth / 2) + 1));
            curRoomPositions.Remove(pos + Vector2Int.right * ((SimpleRandomWalkData.CorridorWidth / 2) + 1));
        }
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
    bool CanReach(Vector2Int start, Vector2Int dest, HashSet<Vector2Int> positions)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);

        int xMax = positions.Max(p => p.x);
        int yMax = positions.Max(p => p.y);
        int xMin = positions.Min(p => p.x);
        int yMin = positions.Min(p => p.y);

        bool[,] visited = new bool[yMax - yMin + 1, xMax - xMin + 1];
        List<Vector2Int> directions = Direction2D.CardinalDirectionsList;
        while (queue.Count > 0)
        {
            Vector2Int pos = queue.Dequeue();
            if (positions.Contains(pos) == false)
                continue;
            if (pos == dest)
                return true;
            Vector2Int cellPos = PosToCell(pos, xMin, yMax);
            if (visited.GetLength(0) <= cellPos.y || visited.GetLength(1) <= cellPos.x || cellPos.y < 0 || cellPos.x < 0)
                continue;
            if (visited[cellPos.y, cellPos.x])
                continue;
            visited[cellPos.y, cellPos.x] = true;
            foreach (var direction in directions)
                queue.Enqueue(pos + direction);
        }
        return false;
    }
    Vector2Int PosToCell(Vector2Int pos, int xMin, int yMax)
    {
        return new Vector2Int(pos.x - xMin, yMax - pos.y);
    }
    Vector2Int CellToPos(Vector2Int cell, int xMin, int yMax)
    {
        return new Vector2Int(cell.x + xMin, yMax - cell.y);
    }
    List<List<Vector2Int>> FindConnectedRegions(HashSet<Vector2Int> positions)
    {
        int xMax = positions.Max(p => p.x);
        int yMax = positions.Max(p => p.y);
        int xMin = positions.Min(p => p.x);
        int yMin = positions.Min(p => p.y);

        bool[,] visited = new bool[yMax - yMin + 1, xMax - xMin + 1];
        List<Vector2Int> directions = Direction2D.CardinalDirectionsList;
        List<List<Vector2Int>> regions = new List<List<Vector2Int>>();
        for (int y = 0; y < visited.GetLength(0); y++)
        {
            for (int x = 0; x < visited.GetLength(1); x++)
            {
                Vector2Int cellPos = new Vector2Int(x, y);
                if (cellPos.y < 0 || cellPos.y >= visited.GetLength(0) ||
                    cellPos.x < 0 || cellPos.x >= visited.GetLength(1))
                    continue;
                if (visited[cellPos.y, cellPos.x])
                    continue;

                Queue<Vector2Int> queue = new Queue<Vector2Int>();
                List<Vector2Int> region = new List<Vector2Int>();
                Vector2Int pos = CellToPos(cellPos, xMin, yMax);
                queue.Enqueue(pos);

                while (queue.Count > 0)
                {
                    pos = queue.Dequeue();
                    if (positions.Contains(pos) == false)
                        continue;
                    cellPos = PosToCell(pos, xMin, yMax);
                    if (visited[cellPos.y, cellPos.x])
                        continue;
                    region.Add(pos);
                    visited[cellPos.y, cellPos.x] = true;
                    foreach (var direction in directions)
                        queue.Enqueue(pos + direction);
                }
                regions.Add(region);
            }
        }
        return regions;
    }
}
