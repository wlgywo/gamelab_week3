using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public TMP_Text timeText;
    public TMP_Text harvetText;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TimeManager.Instance.OnHourChanged += UpdateTimeDisplay;
        TimeManager.Instance.OnDayStart += UpdateTimeDisplay;
    }

    void OnDisable()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnHourChanged -= UpdateTimeDisplay;
            TimeManager.Instance.OnDayStart -= UpdateTimeDisplay;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTimeDisplay(int newHour)
    {
        timeText.text = TimeManager.Instance.GetFormattedTime();
    }
}
