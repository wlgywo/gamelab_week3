using System;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    public Seed cropData;

    public GameObject[] growthStageObjects;

    // private int currentGrowthStage = 0;
    public int daysSincePlanted = 0;
    public bool isEaten = false;

    private Vector3[] originalScales;

    private void Start()
    {
        

        originalScales = new Vector3[growthStageObjects.Length];
        for (int i = 0; i < growthStageObjects.Length; i++)
        {
            if (growthStageObjects[i] != null)
            {
                originalScales[i] = growthStageObjects[i].transform.localScale;
            }
        }
        Plant(cropData);
    }

    private void OnDisable()
    {
    }

    public void Plant(Seed dataToPlant)
    {
        cropData = dataToPlant;
        // currentGrowthStage = 0;
        daysSincePlanted = 0;
        UpdateGrowthVisuals();
        Debug.Log(cropData.cropName + "을(를) 심었습니다.");
    }

    /* public void Grow(int newDay)
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
    }*/

    public void Grow(int day)
    {
        if (isEaten || IsFullyGrown()) return;

        daysSincePlanted++;
        UpdateGrowthVisuals();
    }
    private void UpdateGrowthVisuals()
    {
        if (cropData.isGiantCrop) return;
        float progress = 0f;
        if (cropData.growthDays > 0)
        {
            progress = (float)daysSincePlanted / cropData.growthDays;
        }

        growthStageObjects[1]?.SetActive(progress < 0.25f);


        if (progress >= 0.25f && progress < 0.75f)
        {
            growthStageObjects[2]?.SetActive(true);
            float stemScale = (progress < 0.5f) ? 0.5f : 1.0f;
            growthStageObjects[2].transform.localScale = originalScales[2] * stemScale;
        }
        else
        {
            growthStageObjects[2]?.SetActive(false);
        }


        if (progress >= 0.75f)
        {
            growthStageObjects[3]?.SetActive(true);
            float cropScale = IsFullyGrown() ? 1.0f : 0.5f;
            growthStageObjects[3].transform.localScale = originalScales[3] * cropScale;
        }
        else
        {
            growthStageObjects[3]?.SetActive(false);
        }
    }

    public bool IsFullyGrown()
    {
        return daysSincePlanted >= cropData.growthDays;
    }

    public void Harmed()
    {
        isEaten = true;
        for (int i = 1; i < growthStageObjects.Length; i++)
        {
            growthStageObjects[i]?.SetActive(false);
        }
        growthStageObjects[0]?.SetActive(true);
    }
}

