using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridGenerator
{
    private readonly Transform _parent;    
    private readonly Vector2Int _gridSize;
    private readonly Vector3 _startPoint;
    public Vector2Int GridSize => _gridSize;        
    public GridGenerator(Transform parent, Vector3 startPoint, Vector2Int gridSize)
    {
        if (parent == null)
            throw new ArgumentNullException(nameof(parent), "Parent transform cannot be null.");

        if (gridSize.x <= 0 || gridSize.y <= 0)
        {
            Debug.LogWarning("GridGenerator initialized with invalid grid size.");
            gridSize = new Vector2Int(1, 1); // fallback or abort
        }

        _parent = parent;
        _startPoint = startPoint;
        _gridSize = gridSize;
    }
    public GridBlock[,] GenerateGridUsingGridBlockers(LayerMask gridBlockers)
    {
        var gridBlocks = new GridBlock[GridSize.x, GridSize.y];

        for (int y = 0; y < GridSize.y; y++)
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                var position = _startPoint + new Vector3(x, y, 0);
                if (Physics2D.OverlapBox(position, new Vector2(0.9f, 0.9f), 0f, gridBlockers) == false)
                {
                    gridBlocks[x, y] = CreateGridBlock(new Vector2Int(x, y), position);
                }
            }
        }
        return gridBlocks;
    }    
    public GridBlock[,] GenerateGridUsingTilemap(Tilemap tilemap)
    {   
        var tilemapSize = tilemap.cellBounds;
        var width = tilemapSize.size.x;
        var height = tilemapSize.size.y;
        var gridBlocks = new GridBlock[width, height];

        for (int y = tilemapSize.min.y; y < tilemapSize.max.y; y++)
        {
            for (int x = tilemapSize.min.x; x < tilemapSize.max.x; x++)
            {
                var tilePosition = new Vector3Int(x, y);
                var tile = tilemap.GetTile(tilePosition);
                if (tile != null)
                {
                    var position = _startPoint + new Vector3(x, y, 0);
                    var location = new Vector2Int(x - tilemapSize.min.x, y - tilemapSize.min.y);
                    gridBlocks[location.x, location.y] = CreateGridBlock(location, position);
                }
            }
        }
        return gridBlocks;
    }
    private GridBlock CreateGridBlock(Vector2Int location, Vector3 position)
    {
        var newGridBlock = new GameObject().AddComponent<GridBlock>();
        newGridBlock.transform.localPosition = position;
        newGridBlock.transform.SetParent(_parent, false);
        newGridBlock.Initialize(location);
        return newGridBlock;
    }
}
