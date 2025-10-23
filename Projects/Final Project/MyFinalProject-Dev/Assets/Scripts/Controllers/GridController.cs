using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Tilemaps;
using static Crops;

public class GridController : SingletonMonoBehaviour<GridController>
{
    private static readonly Vector3 BLOCK_CENTER_OFFSET = new Vector3(0.5f, 0.5f, 0);
    private static readonly Vector2 BLOCK_CHECK_SIZE = new(0.9f, 0.9f);
    public GridBlock[,] GridBlocks { get; protected set; }
    public Vector2Int GridSize { get; protected set; }
    public bool IsInitialized { get; protected set; } = false;

    [SerializeField] Transform _gridMin, _gridMax, _gridParent;
    [SerializeField] LayerMask _gridBlockers = 0;

    protected override void InitializeAfterAwake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {   
        Random.InitState(System.Guid.NewGuid().GetHashCode());
        InitializeGrid();
    }
    void InitializeGrid()
    {
        var roundedMin = new Vector3(Mathf.Round(_gridMin.position.x), Mathf.Round(_gridMin.position.y));
        var roundedMax = new Vector3(Mathf.Round(_gridMax.position.x), Mathf.Round(_gridMax.position.y));
        var gridSize = new Vector2Int(Mathf.RoundToInt(roundedMax.x - roundedMin.x),
                                   Mathf.RoundToInt(roundedMax.y - roundedMin.y));

        var generator = new GridGenerator(_gridParent, roundedMin + BLOCK_CENTER_OFFSET, gridSize);
        GridBlocks = generator.GenerateGridUsingGridBlockers(_gridBlockers);
        GridSize = generator.GridSize;
        IsInitialized = true;
    }
    public GridBlock GetBlock(Vector2Int location)
    {
        if (location.x < 0 || location.y < 0 || location.x >= GridSize.x || location.y >= GridSize.y)
            return null;

        return GridBlocks[location.x, location.y];
    }
    public GridBlock GetBlock(Vector3 position)
    {
        var x = Mathf.RoundToInt(position.x - 0.5f) - Mathf.RoundToInt(position.x);
        var y = Mathf.RoundToInt(position.y - 0.5f) - Mathf.RoundToInt(position.y);

        return GetBlock(new Vector2Int(x, y));
    }
    public GridBlock GetRandomBlock()
    {
        if (IsInitialized is false) return null;

        var location = new Vector2Int(Random.Range(0, GridSize.x), Random.Range(0, GridSize.y));
        return GetBlock(location);
    }    
    public void HideVisuals() => _gridParent.gameObject.SetActive(false);
    public void ShowVisuals() => _gridParent.gameObject.SetActive(true);
}


