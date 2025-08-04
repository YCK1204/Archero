using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : SimpleDungeonGenerator
{
    static MapManager _instance = null;
    public struct CorridorDoors
    {
        public Door StartDoor;
        public Door EndDoor;
    }
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
        CorridorFirstGeneration();

        foreach (var map in _mapData)
        {
            Debug.Log($"Map Cell Count: {map.Positions.Count}");
            Debug.Log($"Map Center Position: {map.CenterPosition}");
            if (map.Corridor != null)
            {
                var corridor = map.Corridor;
                Debug.Log($"Corridor Start Position: {corridor.StartPosition}");
                Debug.Log($"Corridor End Position: {corridor.EndPosition}");
                Debug.Log($"Corridor Cell Count: {corridor.Positions.Count}");
            }
        }
    }
    void SetCorridorDoors()
    {
        //foreach (var corridor in _corridorPositions)
        //{
        //    int xMax = corridor.Max(pos => pos.x);
        //    int xMin = corridor.Min(pos => pos.x);
        //    int yMax = corridor.Max(pos => pos.y);
        //    int yMin = corridor.Min(pos => pos.y);


        //}
    }
}
