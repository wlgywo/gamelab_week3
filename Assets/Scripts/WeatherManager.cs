using System;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance { get; private set; }
    [Range(0f, 1f)]
    public float probability = 0.3f;
    public ParticleSystem rainEffect;
    public string spawnPointTag = "Tile";

    public event Action OnRainStarted;
    public event Action OnRainStopped;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        TimeManager.Instance.OnDayStart += SetRain;
    }

    private void OnDisable()
    {
        TimeManager.Instance.OnDayStart -= SetRain;
    }
    void Update()
    {
    }
    public void SetRain(int newday)
    {
        float randomValue = UnityEngine.Random.Range(0f, 1f);

        if (randomValue < probability)
        {
            rainEffect.Play();
            OnRainStarted?.Invoke();
        }
        else
        {
            rainEffect.Stop();
            OnRainStopped?.Invoke();
        }
    }
}
