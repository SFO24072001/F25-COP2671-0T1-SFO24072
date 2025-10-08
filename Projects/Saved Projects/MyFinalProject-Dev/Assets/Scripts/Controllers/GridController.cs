using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Crops;

public class GridController : SingletonMonobehaviour<GridController>
{
    [SerializeField] Transform minPoint, maxPoint, visuals;
    [SerializeField] CropBlock baseGridBlock;    
    [SerializeField] List<BlockRow> blockRows = new();
    [SerializeField] List<CropBlock> activeCrops = new();

    [SerializeField] string farmLandTilemapName;
    Tilemap farmLandTilemap;


    Vector2Int gridSize;
    bool isInitialized = false;
    bool newDay = false;
    protected override void Initialize()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        farmLandTilemap = TilemapFinder(farmLandTilemapName);
        StartCoroutine(UpdateCropRoutine());
        GenerateGrid();
    }
    void GenerateGrid()
    {
        minPoint.position = new Vector3(Mathf.Round(minPoint.position.x), Mathf.Round(minPoint.position.y));
        maxPoint.position = new Vector3(Mathf.Round(maxPoint.position.x), Mathf.Round(maxPoint.position.y));

        var startPoint = minPoint.position + new Vector3(0.5f, 0.5f, 0);

        gridSize = new Vector2Int(Mathf.RoundToInt(maxPoint.position.x - minPoint.position.x),
                                  Mathf.RoundToInt(maxPoint.position.y - minPoint.position.y));

        for (int y = 0; y != gridSize.y; y++)
        {
            var blockRow = new BlockRow();
            for (int x = 0; x != gridSize.x; x++)
            {
                var tilePosition = new Vector3Int(Mathf.CeilToInt(x + minPoint.position.x), Mathf.CeilToInt(y + minPoint.position.y), 0);
                var tile = farmLandTilemap.GetTile(tilePosition);
                if (tile != null)
                {
                    var newBlock = Instantiate(baseGridBlock, startPoint + new Vector3(x, y, 0), Quaternion.identity, visuals);
                    newBlock.location = new Vector2Int(x, y);
                    newBlock.soilSR.sprite = null;
                    newBlock.isFarmland = true;
                    newBlock.currentStage = GrowthStage.ploughed;
                    blockRow.blocks.Add(newBlock);
                }
            }
            blockRows.Add(blockRow);
        }
        isInitialized = true;
    }
    public static Tilemap TilemapFinder(string tileMapName)
    {
        var tileMapObject = GameObject.Find(tileMapName);
        if (tileMapObject == null) return null;

        return tileMapObject.GetComponent<Tilemap>();
    }
    public void AddGrowblock(CropBlock growBlock)
    {
        if (growBlock is null) return;

        activeCrops.Add(growBlock);
    }
    public CropBlock GetBlock(Vector3 position)
    {
        var x = Mathf.RoundToInt(position.x - 0.5f);
        var y = Mathf.RoundToInt(position.y - 0.5f);

        x -= Mathf.RoundToInt(minPoint.position.x);
        y -= Mathf.RoundToInt(minPoint.position.y);

        if (x < gridSize.x && y < gridSize.y)
            return blockRows[y].blocks[x];

        return null;
    }
    public CropBlock GetBlock(Vector2Int location)
    {
        if (location.x < gridSize.x && location.y < gridSize.y)
            return blockRows[location.y].blocks[location.x];

        return null;
    }
    public CropBlock GetRandomBlock()
    {
        if (isInitialized is false) return null;

        var rndSeed = System.Guid.NewGuid().GetHashCode();
        Random.InitState(rndSeed);

        var location = new Vector2Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y));
        var block = GetBlock(location);

        if (block != null)
        {
            if (!block.preventUse && block.currentStage == GrowthStage.barren)
            {
                return block;
            }
        }
        return null;
    }
    private IEnumerator UpdateCropRoutine()
    {
        while (true)
        {
            foreach (var block in activeCrops)
            {
                block.AdvanceCrop();
            }
            newDay = false;
            yield return new WaitUntil(() => newDay);
        }
    }
    public void HideVisuals() => visuals.gameObject.SetActive(false);
    public void ShowVisuals() => visuals.gameObject.SetActive(true);

}
[System.Serializable]
public class BlockRow
{
    public List<CropBlock> blocks = new List<CropBlock>();
}


