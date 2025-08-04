using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    AbstractDungeonGenerator _generator;
    private void Awake()
    {
        _generator = (AbstractDungeonGenerator)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create Dungeon"))
        {
            _generator.GenerateDungeon();
        }
    }
}
#endif
