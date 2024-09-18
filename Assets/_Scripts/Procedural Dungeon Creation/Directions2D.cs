using System.Collections.Generic;
using UnityEngine;

public static class Directions2D
{
    public static Vector2Int RandomCardinalDirection
    { 
        get => CardinalDirectionsList[Random.Range(0, CardinalDirectionsList.Count)];
    }
    
    public static List<Vector2Int> CardinalDirectionsList = new()
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    public static List<Vector2Int> DiagnoalDirectionsList = new()
    {
        new Vector2Int(1,1), // UP-RIGHT
        new Vector2Int(1,-1), // DOWN-RIGHT
        new Vector2Int(-1,-1), // DOWN-LEFT
        new Vector2Int(-1,1) // UP-LEFT
        
    };

    public static List<Vector2Int> EightDirectionsList = new()
    {
        Vector2Int.up,
        new Vector2Int(1, 1), // UP-RIGHT
        Vector2Int.right,
        new Vector2Int(1, -1), // DOWN-RIGHT
        Vector2Int.down,
        new Vector2Int(-1, -1), // DOWN-LEFT
        Vector2Int.left,
        new Vector2Int(-1, 1) // UP-LEFT
    };
}