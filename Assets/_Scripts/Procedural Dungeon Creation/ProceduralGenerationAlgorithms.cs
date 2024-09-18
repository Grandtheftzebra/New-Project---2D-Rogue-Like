using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector2Int> RandomWalk(Vector2Int startPos, int walkLength)
    {
        HashSet<Vector2Int> path = new();
        path.Add(startPos);
        Vector2Int previousPos = startPos;

        for (int i = 0; i < walkLength; i++)
        {
            Vector2Int newPos = previousPos + Directions2D.RandomCardinalDirection;
            path.Add(newPos);
            previousPos = newPos;
        }

        return path;
    }

    /// <summary>
    /// Use List because it is mandatory to keep track of the entries order, as we need the last entry. HashSets don't have order!
    /// Simply creates a list, then declares a direction and sets the currentPos to our initial startPos which gets added to the list.
    /// Then we loop through corridorLength. Each iteration we move one position further in the direction we set at the start.
    /// </summary>
    /// <param name="startPos">Start position of the algorithm. Takes a Vector2Int as argument</param>
    /// <param name="walkLength">Determines iteration length</param>
    /// <returns>A list with coordinates that represent a corridor.</returns>
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int corridorLength)
    {
        List<Vector2Int> corridor = new();
        Vector2Int direction = Directions2D.RandomCardinalDirection;
        Vector2Int currentPos = startPos;
        corridor.Add(currentPos);

        for (int i = 0; i < corridorLength; i++)
        {
            currentPos += direction;
            corridor.Add(currentPos);
        }

        return corridor;
    }

    /// <summary>
    /// Partitions a given space into smaller rooms using a recursive splitting algorithm. This function enqueues the initial space
    /// to be split into a queue and processes each item by checking if it meets the minimum width and height requirements to be considered for splitting.
    /// Rooms are split based on a random decision to favor horizontal or vertical splitting. If a room's dimensions are sufficient for one type of split but not the other,
    /// it will attempt the feasible split. If neither type of split is possible due to the room's dimensions being less than twice the minimum dimensions required,
    /// the room is added directly to the final list without further splitting. The process repeats until no further rooms can be split.
    /// This method may need further enhancements to handle edge cases or specific room dimension requirements more effectively.
    /// </summary>
    /// <param name="spaceToSplit">Defines the initial space to be partitioned, including start position and size.</param>
    /// <param name="minRoomWidth">Specifies the minimum width a room must have to be considered for a vertical split.</param>
    /// <param name="minRoomHeight">Specifies the minimum height a room must have to be considered for a horizontal split.</param>
    /// <returns>A list of rooms, each defined by their size and position within the original space.</returns>
    public static List<BoundsInt> SpatialPartitioning(BoundsInt spaceToSplit, int minRoomWidth, int minRoomHeight)
    {
        Queue<BoundsInt> roomsQueue = new(); // FiFo - First in First out 
        List<BoundsInt> roomsList = new();
        roomsQueue.Enqueue(spaceToSplit);

        while (roomsQueue.Count > 0)
        {
            BoundsInt roomToSplit = roomsQueue.Dequeue();

            if (roomToSplit.size.x >= minRoomWidth && roomToSplit.size.y >= minRoomHeight)
            {
                if (Random.value < 0.5f)
                {
                    if (roomToSplit.size.y >= minRoomHeight * 2)
                        SplitHorizontally(minRoomHeight, roomsQueue, roomToSplit);
                    else if (roomToSplit.size.x >= minRoomWidth * 2)
                        SplitVertically(minRoomWidth, roomsQueue, roomToSplit);
                    else
                        roomsList.Add(roomToSplit);
                }
                else
                {
                    if (roomToSplit.size.x >= minRoomWidth * 2)
                        SplitVertically(minRoomWidth, roomsQueue, roomToSplit);
                    else if (roomToSplit.size.y >= minRoomHeight * 2)
                        SplitHorizontally(minRoomHeight, roomsQueue, roomToSplit);
                    else
                        roomsList.Add(roomToSplit);
                }
            }
        }

        return roomsList;
    }

    /// <summary>
    /// Splits a given room horizontally into two smaller rooms. The horizontal split position (ySplit) is randomly determined within bounds that prevent 
    /// the creation of a new room smaller than half of the minimum room height, calculated as between minRoomHeight * 0.5f and room.size.y - minRoomHeight * 0.5f.
    /// This method ensures that both resulting rooms are at least as tall as half the minimum room height, facilitating further splits. 
    /// Both new rooms are then enqueued back into the roomsQueue for further processing. Debug statements log the split position and room dimensions for development tracking.
    /// </summary>
    /// <param name="minRoomHeight">Used to calculate the lower and upper bounds for the split position to ensure viable room heights post-split.</param>
    /// <param name="roomsQueue">Queue containing rooms to be split further or added to the final list.</param>
    /// <param name="room">The room to be split horizontally.</param>

    private static void SplitHorizontally(int minRoomHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        // Ensure the split does not result in a room smaller than minHeight
        float splitNotRounded = Random.Range(minRoomHeight * 0.5f, room.size.y - minRoomHeight * 0.5f);
        int ySplit = Mathf.RoundToInt(splitNotRounded);

        BoundsInt firstRoom = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));

        BoundsInt secondRoom = new BoundsInt(
            new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
            new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));

        roomsQueue.Enqueue(firstRoom);
        roomsQueue.Enqueue(secondRoom);
    }

    /// <summary>
    /// Splits a given room vertically into two smaller rooms. The vertical split position (xSplit) is randomly determined within bounds that prevent
    /// the creation of a new room narrower than half of the minimum room width, calculated as between minRoomWidth * 0.5f and room.size.x - minRoomWidth * 0.5f.
    /// This method ensures that both resulting rooms are at least as wide as half the minimum room width, facilitating further splits. 
    /// Both new rooms are then enqueued back into the roomsQueue for further splitting or final addition to the rooms list.
    /// Debug statements provide feedback on the split decision and resultant room dimensions for troubleshooting and verification during development.
    /// </summary>
    /// <param name="minRoomWidth">Used to calculate the lower and upper bounds for the split position to ensure viable room widths post-split.</param>
    /// <param name="roomsQueue">Queue for storing rooms that need further splitting or are finalized.</param>
    /// <param name="room">The room to be split vertically.</param>

    private static void SplitVertically(int minRoomWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        // Ensure the split does not result in a room smaller than minWidth
        float splitNotRounded = Random.Range(minRoomWidth * 0.5f, room.size.x - minRoomWidth * 0.5f);
        int xSplit = Mathf.RoundToInt(splitNotRounded);

        BoundsInt firstRoom = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));

        BoundsInt secondRoom = new BoundsInt(
            new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
            new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));

        roomsQueue.Enqueue(firstRoom);
        roomsQueue.Enqueue(secondRoom);
    }
}