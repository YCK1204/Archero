using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

/// <summary>
/// 타일맵에서 좁은 복도를 기준으로 방을 분리하고, 각 방마다 순찰 경로를 생성하는 클래스
/// </summary>
public class RoomDetector : MonoBehaviour
{
    public Tilemap tilemap;

    [Header("방 인식 기준")]
    public int minRoomWidth = 4;
    public int minRoomHeight = 4;

    [Header("복도 판단 기준")]
    public int corridorThreshold = 3;

    private BoundsInt bounds;
    private int width, height;
    private bool[,] visited;
    public List<Room> rooms = new();

    void Start()
    {
        FindRooms();
        GeneratePatrolPaths();
    }

    void FindRooms()
    {
        bounds = tilemap.cellBounds;
        width = bounds.size.x;
        height = bounds.size.y;
        visited = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int cell = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);
                if (visited[x, y]) continue;
                if (tilemap.GetTile(cell) == null) continue;
                if (IsCorridor(cell)) continue;

                Room room = new Room();
                FloodFill(cell, room);

                if (room.tiles.Count > 0)
                {
                    Vector3Int min = room.tiles[0];
                    Vector3Int max = room.tiles[0];

                    foreach (var t in room.tiles)
                    {
                        min = Vector3Int.Min(min, t);
                        max = Vector3Int.Max(max, t);
                    }

                    int roomWidth = max.x - min.x + 1;
                    int roomHeight = max.y - min.y + 1;

                    if (roomWidth >= minRoomWidth && roomHeight >= minRoomHeight)
                    {
                        rooms.Add(room);
                    }
                }
            }
        }
    }

    void FloodFill(Vector3Int start, Room room)
    {
        Queue<Vector3Int> queue = new();
        queue.Enqueue(start);
        MarkVisited(start);
        room.tiles.Add(start);

        Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
        };

        while (queue.Count > 0)
        {
            var curr = queue.Dequeue();

            foreach (var dir in directions)
            {
                var next = curr + dir;
                if (!IsInsideBounds(next)) continue;
                if (IsVisited(next)) continue;
                if (tilemap.GetTile(next) == null) continue;
                if (IsCorridor(next)) continue;

                MarkVisited(next);
                room.tiles.Add(next);
                queue.Enqueue(next);
            }
        }
    }

    bool IsCorridor(Vector3Int pos)
    {
        int walkable = 0;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                Vector3Int check = pos + new Vector3Int(dx, dy, 0);
                if (tilemap.GetTile(check) != null)
                {
                    walkable++;
                }
            }
        }

        // 주변 walkable 타일이 적으면 복도로 간주
        return walkable <= corridorThreshold;
    }

    bool IsInsideBounds(Vector3Int cell)
    {
        int x = cell.x - bounds.xMin;
        int y = cell.y - bounds.yMin;
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    bool IsVisited(Vector3Int cell)
    {
        int x = cell.x - bounds.xMin;
        int y = cell.y - bounds.yMin;
        return visited[x, y];
    }

    void MarkVisited(Vector3Int cell)
    {
        int x = cell.x - bounds.xMin;
        int y = cell.y - bounds.yMin;
        visited[x, y] = true;
    }

    void GeneratePatrolPaths()
    {
        foreach (var room in rooms)
        {
            // 방 중심
            Vector3 center = Vector3.zero;
            foreach (var tile in room.tiles)
                center += tilemap.CellToWorld(tile);
            center /= room.tiles.Count;

            List<Vector3> edgePoints = new();
            foreach (var tile in room.tiles)
            {
                Vector3 worldPos = tilemap.CellToWorld(tile);
                if (Vector3.Distance(worldPos, center) > 1.5f)
                    edgePoints.Add(worldPos);
            }

            edgePoints.Sort((a, b) =>
            {
                float angleA = Mathf.Atan2(a.y - center.y, a.x - center.x);
                float angleB = Mathf.Atan2(b.y - center.y, b.x - center.x);
                return angleA.CompareTo(angleB);
            });

            room.patrolPath = edgePoints;
        }
    }

    void OnDrawGizmos()
    {
        if (rooms == null) return;

        // 순찰 경로 시각화 (기존 코드)
        Gizmos.color = Color.green;
        foreach (var room in rooms)
        {
            for (int i = 0; i < room.patrolPath.Count; i++)
            {
                var from = room.patrolPath[i];
                var to = room.patrolPath[(i + 1) % room.patrolPath.Count];
                Gizmos.DrawLine(from + Vector3.up * 0.25f, to + Vector3.up * 0.25f);
            }
        }

        // 방의 첫 타일 시각화 (추가)
        Gizmos.color = Color.red;
        foreach (var room in rooms)
        {
            if (room.tiles.Count == 0) continue;

            Vector3 firstTileWorld = tilemap.CellToWorld(room.tiles[0]) + Vector3.up * 0.1f;
            Gizmos.DrawSphere(firstTileWorld, 0.15f);
        }
    }

}

/// <summary>
/// 방 데이터를 저장하는 클래스
/// </summary>
[Serializable]
public class Room
{
    public List<Vector3Int> tiles = new();
    public List<Vector3> patrolPath = new();
}
