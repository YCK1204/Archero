using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public static class ProceduralGenerationAlgorihms
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPos);
        var prevPos = startPos;

        for (int i = 0; i < walkLength; i++)
        {
            var newPos = prevPos + Direction2D.GetRandomCardinalDirection();
            path.Add(newPos);
            prevPos = newPos;
        }
        return path;
    }
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int corridorLength, Vector2Int direction)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();

        var currentPos = startPos;
        corridor.Add(currentPos);

        for (int i = 0; i < corridorLength; i++)
        {
            currentPos += direction;
            corridor.Add(currentPos);
        }
        return corridor;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> CardinalDirectionsList = new List<Vector2Int>
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return CardinalDirectionsList[Random.Range(0, CardinalDirectionsList.Count)];
    }
    public static Vector2Int GetRandomCardinalDirectionExceptPrevDirection(Vector2 prevDirection)
    {
        while (true)
        {
            var direction = GetRandomCardinalDirection();
            if (direction != -prevDirection)
            {
                return direction;
            }
        }
    }
}