using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : RandomWalkDungeonGenerator
{
    [SerializeField] private int _corriderLength = 14;
    [SerializeField] private int _corridorCount = 5;
    [SerializeField] [Range(0.1f, 1)] private float _roomPercent = 0.7f; // 1 = 100%
    
    protected override void RunProceduralGeneration()
    {
        CorriderFirstGeneration();
    }

    /// <summary>
    /// Creates two empty HashSets floorTilePositions and potentialGeneratedRooms,
    /// then passes it to BuildCorridors where it's floorTilePositions/potentialGeneratedRooms get determined and stored in a variable called corridors.
    /// Then hashset roomPositions is created which stores the returned hashset of the CreateRooms method.
    /// We then proceed by creating a deadEndPositions list storing all dead ends found by the FindAllDeadEnds method.
    /// Afterwards we call the CreateRoomsAtDeadEnds method which takes the deadEndPositions and roomPositions list, which adds every deadend that is not already a
    /// floor to the roomPositions hashset.
    /// roomPositions is then combined with floorTilePositions via UnionWith() deleting duplicates.
    /// Floortiles get then build by the BuildFloorTiles method and walls are getting build as well by passing in floorTilesPosition and
    /// tilemapVisualizer object as arguments. 
    /// </summary>
    private void CorriderFirstGeneration()
    {
        HashSet<Vector2Int> floorTilePositions = new();
        HashSet<Vector2Int> potentialRoomPositions = new();
        
        List<List<Vector2Int>> corridors = BuildCorridors(floorTilePositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);
        List<Vector2Int> deadEndPositions = FindAllDeadEnds(floorTilePositions);
        CreateRoomsAtDeadEnds(deadEndPositions, roomPositions);
        floorTilePositions.UnionWith(roomPositions);

        for (int i = 0; i < corridors.Count; i++)
        {
            // You can switch between IncreaseCorridorSizeByOne and IncreaseCorridorBrush3by3 and see what you like better.
            // corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);  
            corridors[i] = IncreaseCorridorSizeToThree(corridors[i]);
            floorTilePositions.UnionWith(corridors[i]);
        }
        
        tilemapVisualizer.BuildFloorTiles(floorTilePositions);
        WallGenerator.BuildWalls(floorTilePositions, tilemapVisualizer);
    }

    
    /// <summary>
    /// Creates a List of type Vector2Int which stores deadEndPositions. Then loops through all passed positions of the given floorTilePositions argument
    /// and creates a local variable neighboursCount which tracks how many neighbours each position has. Inside the first loop we create another loop that checks every
    /// direction of each position, if the combination of the position + direction is already in the floorTilePositions we increment the neighboursCount by 1.
    /// At the end we check if the neighboursCount is exactly 1, if true then we add that position to the deadEndPositions list.
    /// </summary>
    /// <param name="floorTilePositions">
    /// Hashset of the entire generated corridors, that is declared in the CorriderFirstGeneration method.
    /// Each tile is stored as a position. E.g (0,0).
    /// </param>
    /// <returns>Every deadEnd found in the given floorTilePositions argument</returns>
    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorTilePositions)
    {
        List<Vector2Int> deadEndPositions = new();

        foreach (var position in floorTilePositions)
        {
            int neighboursCount = 0;

            foreach (var direction in Directions2D.CardinalDirectionsList)
            {
                if (floorTilePositions.Contains(position + direction))
                {
                    neighboursCount++;
                }
            }
            
            if (neighboursCount == 1)
            {
                deadEndPositions.Add(position);
            }        }

        return deadEndPositions;
    }
    
    /// <summary>
    /// We loop through each deadEndPositions. Each entry is checked if it's already contained inside the roomPositions hashSet.
    /// If not we create a HashSet room that which stores the random generated room by our RunRandomWalk method. room is then
    /// added to roomPositions via UnionWith.
    /// </summary>
    /// <param name="deadEndPositions">Has stored every deadEnd position from the before called FindAllDeadEnds method.</param>
    /// <param name="roomPositions">Has stored every Vector2Int coordinates that are determined to be a room</param>
    private void CreateRoomsAtDeadEnds(List<Vector2Int> deadEndPositions, HashSet<Vector2Int> roomPositions)
    {
        foreach (var position in deadEndPositions)
        {
            if (!roomPositions.Contains(position))
            {
                HashSet<Vector2Int> room = RunRandomWalk(_randomWalkParameters, position);
                roomPositions.UnionWith(room);
            }
        }
    }
    
    /// <summary>
    /// Makes the created corridors bigger in size by 
    /// </summary>
    /// <param name="corridor">A list of Vector2Int</param>
    /// <returns></returns>
    private List<Vector2Int> IncreaseCorridorSizeToThree(List<Vector2Int> corridor)
    {
        List<Vector2Int> adjustedCorridors = new();

        for (int i = 0; i < corridor.Count; i++) 
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    Debug.Log(corridor[i] + new Vector2Int(x,y));
                    adjustedCorridors.Add(corridor[i] + new Vector2Int(x,y));
                }
            }
        }

        return adjustedCorridors;
    }
    
    private List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
    {
        List<Vector2Int> adjustedCorridor = new();
        Vector2Int previousDirection = Vector2Int.zero;

        for (int i = 1; i < corridor.Count; i++)
        {
            Vector2Int directionFromCell = corridor[i] - corridor[i - 1];

            if (previousDirection != Vector2Int.zero && directionFromCell != previousDirection)
            {
                // handle corner
                for (int j = -1; j < 2; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        adjustedCorridor.Add(corridor[i - 1] + new Vector2Int(j, k));
                    }

                    previousDirection = directionFromCell;
                }
            }
            else
            {
                // Add a single cell in the direction + 90 degrees
                Vector2Int newCorridorTileOffset = GetDirection90From(directionFromCell);
                adjustedCorridor.Add(corridor[i - 1]);
                adjustedCorridor.Add(corridor[i - 1] + newCorridorTileOffset);

                previousDirection = directionFromCell;
            }
        }

        return adjustedCorridor;
    }

    private Vector2Int GetDirection90From(Vector2Int direction)
    {
       if (direction == Vector2Int.up)
           return Vector2Int.right;
       else if (direction == Vector2Int.right)
           return Vector2Int.down;
       else if (direction == Vector2Int.down)
           return Vector2Int.left;
       else if (direction == Vector2Int.left)
           return Vector2Int.up;
       else
           return Vector2Int.zero; 
    }

    /// <summary>
    /// Creates HashSet roomPositions. Calculates how much rooms are going to be created which is stores in roomsCreationCount.
    /// Creates List roomsToCreate that orders the potentialGeneratedRooms via linq OrderBy giving it pseudo random indentifiers(Guid.NewGuid())
    /// then takes the amount of roomsCreationCount out of the set and finally stores them in the roomsToCreate list with ToList().
    /// After that we loop through the list and apply the RunRandomWalk algorithm on every entry which is then stored in roomPositions HashSet
    /// via UnionWith to prevent duplicates. 
    /// </summary>
    /// <param name="potentialRoomPositions">
    /// The in CorridorFirstGeneration created HashSet passed in as argument, to determine the rooms.
    /// Always a unique amount, that depends on if BuildCorridors had some duplicated corridors (which decreases the potentialRoomPositions count
    /// therefor creates less rooms.
    /// </param>
    /// <returns></returns>
    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new();
        int roomsCreationCount = Mathf.RoundToInt(potentialRoomPositions.Count * _roomPercent);
        
        List<Vector2Int> roomsToCreate =
            potentialRoomPositions.OrderBy(n => Guid.NewGuid()).Take(roomsCreationCount).ToList(); // Probably simplify this with just Random.Range!

        foreach (var roomPos in roomsToCreate)
        {
            HashSet<Vector2Int> roomFloor = RunRandomWalk(_randomWalkParameters, roomPos);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }

    /// <summary>
    /// Builds Corridors.
    /// First iteration always starts at (0,0) based on the startPos which is defined in the base class and gets stored in the potentialRoomPositions HashSet.
    /// We then create a List that stores Lists of Vector2Int.
    /// For loop iterates _corriderCount times. Each iteration a list "corrider" is created which generates random coordinates resembling a corridor
    /// using the RandomWalkCorridor method. The result is stored added in our corridors List.
    /// After each iteration the currentPos is set to the last entry of corridor list to continue from where the algorithm has ended.
    /// Also the currentPos is added to the potentialRoomPositions HashSet. Meaning that every corridor end could spawn a room!
    /// Each iteration of the corridor list is stored inside the floorTilePositions HashSet via UnionWith() to make sure we don't have duplicates.
    /// </summary>
    /// <param name="floorTilePositions">
    /// Hashset of the entire generated corridors, that is declared in the CorriderFirstGeneration method.
    /// Each tile is stored as a position. E.g (0,0).
    /// </param>
    /// <param name="potentialRoomPositions">
    /// Stores the position of the startPos and every endPos of each corrider.
    /// </param>
    /// <returns>The corridors list containing every created corridor</returns>
    private List<List<Vector2Int>> BuildCorridors(HashSet<Vector2Int> floorTilePositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        Vector2Int currentPos = startPos;
        potentialRoomPositions.Add(currentPos);
        List<List<Vector2Int>> corridors = new();

        for (int i = 0; i < _corridorCount; i++)
        {
            List<Vector2Int> corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPos, _corriderLength);
            corridors.Add(corridor);
            currentPos = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPos);
            floorTilePositions.UnionWith(corridor);
        }

        return corridors;
    }
}
