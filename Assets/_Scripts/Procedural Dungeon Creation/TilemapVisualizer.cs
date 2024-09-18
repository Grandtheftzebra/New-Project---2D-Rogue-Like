using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [Header("Floor")]
    [SerializeField] private Tilemap _floorTilemap;
    [SerializeField] private TileBase _floorTile;
    [Header(("Walls"))]
    [SerializeField] private Tilemap _wallTilemap;
    [SerializeField] private TileBase _wallTop;
    [SerializeField] private TileBase _wallSideRight;
    [SerializeField] private TileBase _wallSideLeft;
    [SerializeField] private TileBase _wallBottom;
    [SerializeField] private TileBase _wallFull;
    [Header("Corners")] 
    [SerializeField] private TileBase _wallInnerCornerDownLeft;
    [SerializeField] private TileBase _wallInnerCornerDownRight;
    [SerializeField] private TileBase _wallDiagonalCornerDownLeft;
    [SerializeField] private TileBase _wallDiagonalCornerDownRight;
    [SerializeField] private TileBase _wallDiagonalCornerUpLeft;
    [SerializeField] private TileBase _wallDiagonalCornerUpRight;

    public void ClearTilemap()
    {
        _floorTilemap.ClearAllTiles();
        _wallTilemap.ClearAllTiles();
    }
    
    /// <summary>
    /// FloorTile builder. Takes a collection of positions and passes it to the BuildTiles helper method.
    /// BuildTiles takes 2 more parameters.
    /// _floorTilemap: Simply the floorTilemap determined in the inspector
    /// _floorTile: Simply the tile determined in the Inspector 
    /// </summary>
    /// <param name="floorTilePositions">
    /// Hashset of the entire generated floor, each tile is stored as a position. E.g (0,0).
    /// Decided for IEnumerable to be less restrictive, so an argument doesn't necessarily has to be a HashSet.
    /// It's just important that the collection stores Vector2Int data. 
    /// </param>
    public void BuildFloorTiles(IEnumerable<Vector2Int> floorTilePositions)
    {
        BuildTiles(floorTilePositions, _floorTilemap, _floorTile);
    }

    /// <summary>
    /// Loops through each position of the in BuildFloorTiles passed collection and applies the BuildSingleTile method
    /// on it to build the tile.
    /// </summary>
    /// <param name="positions">Collection of Vector2Int positions. E.g (0,0)</param>
    /// <param name="tilemap">The determined tilemap in the inspector, which is passed to BuildTiles</param>
    /// <param name="tile">The determined tile in the inspector, which is passed to BuildTiles</param>
    private void BuildTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            BuildSingleTile(tilemap, tile, position);
        }
    }

    /// <summary>
    /// The Tilemap component in Unity has a grid layout, and each cell in this grid can hold a tile.
    /// The WorldToCell method takes a Vector3 representing a position in world space and translates it into a Vector3Int,
    /// which represents the indices of the cell in the tilemap grid where that position corresponds.
    /// It then sets the tile to the translated position.
    /// </summary>
    /// <param name="tilemap">The determined tilemap in the inspector, which is passed to BuildTiles</param>
    /// <param name="tile">The determined tile in the inspector, which is passed to BuildTiles</param>
    /// <param name="position">A single position. E.g. (0,0).</param>
    private void BuildSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        Vector3Int tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }


    /// <summary>
    /// Uses the BuildSingleTile method to convert each position into the correct cell for the tilemap grid.
    /// And then sets the given tile based on the tilemap and tile it's given as an argument.
    /// </summary>
    /// <param name="position">A single position. E.g. (0,0).</param>
    public void BuildSingleBasicWall(Vector2Int position, string binaryType)
    {
        int binaryTypeToInt = Convert.ToInt32(binaryType, 2);
        Debug.Log($"Position: {position} and binaryType: {binaryType}");
        TileBase wallTile = null;
        
        if (WallTypesChecker.wallTop.Contains(binaryTypeToInt)) 
            wallTile = _wallTop;
        else if (WallTypesChecker.wallSideRight.Contains(binaryTypeToInt))
            wallTile = _wallSideRight;
        else if (WallTypesChecker.wallSideLeft.Contains(binaryTypeToInt))
            wallTile = _wallSideLeft;
        else if (WallTypesChecker.wallBottom.Contains(binaryTypeToInt))
            wallTile = _wallBottom;
        else if (WallTypesChecker.wallFull.Contains(binaryTypeToInt))
            wallTile = _wallFull;
        
        if (wallTile != null)
            BuildSingleTile(_wallTilemap, wallTile, position);
    }

    public void BuildSingleCornerWall(Vector2Int position, string binaryType)
    {
        int binaryTypeToInt = Convert.ToInt32(binaryType, 2);

        TileBase cornerWallTile = null;

        if (WallTypesChecker.wallInnerCornerDownLeft.Contains(binaryTypeToInt))
            cornerWallTile = _wallInnerCornerDownLeft;
        else if (WallTypesChecker.wallInnerCornerDownRight.Contains(binaryTypeToInt))
            cornerWallTile = _wallInnerCornerDownRight;
        else if (WallTypesChecker.wallDiagonalCornerDownLeft.Contains(binaryTypeToInt))
            cornerWallTile = _wallDiagonalCornerDownLeft;
        else if (WallTypesChecker.wallDiagonalCornerDownRight.Contains(binaryTypeToInt))
            cornerWallTile = _wallDiagonalCornerDownRight;
        else if (WallTypesChecker.wallDiagonalCornerUpLeft.Contains(binaryTypeToInt))
            cornerWallTile = _wallDiagonalCornerUpLeft;
        else if (WallTypesChecker.wallDiagonalCornerUpRight.Contains(binaryTypeToInt))
            cornerWallTile = _wallDiagonalCornerUpRight;
        else if (WallTypesChecker.wallFullEightDirections.Contains(binaryTypeToInt))
            cornerWallTile = _wallFull;
        else if (WallTypesChecker.wallBottomEightDirections.Contains(binaryTypeToInt))
            cornerWallTile = _wallBottom;
        
        
        
        
        if (cornerWallTile != null)
            BuildSingleTile(_wallTilemap, cornerWallTile, position);
    }
}
