using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapGridController : MonoBehaviour
{
    private static readonly Vector3 BLOCK_CENTER_OFFSET = new Vector3(0.5f, 0.5f, 0);

    public GridBlock[,] GridBlocks { get; protected set; }
    public Vector2Int GridSize { get; protected set; }
    public bool IsInitialized { get; protected set; } = false;

    private Tilemap _farmLandTilemap;
    private BoundsInt _tilemapSize;

    private void Awake()
    {
        _farmLandTilemap = GetComponent<Tilemap>();
    }
    private void Start()
    {
        CompressAndCalculateBounds();
        InitializeGrid();
    }
    private void CompressAndCalculateBounds()
    {
        _farmLandTilemap.CompressBounds();
        _tilemapSize = _farmLandTilemap.cellBounds;
    }
    void InitializeGrid()
    {
        var generator = new GridGenerator(transform, BLOCK_CENTER_OFFSET, new Vector2Int(_tilemapSize.max.x - _tilemapSize.min.x, _tilemapSize.max.y - _tilemapSize.min.y));
        GridBlocks = generator.GenerateGridUsingTilemap(_farmLandTilemap);
        GridSize = generator.GridSize;
        IsInitialized = true;
    }
}
