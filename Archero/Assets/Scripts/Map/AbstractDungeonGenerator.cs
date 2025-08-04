using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer TilemapVisualizer = null;
    [SerializeField]
    protected Vector2Int StartPos = Vector2Int.zero;
    [SerializeField]
    protected int DungeonWidth = 20, DungeonHeight = 20;
    public void GenerateDungeon()
    {
        TilemapVisualizer.Clear();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
}
