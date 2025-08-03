using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
    [Range(1, 9)]
    [SerializeField]
    private int CorridorWidth = 3;
    [SerializeField]
    bool ShowCorridor;
    [SerializeField]
    bool ShowRoom;
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
        // 8. ���� ���� ������ ���� ��, ���� �� Border��ġ�� ����
        // 9. ���� �翷 min, max + 3 Ȯ��(dfs�� ���ⱸ�κ��� ���� �߽������� ��ȿ�� �˻�) �ߺ��� ĭ ����
        // 10. 9�� Ȯ���ϸ� ��ȿ�� ���� ���ⱸ ����
        // 11. �����Ǹ鼭 ������ �� �ִ� ������ ���� ����(bfs�� ���� ��ġ�κ��� �ʱ��� �̵� �������� ��ȿ�� �˻�)
        // 12. ���� ����

        if (CorridorWidth % 2 != 1)
        {
            Debug.LogError("CorridorWidth ���� �ݵ�� Ȧ������ �մϴ�.");
            return;
        }

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
        List<HashSet<Vector2Int>> corridors = CreateCorridors(roomPositions, mapToMapDirections, roomCenterPositions);

#if UNITY_EDITOR
        if (ShowCorridor)
        {
            foreach (var corridor in corridors)
            {
                foreach (var pos in corridor)
                    floorPos.Add(pos);
                Debug.Log($"corridor length: {corridor.Count}");
            }
        }
        if (ShowRoom)
        {
            foreach (var room in roomPositions)
            {
                foreach (var pos in room)
                    floorPos.Add(pos);
            }
        }
#endif
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
        List<HashSet<Vector2Int>> roomPositions = new List<HashSet<Vector2Int>>();

        foreach (var pos in roomCenterPositions)
        {
            var roomFloor = RunRandomWalk(pos);
            roomPositions.Add(roomFloor);
        }
        return roomPositions;
    }
    private List<HashSet<Vector2Int>> CreateCorridors(List<HashSet<Vector2Int>> roomPositions, List<Vector2Int> directions, List<Vector2Int> roomCenterPositions)
    {
        List<HashSet<Vector2Int>> corridors = new List<HashSet<Vector2Int>>();

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
    HashSet<Vector2Int> CreateCorridor(HashSet<Vector2Int> curRoomPositions,
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

        while (curPos != endPos)
        {
            corridorPositions.Add(curPos);
            if (curPos != startPos)
            {
                for (int i = 1; i <= CorridorWidth / 2; i++)
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
        return corridorPositions;
    }

    Vector2Int EnsureCorridorEndPosition(Vector2Int curPos, Vector2Int endPos, HashSet<Vector2Int> curRoomPositions, Vector2Int direction)
    {
        Vector2Int startPos = curPos;

        bool horizontal = direction.x != 0;
        while (curPos != endPos)
        {
            RemoveCorridorSide(curRoomPositions, curPos, horizontal);
            if (Bfs(curPos + direction, endPos, curRoomPositions) == true)
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
            if (Bfs(curPos + direction, endPos, curRoomPositions) == true)
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
            for (int i = 1; i <= CorridorWidth / 2; i++)
            {
                curRoomPositions.Remove(pos + Vector2Int.up * i);
                curRoomPositions.Remove(pos + Vector2Int.up * (i + 2));
                curRoomPositions.Remove(pos + Vector2Int.down * i);
                curRoomPositions.Remove(pos + Vector2Int.down * (i + 2));
            }
            curRoomPositions.Remove(pos + Vector2Int.up * ((CorridorWidth / 2) + 1));
            curRoomPositions.Remove(pos + Vector2Int.down * ((CorridorWidth / 2) + 1));
        }
        else
        {
            for (int i = 1; i <= CorridorWidth / 2; i++)
            {
                curRoomPositions.Remove(pos + Vector2Int.left * i);
                curRoomPositions.Remove(pos + Vector2Int.left * (i + 2));
                curRoomPositions.Remove(pos + Vector2Int.right * i);
                curRoomPositions.Remove(pos + Vector2Int.right * (i + 2));
            }
            curRoomPositions.Remove(pos + Vector2Int.left * ((CorridorWidth / 2) + 1));
            curRoomPositions.Remove(pos + Vector2Int.right * ((CorridorWidth / 2) + 1));
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
    bool Bfs(Vector2Int start, Vector2Int dest, HashSet<Vector2Int> positions)
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
            Vector2Int cellPos = new Vector2Int(pos.x - xMin, yMax - pos.y);
            if (visited.GetLength(0) <= cellPos.y || visited.GetLength(1) <= cellPos.x || cellPos.y < 0 || cellPos.x < 0)
                continue;
            if (visited[cellPos.y, cellPos.x])
                continue;
            visited[cellPos.y, cellPos.x] = true;
            foreach (var direction in directions)
            {
                Vector2Int next = pos + direction;
                Vector2Int nextCell = new Vector2Int(next.x - xMin, yMax - next.y);
                if (nextCell.y < 0 || nextCell.y >= visited.GetLength(0) ||
                    nextCell.x < 0 || nextCell.x >= visited.GetLength(1))
                    continue;
                if (visited[nextCell.y, nextCell.x])
                    continue;

                queue.Enqueue(next);
            }
        }
        return false;
    }
}
