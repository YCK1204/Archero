using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : SimpleDungeonGenerator
{
    List<HashSet<Vector2Int>> _roomPositions;
    List<HashSet<Vector2Int>> _corridorPositions;

    static MapManager _instance = null;
    public static MapManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MapManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("MapManager");
                    _instance = obj.AddComponent<MapManager>();
                }
            }
            return _instance;
        }
    }
    private void Start()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(TilemapVisualizer);
    }
    public void GenerateMap()
    {
        TilemapVisualizer.Clear();
        var data = CorridorFirstGeneration();

        _roomPositions = data.Item1;
        _corridorPositions = data.Item2;
    }
}
