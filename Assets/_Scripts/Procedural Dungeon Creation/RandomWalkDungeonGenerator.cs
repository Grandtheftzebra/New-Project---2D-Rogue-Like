using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class RandomWalkDungeonGenerator : AbstractDungeonGenerator
{
   [SerializeField] protected RandomWalkSO _randomWalkParameters;
   
   protected override void RunProceduralGeneration()
   {
      HashSet<Vector2Int> floorTilePositions = RunRandomWalk(_randomWalkParameters, startPos);
      
      tilemapVisualizer.ClearTilemap();
      tilemapVisualizer.BuildFloorTiles(floorTilePositions);
      WallGenerator.BuildWalls(floorTilePositions, tilemapVisualizer);
   }

   /// <summary>
   ///  Creates a generatedFloor HashSet.
   ///  Runs a for loop based on the _iterations amount specified.
   ///  Each iteration fires the RandomWalk static method declared in ProceduralGenerationAlgorithms.
   ///  RandomWalk takes our currentPos and walkLength, based on those it will generate tiles randomly until the
   ///  walkLength is reached. The result is stored in path. We then combine generatedFloor with path via UnionWith.
   ///  UnionWith() also prevents duplicates. So basically generatedFloor stores each iteration of the RandomWalk
   ///  method in it and kicks out every duplicate. If the _startRandomlyEachIteration flag is active we make sure to always
   ///  start each iteration at a random position based on the actual count of generatedTiles.
   /// </summary>
   /// <param name="parameter">To make RunRandomWalk more modular it takes the RandomWalkSO parameter, so we can use it more easily in other classes,
   ///  as we can simply put in a customized Scriptable Object.
   /// </param>
   /// <param name="pos">Defines the currentPos from which the algorithm starts</param>
   /// <returns>
   /// The generatedFloor HashSet which is then stored in the floorTilePositions HashSet!
   /// </returns>
   protected HashSet<Vector2Int> RunRandomWalk(RandomWalkSO parameter, Vector2Int pos)
   {
      Vector2Int currentPos = pos;
      HashSet<Vector2Int> generatedFloor = new();

      for (int i = 0; i < parameter.Iterations; i++)
      {
         HashSet<Vector2Int> path = ProceduralGenerationAlgorithms.RandomWalk(currentPos, parameter.WalkLength);
         generatedFloor.UnionWith(path);

         if (parameter.RandomlyStartEachIteration)
            currentPos = generatedFloor.ElementAt(Random.Range(0, generatedFloor.Count));
      }

      return generatedFloor;
   }
   
}
