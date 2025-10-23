using System;
using System.Collections;
using UnityEngine;

public class FruitTreeController : MonoBehaviour
{
    private readonly int FruitStageOffset = Enum.GetValues(typeof(TreeTypes)).Length;
    public enum TreeTypes
    {
        Random, Apple, Cherry, Peach, Pear
    } 
    [Header("Growth Stages")]
    [SerializeField] private Transform[] _growthStages;
    [SerializeField] private TreeTypes _treeType;
    [SerializeField] private int _currentStage = 0;
    [SerializeField] private float _stageDelay = 2f;
    [SerializeField] private bool _nextStage;
    [SerializeField] private bool _isCycleActive = true;

    void Start()
    {
        _growthStages = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _growthStages[i] = transform.GetChild(i);
        }
        if (_growthStages.Length == 0)
        {
            Debug.LogError("No growth stage sprites assigned!");
            enabled = false;
            return;
        }        
        StartCoroutine(GrowTreeCoroutine());
    }
    private IEnumerator GrowTreeCoroutine()
    {        
        while(enabled)
        {
            if (_isCycleActive)
            {
                ActivateCurrentStage();
                yield return new WaitUntil(() => _nextStage);
                _nextStage = false;

                yield return new WaitForSeconds(_stageDelay);
                GrowTree();
            }
            else
                yield return null;
        }
    }
    private void GrowTree()
    {
        if (_currentStage <  _growthStages.Length -FruitStageOffset)
        {
            _currentStage++;
        } 
        if (_currentStage == _growthStages.Length -FruitStageOffset)
        {
            var index = (int)_treeType;

            if (index == 0)
            {
                index = UnityEngine.Random.Range(1, FruitStageOffset);
            }
            _currentStage += index;
        }
    }
    public void TriggerNextStage()
    {
        _nextStage = true;
    }
    private void ActivateCurrentStage()
    {
        ResetTree();
        if (_currentStage >= 0 && _currentStage < _growthStages.Length)
        {
            _growthStages[_currentStage].gameObject.SetActive(true);
        }            
    }
    private void ResetTree()
    {
        foreach (var stage in _growthStages)
        {
            stage.gameObject.SetActive(false);
        }
    }
}
