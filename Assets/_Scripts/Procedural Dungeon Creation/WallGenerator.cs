using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator 
{
    /// <summary>
    /// Loops through every position that is determined a wall and will build a basic wall.
    /// </summary>
    /// <param name="floorTilePositions">Hashset of the entire generated floor, each tile is stored as a position. E.g (0,0)</param>
    /// <param name="tilemapVisualizer">The TilemapVisualizer class has stored the methods that will build the tiles</param>
    public static void BuildWalls(HashSet<Vector2Int> floorTilePositions, TilemapVisualizer tilemapVisualizer)
    {
        HashSet<Vector2Int> basicWallPositions = DetermineWallPositions(floorTilePositions, Directions2D.CardinalDirectionsList);
        HashSet<Vector2Int> cornerWallPositions = DetermineWallPositions(floorTilePositions, Directions2D.DiagnoalDirectionsList);

        BuildBasicWalls(floorTilePositions ,tilemapVisualizer, basicWallPositions);
        BuildCornerWalls(floorTilePositions ,tilemapVisualizer, cornerWallPositions);
        
    }

   private static void BuildBasicWalls(HashSet<Vector2Int> floorTilePositions, TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositions)
    {
        foreach (var position in basicWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Directions2D.CardinalDirectionsList)
            {
                Debug.Log($"direction is: {direction}");
                Vector2Int neighbourPos = position + direction;

                if (floorTilePositions.Contains(neighbourPos))
                    neighboursBinaryType += "1";
                else
                    neighboursBinaryType += "0";
            }
            
            tilemapVisualizer.BuildSingleBasicWall(position, neighboursBinaryType); // Continue here, add neighboursBinaryType as Param in BuildSingleBasicWall
        }
    }

    private static void BuildCornerWalls(HashSet<Vector2Int> floorTilePositions, TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions)
    {
        foreach (var position in cornerWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Directions2D.EightDirectionsList)
            {
                Vector2Int neighbourPos = position + direction;

                if (floorTilePositions.Contains(neighbourPos))
                    neighboursBinaryType += "1";
                else
                    neighboursBinaryType += "0";
            }

            tilemapVisualizer.BuildSingleCornerWall(position, neighboursBinaryType);
        }
    }
   
    /// <summary>
    ///  Loop through each floorTile, check it's cardinalDirections and for every position that is not part of the floorTilePositions
    ///  HashSet the algorithm will add the position inside the wallPositions Hashset.
    /// </summary>
    /// <param name="floorTilePositions">Hashset of the entire generated floor</param>
    /// <param name="directionsList">contains north, east, south, west as Vector2Ints</param>
    /// <returns>
    /// A Hashset of every tilePosition that is determined a wall.
    /// </returns>
    private static HashSet<Vector2Int> DetermineWallPositions(HashSet<Vector2Int> floorTilePositions, List<Vector2Int> directionsList)
    {
        HashSet<Vector2Int> wallPositions = new();

        foreach (var position in floorTilePositions)
        {
            foreach (var direction in directionsList)
            {
               Vector2Int neighbourPos = position + direction;
               
               if (!floorTilePositions.Contains(neighbourPos))
                   wallPositions.Add(neighbourPos);
            }
        }

        return wallPositions;
    }
}
