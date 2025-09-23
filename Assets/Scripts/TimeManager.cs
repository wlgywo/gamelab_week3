using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    public const int HOURS_PER_DAY = 24;
    public const float SECONDS_PER_HOUR = 12f; // 1�ð�

    //�� �ð� ����� �� ȣ��Ǵ� �̺� (����: ���ο� �ð�)
    public event Action<int> OnHourChanged;
    // ��¥�� ����� �� ȣ��Ǵ� �̺� (����: ���ο� ��¥)
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
            // ... (���� �ð� ���� ����) ...
            _timeSinceLastHour -= SECONDS_PER_HOUR;
            CurrentHour++;
            OnHourChanged?.Invoke(CurrentHour);
            if (CurrentHour >= HOURS_PER_DAY)
            {
                // �Ϸ簡 ������ ������ �����Ǹ� ��ȯ �ڷ�ƾ�� ����
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

        // �ð��� ���߰� ���� UI ǥ��
        Time.timeScale = 0f;
        UIManager.Instance.mainCanvas.SetActive(false);
        UIManager.Instance.oneRoomUI.SetActive(false);
        UIManager.Instance.sellUI.SetActive(false);
        UIManager.Instance.storeUI.SetActive(false);
        yield return StartCoroutine(FairyManager.Instance.CheckAndRunFairyEvent(CurrentDay));

        UIManager.Instance.UpdateTodayEarningText();
        UIManager.Instance.continueText.gameObject.SetActive(true);
        UIManager.Instance.todayEarningText.gameObject.SetActive(true);
        InputManager.Instance.isPlayerInputLocked = true; // �Է� ���
        
        // �Ϸ� �� �̺�Ʈ ȣ��
        CurrentHour = 0;
        CurrentDay++;
        
        OnDayEnd?.Invoke(CurrentDay);

        // �÷��̾��� ���� �� ���� �Է��� ���
        TimeManager.Instance.canProceedToNextDay = false;
        yield return new WaitUntil(() => TimeManager.Instance.canProceedToNextDay);

        // ���� UI ����
        UIManager.Instance.mainCanvas.SetActive(true);
        UIManager.Instance.continueText.gameObject.SetActive(false);
        UIManager.Instance.todayEarningText.gameObject.SetActive(false);
        

        // UIManager���� ȭ���� ��� �϶�� ��û�ϰ� ���� ������ ���
        yield return StartCoroutine(UIManager.Instance.FadeIn());

        // �Ϸ� ���� �̺�Ʈ ȣ�� �� �ð� �帧 �簳
        GiantCropManager.Instance.CheckForGiantCrops();
        OnDayStart?.Invoke(CurrentDay); // �� ���ϴ� Ÿ�ֿ̹� ȣ��
        Time.timeScale = 1f;
        InputManager.Instance.playerInput.Player.Enable();
        InputManager.Instance.playerInput.UI.Disable();

        InputManager.Instance.isPlayerInputLocked = false;
        _isTransitioningDay = false; // ��ȯ �Ϸ�
    }
}
