using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public List<TextMeshProUGUI> modeTexts;

    public TMP_Text timeText;
    public TMP_Text harvetText;
    public TMP_Text moneyText;
    public TMP_Text waterText;
    public TMP_Text weatherProbabilityText;
    public TMP_Text scarecrowText;

    public GameObject mainCanvas;
    private Coroutine _updateTextCoroutine;

    [Header("인벤토리 텍스트")]
    public TMP_Text inventoryText_Fertilizer;
    public TMP_Text inventoryText_Parsnip;
    public TMP_Text inventoryText_Carrot;
    public TMP_Text inventoryText_Radish;
    public TMP_Text inventoryText_Potato;  
    public TMP_Text inventoryText_Eggplant;
    public TMP_Text inventoryText_Pumpkin;
    public TMP_Text inventoryText_Scarecrow;


    public GameObject storeNoticeText;
    public GameObject sellNoticeText;
    public GameObject oneRoomNoticeText;
    public GameObject storeUI;
    public GameObject sellUI;
    public GameObject oneRoomUI;

    [Header("페이드 효과")]
    public Image fadeImage;
    public float fadeDuration = 1f;
    public bool canProceedToNextDay = false;

    public TMP_Text continueText;
    public TMP_Text todayEarningText;
    public int earning = 0;

    [Header("텍스트 색상")]
    public Color defaultColor = Color.white;
    public Color selectedColor = Color.yellow;

    [Header("허수아비 메시지")]
    public GameObject askHarvestUI;
    private Action onYesAction;
    private Action onNoAction;
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
        //TimeManager.Instance.OnDayStart += StartFadeIn;
        //TimeManager.Instance.OnDayEnd += StartFadeOut;
        // TimeManager.Instance.OnFadeStart += () => StartCoroutine(DayTransitionCoroutine());

        ModeManager.Instance.ChangeModeText += UpdateModeText;

        UpdateTimeDisplay(TimeManager.Instance.CurrentHour);
        UpdateInventoryText();
        UpdateWaterText(100f);
        UpdateWeatherProbabilityText();
    }

    void OnDisable()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnHourChanged -= UpdateTimeDisplay;
            TimeManager.Instance.OnDayStart -= UpdateTimeDisplay;
            //TimeManager.Instance.OnDayStart -= StartFadeIn;
            //TimeManager.Instance.OnDayEnd -= StartFadeOut;
            // TimeManager.Instance.OnFadeStart -= () => StartCoroutine(DayTransitionCoroutine());

            ModeManager.Instance.ChangeModeText -= UpdateModeText;
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

    public void UpdateModeText(int mode)
    {
        int selectedIndex = (mode == 0) ? 9 : mode - 1;

        // 2. 모든 텍스트를 순회하며 색상을 설정합니다.
        for (int i = 0; i < modeTexts.Count; i++)
        {
            // 3. 현재 인덱스가 위에서 결정된 selectedIndex와 같은지 확인합니다.
            if (i == selectedIndex)
            {
                modeTexts[i].color = selectedColor;
            }
            else
            {
                modeTexts[i].color = defaultColor;
            }
        }
    }

    public void ToggleStoreUI()
    {
        storeUI.SetActive(!storeUI.activeSelf);
        InputManager.Instance.isPlayerInputLocked = storeUI.activeSelf;
    }

    public void ToggleSellUI()
    {
        sellUI.SetActive(!sellUI.activeSelf);
        InputManager.Instance.isPlayerInputLocked = sellUI.activeSelf;
    }

    public void ToggleOneRoomUI()
    {
        oneRoomUI.SetActive(!oneRoomUI.activeSelf);
        InputManager.Instance.isPlayerInputLocked = oneRoomUI.activeSelf;
    }
    
    public void ShowSellNoticeText()
    {
        if(sellNoticeText == null) return;
        sellNoticeText.SetActive(true);
    }

    public void HideSellNoticeText()
    {
        if (sellNoticeText == null) return;
        sellNoticeText.SetActive(false);
    }

    public void ShowStoreNoticeText()
    {
        if(storeNoticeText == null) return;
        storeNoticeText.GetComponent<TextMeshProUGUI>().text = $"- 상점 -\n >> 들어가기 [E] <<";
        storeNoticeText.SetActive(true);
    }

    public void HideStoreNoticeText()
    {
        if (storeNoticeText == null) return;
        storeNoticeText.SetActive(false);
    }

    public void ShowOneRoomNoticeText()
    {
        if (oneRoomNoticeText == null) return;
        oneRoomNoticeText.SetActive(true);
    }

    public void HideOneRoomNoticeText()
    {
        if (oneRoomNoticeText == null) return;
        oneRoomNoticeText.SetActive(false);
    }

    public void UpdateMoneyText()
    {
        moneyText.text = $"지갑: {GameManager.Instance.playerMoney}원";
    }

    public void UpdateHarvestText()
    {
        /*
        harvetText.text = $"파스닙: {CropManager.Instance.inventory[]} 개\n" +
                        $"당근: {CropManager.Instance.harvestAmountCarrot} 개\n";
        // 여기다가 다른 작물 추가
        */


        StringBuilder sb = new StringBuilder();

        foreach (KeyValuePair<string, int> item in CropManager.Instance.inventory)
        {
            sb.AppendLine($"{item.Key}: {item.Value} 개");
        }

        harvetText.text = sb.ToString();
    }

    public void UpdateInventoryText()
    {
        inventoryText_Fertilizer.text = $"4. 비료 X {GameManager.Instance.fertilizerCount}";
        inventoryText_Parsnip.text = $"5. 파스닙 씨앗 X {CropManager.Instance.seedParsnip}";  
        inventoryText_Carrot.text = $"6. 당근 씨앗 X {CropManager.Instance.seedCarrot}";
        inventoryText_Radish.text = $"7. 무 씨앗 X {CropManager.Instance.seedRadish}";
        inventoryText_Potato.text = $"8. 감자 씨앗 X {CropManager.Instance.seedPotato}";
        inventoryText_Eggplant.text = $"9. 가지 씨앗 X {CropManager.Instance.seedEggplant}";
        inventoryText_Pumpkin.text = $"10. 호박 씨앗 X {CropManager.Instance.seedPumpkin}";
        inventoryText_Scarecrow.text = $"F1. 허수아비 X {GameManager.Instance.scarecrowCount}";
    }

    public void UpdateWaterText(float waterAmount)
    {
        waterText.text = $"2. 물뿌리개 (현재 물 양: {waterAmount})";
    }

    public void UpdateWeatherProbabilityText()
    {
        float probabilityPercent = WeatherManager.Instance.probability * 100f;
        weatherProbabilityText.text = $"강수 확률: {probabilityPercent.ToString("F0")}%";
    }

    public void UpdateTodayEarningText()
    {
        todayEarningText.text = $"{TimeManager.Instance.CurrentDay} 일차\n오늘의 수익\n{earning} 원\n총 수익\n{GameManager.Instance.allEarnings} 원";
    }

    public void ShowScarecrowMessage(string text)
    {
        // 2. 이전에 실행된 코루틴이 아직 실행 중인지 확인
        if (_updateTextCoroutine != null)
        {
            // 3. 만약 실행 중이라면 즉시 중지
            StopCoroutine(_updateTextCoroutine);
        }

        // 4. 새로운 코루틴을 시작하고, 그 참조를 변수에 저장
        _updateTextCoroutine = StartCoroutine(UpdateScarecrowText(text));

    }
    public IEnumerator UpdateScarecrowText(string text)
    {
        scarecrowText.text = text;
        yield return new WaitForSeconds(2f);
        scarecrowText.text = "";
    }

    public IEnumerator FadeOut()
    {
        float t = 0;
        Color color = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            color.a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        float t = fadeDuration;
        Color color = fadeImage.color;
        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            color.a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }
    public void ShowConfirmationPopup(Action onYes, Action onNo)
    {
        this.onYesAction = onYes;
        this.onNoAction = onNo;
        askHarvestUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnYesClicked()
    {
        askHarvestUI.SetActive(false);
        Time.timeScale = 1f;
        onYesAction?.Invoke();
    }

    public void OnNoClicked()
    {
        askHarvestUI.SetActive(false);
        Time.timeScale = 1f;
        onNoAction?.Invoke();
    }
}
