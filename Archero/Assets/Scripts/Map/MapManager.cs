using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using UnityEngine;

public class MapManager : SimpleDungeonGenerator
{
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
        SetDoors();
    }
    void SetDoors()
    {
        Defines.ResourceManager.GetInstance.LoadAsync<GameObject>("Door", (g) =>
        {
            var parent = new GameObject();
            parent.name = "DoorsParent";
            parent.transform.position = Vector3.zero;

            var go = g.GetComponent<Door>();
            foreach (var data in _mapData)
            {
                var corridor = data.Corridor;
                if (corridor != null)
                {
                    Door enterDoor = GameObject.Instantiate(go, parent.transform);
                    Door exitDoor = GameObject.Instantiate(go, parent.transform);

                    corridor.StartDoor = enterDoor;
                    corridor.EndDoor = exitDoor;
                    enterDoor.transform.position = (Vector3Int)corridor.StartPosition;
                    exitDoor.transform.position = (Vector3Int)corridor.EndPosition;

                    enterDoor.Init(parent, corridor.StartPosition, corridor.EndPosition, true);
                    exitDoor.Init(parent, corridor.StartPosition, corridor.EndPosition, false);
                }
            }
        });
    }
}
