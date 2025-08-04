using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavCach : MonoBehaviour
{
    public CollectSourcesCache2d cacheSources2D;

    private void Update()
    {
        if (cacheSources2D.IsDirty)
        {
            cacheSources2D.UpdateNavMesh();
        }
    }
}
