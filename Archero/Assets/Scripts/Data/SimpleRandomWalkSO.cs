using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkParameters_", menuName = "PCG/SimpleRandomWalkData")]
public class SimpleRandomWalkSO : ScriptableObject
{
    public int Iterations = 10, WalkLength = 10;
    public bool StartRandomlyEachIteration = true;
    public int RoomSpacing = 50;
    [Range(1, 9)]
    public int CorridorWidth = 3;
    public int CorridorCount = 5;
}
