using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    Tilemap FloorTilemap, WallTilemap;
    [SerializeField]
    TileBase FloorBlackTile, FloorWhiteTile;
    [SerializeField]
    RuleTile WallTile;
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPos)
    {
        foreach (var position in floorPos)
        {
            var x = Math.Abs(position.x + 10000) % 4;
            var y = Math.Abs(position.y + 10000) % 4;

            if (x < 2)
            {
                if (y < 2)
                {
                    PaintSingleTile(FloorTilemap, FloorBlackTile, position);
                }
                else
                {
                    PaintSingleTile(FloorTilemap, FloorWhiteTile, position);
                }
            }
            else
            {
                if (y < 2)
                {
                    PaintSingleTile(FloorTilemap, FloorWhiteTile, position);

                }
                else
                {
                    PaintSingleTile(FloorTilemap, FloorBlackTile, position);

                }
            }

            //if ((x + y) % 2 == 0)
            //{
            //    PaintSingleTile(FloorTilemap, FloorBlackTile, position);
            //}
            //else
            //{
            //    PaintSingleTile(FloorTilemap, FloorWhiteTile, position);
            //}
        }
    }
    public void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
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
