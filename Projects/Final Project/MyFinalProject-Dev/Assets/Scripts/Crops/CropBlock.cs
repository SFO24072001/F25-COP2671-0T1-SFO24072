using System.Collections;
using System.Linq;
using UnityEngine;
using static Crops;

public class CropBlock : MonoBehaviour
{
    public GrowthStage currentStage = GrowthStage.barren;
    public SeedPacket currentSeedPacket;
    public SpriteRenderer soilSR;
    public SpriteRenderer cropSR;
    public Sprite soilTilled, soilWatered;
    public bool isWildCrop = false;
    public Vector2Int location;

    public bool newDay = false;

    public int daysWithoutWater = 0;
    public int daysInStage = 0;

    public bool isFarmland = false;
    public bool soilIsWatered = false;
    public bool preventUse = false;

    private void OnEnable()
    {
        soilSR = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(q => q.name.Contains(Constants.CropBlockSoil));
        cropSR = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(q => q.name.Contains(Constants.CropBlockCrop));
    }
    private void Start()
    {
        // TODO: TimeManager.Events?.OnNewDay?.AddListener(() => newDay = true);
        StartCoroutine(UpdateCropRoutine());
    }
    private IEnumerator UpdateCropRoutine()
    {
        while (isActiveAndEnabled)
        {
            AdvanceCrop();
            newDay = false;
            yield return new WaitUntil(() => newDay);
        }
    }
    public void AdvanceCrop()
    {
        if (preventUse) return;
        if (currentStage == GrowthStage.rotten) return;
        if (isWildCrop is false && soilIsWatered is false)
        {
            daysWithoutWater++;
            return;
        }
        if (daysInStage > 0)
        {
            daysInStage--;
            return;
        }
        daysInStage = AdvanceStage();
    }
    public int AdvanceStage()
    {
        if (preventUse) return 0;
        if (currentSeedPacket == null) return 0;

        currentStage++;
        if ((int)currentStage > (int)GrowthStage.harvest)
            currentStage = GrowthStage.harvest;

        soilIsWatered = false;
        UpdateCropSprite();
        PrepareSoil();

        var cropStageInfo = currentSeedPacket.GetCropStage(currentStage);
        return cropStageInfo.daysInStage;
    }

    public void SetSeedPacket(SeedPacket seedPacket) => currentSeedPacket = seedPacket;
    public void AddToHarvestGrid()
    {
        // TODO: GridController.Instance.AddGrowblock(this);
    }
    public void AdvanceTo(GrowthStage stage)
    {
        if (preventUse) return;
        switch (stage)
        {
            case GrowthStage.barren:
                soilIsWatered = false;
                PrepareSoil();
                break;
            case GrowthStage.ploughed:
                PloughSoil();
                break;
            case GrowthStage.planted:
                PlantCrop();
                break;
            case GrowthStage.sprout:
                AdvanceCrop();
                break;
            case GrowthStage.growing1:
                AdvanceCrop();
                break;
            case GrowthStage.growing2:
                AdvanceCrop();
                break;
            case GrowthStage.harvest:
                HarvestCrop();
                break;
        }
    }
    
    public void PrepareSoil()
    {
        if (preventUse) return;
        if (isWildCrop) return;

        if (currentStage == GrowthStage.barren)
        {
            soilSR.sprite = null;
            cropSR.sprite = null;
        }
        else if (soilIsWatered)
            soilSR.sprite = soilWatered;
        else
            soilSR.sprite = soilTilled;
    }
    public void PloughSoil()
    {
        if (preventUse) return;
        if (currentStage == GrowthStage.barren)
        {
            currentStage = GrowthStage.ploughed;
            PrepareSoil();
        }
    }
    public void WaterSoil()
    {
        if (preventUse) return;

        soilIsWatered = true;
        daysWithoutWater = 0;

        PrepareSoil();
    }
    public void PlantCrop()
    {
        if (preventUse) return;

        if ((currentStage == GrowthStage.ploughed && soilIsWatered is true) || isWildCrop is true)
        {
            currentStage = GrowthStage.planted;
            UpdateCropSprite();
        }
    }
    private void UpdateCropSprite()
    {
        if (preventUse) return;
        if (cropSR == null) return;
        if (currentSeedPacket == null) return;
        if (currentSeedPacket.GetCropStage(currentStage).cropImage == null) return;

        cropSR.sprite = currentSeedPacket.GetCropStage(currentStage).cropImage;
    }
    //public void AdvanceCrop()
    //{
    //    if (preventUse) return;
    //    if (isWildCrop is false && soilIsWatered is false) return;

    //    if (_currentStage == GrowthStage.planted ||
    //        _currentStage == GrowthStage.sprout ||
    //        _currentStage == GrowthStage.growing1 ||
    //        _currentStage == GrowthStage.growing2)
    //    {
    //        _currentStage++;
    //        soilIsWatered = false;
    //        UpdateCropSprite();
    //    }
    //    PrepareSoil();
    //}
    public void AddWildCrop(SeedPacket seedPacket)
    {
        isWildCrop = true;
        currentSeedPacket = seedPacket;
        currentStage = GrowthStage.sprout;

        var cropStageInfo = currentSeedPacket.GetCropStage(currentStage);
        daysInStage = cropStageInfo.daysInStage;
        UpdateCropSprite();
        AddToHarvestGrid();
    }
    public void HarvestCrop()
    {
        if (preventUse) return;
        if (currentStage != GrowthStage.harvest) return;

        currentStage = GrowthStage.barren;
        PrepareSoil();
    }
    
    
}
