using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurfaceTest : MonoBehaviour
{
    public NavMeshSurface surfaces;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
/*        Component[] components = GetComponents<Component>();
        foreach (Component comp in components)
        {
            Debug.Log(comp.GetType().Name);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        surfaces.BuildNavMesh();
    }
}
