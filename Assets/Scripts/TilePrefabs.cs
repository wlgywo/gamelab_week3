using JetBrains.Annotations;
using System;
using UnityEngine;

public class TilePrefabs : MonoBehaviour
{
    public bool isDirtSpawned = false;
    public bool isSeedSpawned = false;
    public bool isWatered = false;
    public Material wateredMaterial;
    public Material defaultMaterial;

    public bool isOccupiedByGiantCrop = false; // 거대 작물 여부
    public bool isWateredToday = false;
    public CropBehaviour GetContainedCrop()
    {
        if (isSeedSpawned)
        {
            return GetComponentInChildren<CropBehaviour>();
        }
        return null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TimeManager.Instance.OnDayEnd += DayChangeHappend;
        WeatherManager.Instance.OnRainStarted += GetWet;
        WeatherManager.Instance.OnRainStopped += Dry;
        // GiantCropManager가 존재할 때만 자신을 등록합니다.
        if (GiantCropManager.Instance != null)
        {
            GiantCropManager.Instance.RegisterTile(this);
        }
    }

    private void OnDisable()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnDayEnd -= DayChangeHappend;
            WeatherManager.Instance.OnRainStarted -= GetWet;
            WeatherManager.Instance.OnRainStopped -= Dry;
        }
        // GiantCropManager가 존재할 때만 자신을 등록 해제합니다.
        if (GiantCropManager.Instance != null)
        {
            GiantCropManager.Instance.UnregisterTile(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetWet()
    {
        GetComponent<SpriteRenderer>().material = wateredMaterial;
        isWatered = true;
        isWateredToday = true;
    }

    public void GetWetByPlayer()
    {
        if (!isWatered)
        {
            GetComponent<SpriteRenderer>().material = wateredMaterial;
            isWatered = true;
            isWateredToday = true;
        }
    }
    private void Dry()
    {
        GetComponent<SpriteRenderer>().material = defaultMaterial;
        isWatered = false;
        isWateredToday = false;
    }
    public void DayChangeHappend(int newDay)
    {
        if (isWatered && gameObject.GetComponentInChildren<CropBehaviour>()!=null)
            gameObject.GetComponentInChildren<CropBehaviour>().Grow(newDay);
        Dry();
    }
}
