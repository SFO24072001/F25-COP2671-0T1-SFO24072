using System.Linq;
using UnityEngine;
using static Crops;

public class SeedPacket : MonoBehaviour
{
    public new string name;
    [Range(1, 95)]
    public int highQualtiyChance = 5;
    public CropStageInfo[] cropStages;
    public CropType cropType;
    public int costToBuy;
    public int daysUntilRot;
    public int priceToSell;
    public int priceToSellQuality;    
    public Sprite seedPacketImage;
    public string description;
    public CropStageInfo GetCropStage(GrowthStage growthStage) 
    {
        switch (growthStage) 
        {
            case GrowthStage.sprout:
                return cropStages.FirstOrDefault(q => q.cropStage == CropStage.spout);
            case GrowthStage.planted:
                return cropStages.FirstOrDefault(q => q.cropStage == CropStage.seed);
            case GrowthStage.harvest:
                return cropStages.FirstOrDefault(q => q.cropStage == CropStage.harvest);            
            case GrowthStage.growing1:
                return cropStages.FirstOrDefault(q => q.cropStage == CropStage.grow1);
            case GrowthStage.growing2: 
                return cropStages.FirstOrDefault(q => q.cropStage == CropStage.grow2);
            default:
                return null;
        }
    }
    [System.Serializable]
    public class CropStageInfo
    {
        public CropStage cropStage;        
        public Sprite cropImage;
        public int daysInStage;
    }
}

