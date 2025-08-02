using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap FloorTilemap, WallTilemap;
    [SerializeField]
    private TileBase FloorTile, WallTile;
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPos)
    {
        PaintFloorTiles(floorPos, FloorTilemap, FloorTile);
    }

    private void PaintFloorTiles(IEnumerable<Vector2Int> floorPos, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in floorPos)
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePos = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePos, tile);
    }
    public void Clear()
    {
        FloorTilemap.ClearAllTiles();
        WallTilemap.ClearAllTiles();
    }

    public void PaintSigleBasicWall(Vector2Int pos)
    {
        PaintSingleTile(WallTilemap, WallTile, pos);
    }
    public void GenerateWalls(HashSet<Vector2Int> floorPositions)
    {
        var basicWallPos = FindWallsInDirections(floorPositions, Direction2D.CardinalDirectionsList);
        foreach (var pos in basicWallPos)
        {
            PaintSigleBasicWall(pos);
        }
    }
    private HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPos, List<Vector2Int> directions)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var pos in floorPos)
        {
            foreach (var direction in directions)
            {
                Vector2Int neighborPos = pos + direction;
                if (!floorPos.Contains(neighborPos))
                {
                    wallPositions.Add(neighborPos);
                }
            }
        }
        return wallPositions;
    }
}
