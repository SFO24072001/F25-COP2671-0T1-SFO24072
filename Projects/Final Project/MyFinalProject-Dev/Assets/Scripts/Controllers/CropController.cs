using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropController : MonoBehaviour
{
    [SerializeField] private bool _isCycleActive = true;

    bool _newDay = false;
    public List<CropBlock> _activeBlocks = new List<CropBlock>();


    void Start()
    {
        var rndSeed = System.Guid.NewGuid().GetHashCode();
        Random.InitState(rndSeed);

        StartCoroutine(UpdateCropRoutine());
    }

    private IEnumerator UpdateCropRoutine()
    {
        while (enabled)
        {
            if (_isCycleActive)
            {
                foreach (var block in _activeBlocks)
                {
                    block.AdvanceCrop();
                }
                _newDay = false;
                yield return new WaitUntil(() => _newDay);
            }
            else
                yield return null;
        }
    }
}
