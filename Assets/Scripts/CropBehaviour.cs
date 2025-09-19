using System;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    public Seed cropData;

    // [0] = ¾¾¾Ñ, [1] = »õ½Ï, ...
    public GameObject[] growthStageObjects;

    private int currentGrowthStage = 0;
    private int daysSincePlanted = 0;
    public bool isEaten = false;

    private void Start()
    {
        Plant(cropData);
    }

    private void OnDisable()
    {
    }

    public void Plant(Seed dataToPlant)
    {
        cropData = dataToPlant;
        currentGrowthStage = 0;
        daysSincePlanted = 0;
        UpdateGrowthVisuals();
        Debug.Log(cropData.cropName + "À»(¸¦) ½É¾ú½À´Ï´Ù.");
    }

    public void Grow(int newDay)
    {
        if(isEaten) return;
        if (IsFullyGrown()) return;

        daysSincePlanted++;

        int growthLevel = growthStageObjects.Length;
        if (cropData.growthDays <= 0) return;

        int newStage = Mathf.FloorToInt((float)daysSincePlanted / cropData.growthDays * growthLevel);

        if (newStage > currentGrowthStage)
        {
            currentGrowthStage = newStage;
            UpdateGrowthVisuals();
        }
    }

    private void UpdateGrowthVisuals()
    {        
        currentGrowthStage = Mathf.Clamp(currentGrowthStage, 0, growthStageObjects.Length-1);

        for (int i = 1; i < growthStageObjects.Length; i++)
        {
            if (growthStageObjects[i] != null)
            {
                growthStageObjects[i].SetActive(i-1 == currentGrowthStage);
                gameObject.GetComponent<SpawnSqueeze>().CallSqueeze();
            }
        }
    }

    public bool IsFullyGrown()
    {
        return daysSincePlanted >= cropData.growthDays-1;
    }

    public void Harmed()
    {
        isEaten = true;
        for (int i = 1; i < growthStageObjects.Length; i++)
        {
            if (growthStageObjects[i] != null)
            {
                growthStageObjects[i].SetActive(false);
            }
        }
        growthStageObjects[0].SetActive(true);
    }
}

