using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomWalkParameters_",menuName = "PCG - RandomWalkData")]
public class RandomWalkSO : ScriptableObject
{
    public int Iterations = 10;
    public int WalkLength = 10;
    public bool RandomlyStartEachIteration = true;
}
