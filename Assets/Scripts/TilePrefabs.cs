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
    public bool isRaining = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        TimeManager.Instance.OnDayEnd += DayChangeHappend;
        WeatherManager.Instance.OnRainStarted += GetWet;
        WeatherManager.Instance.OnRainStopped += Dry;
    }

    private void OnDisable()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnDayEnd -= DayChangeHappend;
            WeatherManager.Instance.OnRainStarted -= GetWet;
            WeatherManager.Instance.OnRainStopped -= Dry;
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
    }

    public void GetWetByPlayer()
    {
        if (!isWatered)
        {
            GetComponent<SpriteRenderer>().material = wateredMaterial;
            isWatered = true;
        }
    }
    private void Dry()
    {
        GetComponent<SpriteRenderer>().material = defaultMaterial;
        isWatered = false;
    }
    public void DayChangeHappend(int newDay)
    {
        if (isWatered && gameObject.GetComponentInChildren<CropBehaviour>()!=null)
            gameObject.GetComponentInChildren<CropBehaviour>().Grow(newDay);
        Dry();
    }
}
