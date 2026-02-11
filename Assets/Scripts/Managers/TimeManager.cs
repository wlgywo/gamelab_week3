using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    public const int HOURS_PER_DAY = 24;
    public const float SECONDS_PER_HOUR = 12f; // 1시간

    //매 시간 변경될 때 호출되는 이벤 (인자: 새로운 시간)
    public event Action<int> OnHourChanged;
    // 날짜가 변경될 때 호출되는 이벤 (인자: 새로운 날짜)
    public event Action<int> OnDayEnd;
    public event Action<int> OnDayStart;
    public event Action OnFadeStart;

    private float _timeSinceLastHour = 0f;
    private bool _isTransitioningDay = false;
    public bool canProceedToNextDay = false;

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

    void Start()
    {

    }

    void Update()
    {
        /*_timeSinceLastHour += Time.deltaTime;

        if (_timeSinceLastHour >= SECONDS_PER_HOUR)
        {
            _timeSinceLastHour -= SECONDS_PER_HOUR;
            CurrentHour++;
            OnHourChanged?.Invoke(CurrentHour);
            if (CurrentHour >= HOURS_PER_DAY)
            {
                CurrentHour = 0; 
                CurrentDay++;
                GiantCropManager.Instance.CheckForGiantCrops();
                OnDayEnd?.Invoke(CurrentDay);
                OnDayStart?.Invoke(CurrentDay);
            }
        }*/
        _timeSinceLastHour += Time.deltaTime;
        if (_timeSinceLastHour >= SECONDS_PER_HOUR)
        {
            // ... (기존 시간 증가 로직) ...
            _timeSinceLastHour -= SECONDS_PER_HOUR;
            CurrentHour++;
            OnHourChanged?.Invoke(CurrentHour);
            if (CurrentHour >= HOURS_PER_DAY)
            {
                // 하루가 끝나는 조건이 충족되면 전환 코루틴을 시작
                StartCoroutine(DayTransitionSequence());
            }
        }
    }

    public string GetFormattedTime()
    {
        return $"Day {CurrentDay}, {CurrentHour:00}:00";
    }

    public IEnumerator DayTransitionSequence()
    {
        if(_isTransitioningDay) yield break;
        _isTransitioningDay = true;

        InputManager.Instance.playerInput.Player.Disable();
        InputManager.Instance.playerInput.UI.Enable();

        yield return StartCoroutine(UIManager.Instance.FadeOut());

        // 시간이 멈추고 정산 UI 표시
        Time.timeScale = 0f;
        UIManager.Instance.mainCanvas.SetActive(false);
        UIManager.Instance.oneRoomUI.SetActive(false);
        UIManager.Instance.sellUI.SetActive(false);
        UIManager.Instance.storeUI.SetActive(false);
        yield return StartCoroutine(FairyManager.Instance.CheckAndRunFairyEvent(CurrentDay));

        UIManager.Instance.UpdateTodayEarningText();
        UIManager.Instance.continueText.gameObject.SetActive(true);
        UIManager.Instance.todayEarningText.gameObject.SetActive(true);
        InputManager.Instance.isPlayerInputLocked = true; // 입력 잠금
        
        // 하루 끝 이벤트 호출
        CurrentHour = 0;
        CurrentDay++;
        
        OnDayEnd?.Invoke(CurrentDay);

        // 플레이어의 다음 날 진행 입력을 대기
        TimeManager.Instance.canProceedToNextDay = false;
        yield return new WaitUntil(() => TimeManager.Instance.canProceedToNextDay);

        // 정산 UI 숨김
        UIManager.Instance.mainCanvas.SetActive(true);
        UIManager.Instance.continueText.gameObject.SetActive(false);
        UIManager.Instance.todayEarningText.gameObject.SetActive(false);
        

        // UIManager에게 화면을 밝게 하라고 요청하고 끝날 때까지 대기
        yield return StartCoroutine(UIManager.Instance.FadeIn());

        // 하루 시작 이벤트 호출 및 시간 흐름 재개
        GiantCropManager.Instance.CheckForGiantCrops();
        OnDayStart?.Invoke(CurrentDay); // ★ 원하는 타이밍에 호출
        Time.timeScale = 1f;
        InputManager.Instance.playerInput.Player.Enable();
        InputManager.Instance.playerInput.UI.Disable();

        InputManager.Instance.isPlayerInputLocked = false;
        _isTransitioningDay = false; // 전환 완료
    }
}
