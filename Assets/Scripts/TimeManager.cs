using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    public const int HOURS_PER_DAY = 10;
    public const float SECONDS_PER_HOUR = 1f; // 1�ð�

    //�� �ð� ����� �� ȣ��Ǵ� �̺� (����: ���ο� �ð�)
    public event Action<int> OnHourChanged;
    // ��¥�� ����� �� ȣ��Ǵ� �̺� (����: ���ο� ��¥)
    public event Action<int> OnDayEnd;
    public event Action<int> OnDayStart;

    private float _timeSinceLastHour = 0f;

    public int CurrentHour { get; private set; } = 6; 
    public int CurrentDay { get; private set; } = 1;

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

    void Update()
    {
        _timeSinceLastHour += Time.deltaTime;

        if (_timeSinceLastHour >= SECONDS_PER_HOUR)
        {
            _timeSinceLastHour -= SECONDS_PER_HOUR;
            CurrentHour++;
            OnHourChanged?.Invoke(CurrentHour);
            if (CurrentHour >= HOURS_PER_DAY)
            {
                CurrentHour = 0; 
                CurrentDay++;
                OnDayEnd?.Invoke(CurrentDay);
                OnDayStart?.Invoke(CurrentDay);
            }
        }
    }

    public string GetFormattedTime()
    {
        return $"Day {CurrentDay}, {CurrentHour:00}:00";
    }
}
