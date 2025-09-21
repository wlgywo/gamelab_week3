using JetBrains.Annotations;
using System;
using UnityEngine;

public class TilePrefabs : MonoBehaviour
{
    public bool isDirtSpawned = false;
    public bool isSeedSpawned = false;
    public bool isWatered = false;
    public bool isOccupiedByGiantCrop = false; // �Ŵ� �۹� ����
    public bool isOccupiedByScarecrow = false; // ����ƺ� ����
    public bool isWateredToday = false;
    public bool isFertilized = false; // ��� ����

    public Material wateredMaterial;
    public Material defaultMaterial;
    public Material fertilizedMaterial_Dry;
    public Material fertilizedMaterial_Wet;
    
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
        // FairyManager.Instance.OnFairyEventStarted += DayChangeHappend;
        TimeManager.Instance.OnDayEnd += DayChangeHappend;
        TimeManager.Instance.OnDayStart += DryForDayStart;
        WeatherManager.Instance.OnRainStarted += GetWet;
        WeatherManager.Instance.OnRainStopped += Dry;
        // GiantCropManager�� ������ ���� �ڽ��� ����մϴ�.
        if (GiantCropManager.Instance != null)
        {
            GiantCropManager.Instance.RegisterTile(this);
        }
    }

    private void OnDisable()
    {
        if (TimeManager.Instance != null)
        {
            // FairyManager.Instance.OnFairyEventStarted -= DayChangeHappend;
            TimeManager.Instance.OnDayEnd -= DayChangeHappend;
            TimeManager.Instance.OnDayStart -= DryForDayStart;
            WeatherManager.Instance.OnRainStarted -= GetWet;
            WeatherManager.Instance.OnRainStopped -= Dry;
        }
        // GiantCropManager�� ������ ���� �ڽ��� ��� �����մϴ�.
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
        if(isFertilized)
        {
            GetComponent<SpriteRenderer>().material = fertilizedMaterial_Wet;
        }
        else
            GetComponent<SpriteRenderer>().material = wateredMaterial;
        isWatered = true;
        isWateredToday = true;
    }

    public void GetWetByPlayer()
    {
        if(isFertilized)
        {
            GetComponent<SpriteRenderer>().material = fertilizedMaterial_Wet;
        }
        else
        if (!isWatered)
        {
            GetComponent<SpriteRenderer>().material = wateredMaterial;
        }
        isWatered = true;
        isWateredToday = true;
    }

    private void DryForDayStart(int newday)
    {
        Dry();
    }
    private void Dry()
    {
        if(isFertilized)
        {
            GetComponent<SpriteRenderer>().material = fertilizedMaterial_Dry;
        }
        else
            GetComponent<SpriteRenderer>().material = defaultMaterial;
        isWatered = false;
        isWateredToday = false;
    }
    public void DayChangeHappend(int newDay)
    {
        if (isWatered && gameObject.GetComponentInChildren<CropBehaviour>()!=null)
            gameObject.GetComponentInChildren<CropBehaviour>().Grow(newDay);
    }

    public void GetFertilized()
    {
        if(WeatherManager.Instance.isRaining||isWatered)
            GetComponent<SpriteRenderer>().material = fertilizedMaterial_Wet;
        else
            GetComponent<SpriteRenderer>().material = fertilizedMaterial_Dry;
        isFertilized = true;
    }

    public void RemoveFertilizer()
    {
        isFertilized = false;
        if (isWatered)
            GetComponent<SpriteRenderer>().material = wateredMaterial;
        else
            GetComponent<SpriteRenderer>().material = defaultMaterial;
    }
}
