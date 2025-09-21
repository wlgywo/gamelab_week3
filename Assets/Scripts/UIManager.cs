using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public List<TextMeshProUGUI> modeTexts;

    public TMP_Text timeText;
    public TMP_Text harvetText;
    public TMP_Text moneyText;
    public TMP_Text waterText;
    public TMP_Text weatherProbabilityText;

    public GameObject mainCanvas;

    [Header("�κ��丮 �ؽ�Ʈ")]
    public TMP_Text inventoryText_Fertilizer;
    public TMP_Text inventoryText_Parsnip;
    public TMP_Text inventoryText_Carrot;
    public TMP_Text inventoryText_Radish;
    public TMP_Text inventoryText_Potato;  
    public TMP_Text inventoryText_Eggplant;
    public TMP_Text inventoryText_Pumpkin;
    

    public GameObject storeNoticeText;
    public GameObject sellNoticeText;
    public GameObject oneRoomNoticeText;
    public GameObject storeUI;
    public GameObject sellUI;
    public GameObject oneRoomUI;

    [Header("���̵� ȿ��")]
    public Image fadeImage;
    public float fadeDuration = 1f;
    public bool canProceedToNextDay = false;

    public TMP_Text continueText;
    public TMP_Text todayEarningText;
    public int earning = 0;

    [Header("�ؽ�Ʈ ����")]
    public Color defaultColor = Color.white;
    public Color selectedColor = Color.yellow;
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

        // 2. ��� �ؽ�Ʈ�� ��ȸ�ϸ� ������ �����մϴ�.
        for (int i = 0; i < modeTexts.Count; i++)
        {
            // 3. ���� �ε����� ������ ������ selectedIndex�� ������ Ȯ���մϴ�.
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
        sellNoticeText.SetActive(true);
    }

    public void HideSellNoticeText()
    {
        sellNoticeText.SetActive(false);
    }

    public void ShowStoreNoticeText()
    {
        storeNoticeText.GetComponent<TextMeshProUGUI>().text = $"- ���� -\n >> ���� [E] <<";
        storeNoticeText.SetActive(true);
    }

    public void HideStoreNoticeText()
    {
        storeNoticeText.SetActive(false);
    }

    public void ShowOneRoomNoticeText()
    {
        oneRoomNoticeText.SetActive(true);
    }

    public void HideOneRoomNoticeText()
    {
        oneRoomNoticeText.SetActive(false);
    }

    public void UpdateMoneyText()
    {
        moneyText.text = $"����: {GameManager.Instance.playerMoney}��";
    }

    public void UpdateHarvestText()
    {
        /*
        harvetText.text = $"�Ľ���: {CropManager.Instance.inventory[]} ��\n" +
                        $"���: {CropManager.Instance.harvestAmountCarrot} ��\n";
        // ����ٰ� �ٸ� �۹� �߰�
        */


        StringBuilder sb = new StringBuilder();

        foreach (KeyValuePair<string, int> item in CropManager.Instance.inventory)
        {
            sb.AppendLine($"{item.Key}: {item.Value} ��");
        }

        harvetText.text = sb.ToString();
    }

    public void UpdateInventoryText()
    {
        inventoryText_Fertilizer.text = $"4. ��� X {GameManager.Instance.fertilizerCount}";
        inventoryText_Parsnip.text = $"5. �Ľ��� ���� X {CropManager.Instance.seedParsnip}";  
        inventoryText_Carrot.text = $"6. ��� ���� X {CropManager.Instance.seedCarrot}";
        inventoryText_Radish.text = $"7. �� ���� X {CropManager.Instance.seedRadish}";
        inventoryText_Potato.text = $"8. ���� ���� X {CropManager.Instance.seedPotato}";
        inventoryText_Eggplant.text = $"9. ���� ���� X {CropManager.Instance.seedEggplant}";
        inventoryText_Pumpkin.text = $"10. ȣ�� ���� X {CropManager.Instance.seedPumpkin}";
    }

    public void UpdateWaterText(float waterAmount)
    {
        waterText.text = $"2. ���Ѹ��� (���� �� ��: {waterAmount})";
    }

    public void UpdateWeatherProbabilityText()
    {
        float probabilityPercent = WeatherManager.Instance.probability * 100f;
        weatherProbabilityText.text = $"���� Ȯ��: {probabilityPercent.ToString("F0")}%";
    }

    public void UpdateTodayEarningText()
    {
        todayEarningText.text = $"{TimeManager.Instance.CurrentDay-1} ����\n������ ����\n{earning} ��\n�� ����\n{GameManager.Instance.allEarnings} ��";
    }

    /*
    public void StartFadeIn(int newday)
    {
        StartCoroutine(FadeIn());
    }

    public void StartFadeOut(int newday)
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        Color color = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = t / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float t = fadeDuration;
        Color color = fadeImage.color;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            color.a = t / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }
    }
    */
    /*IEnumerator DayTransitionCoroutine()
    {
        InputManager.Instance.isPlayerInputLocked = true;
        float t = 0;
        Color color = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            color.a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        UpdateTodayEarningText();
        mainCanvas.SetActive(false);
        Time.timeScale = 0f;
        continueText.gameObject.SetActive(true);
        todayEarningText.gameObject.SetActive(true);

        canProceedToNextDay = false;
        yield return null;
        yield return new WaitUntil(() => canProceedToNextDay);


        mainCanvas.SetActive(true);
        Time.timeScale = 1f;
        continueText.gameObject.SetActive(false);
        todayEarningText.gameObject.SetActive(false);


        t = fadeDuration;
        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            color.a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        InputManager.Instance.isPlayerInputLocked = false;
    }*/

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
}
