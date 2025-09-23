using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
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
    public GameObject scarecrowText;

    public GameObject mainCanvas;
    private Coroutine _updateTextCoroutine;
    // public GameObject Player;

    [Header("�κ��丮 �ؽ�Ʈ")]
    public TMP_Text inventoryText_Fertilizer;
    public TMP_Text inventoryText_Parsnip;
    public TMP_Text inventoryText_Carrot;
    public TMP_Text inventoryText_Radish;
    public TMP_Text inventoryText_Potato;  
    public TMP_Text inventoryText_Eggplant;
    public TMP_Text inventoryText_Pumpkin;
    public TMP_Text inventoryText_Scarecrow;
    public TMP_Text inventoryText_Manure;
    public TMP_Text inventoryText_Pesticide;
    public TMP_Text inventoryText_Nutrition;

    public GameObject storeNoticeText;
    public GameObject sellNoticeText;
    public GameObject oneRoomNoticeText;
    public GameObject likeNoticeTextUI;
    public GameObject waterCanFillTextUI;
    public TMP_Text talkText;
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

    [Header("����ƺ� �޽���")]
    public GameObject askHarvestUI;
    private Action onYesAction;
    private Action onNoAction;

    [Header("ȣ����")]
    public TMP_Text likeabilityText;
    private Queue<string> noticeQueue = new Queue<string>();
    private Coroutine noticeCoroutine = null;
    public GameObject getMarryUI;
    private Action onMarryYesAction;
    private Action onMarryNoAction;
    public TMP_Text marryText;
    public Image cropImage;

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

        for (int i = 0; i < modeTexts.Count; i++)
        {
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
        
        if(storeUI.activeSelf)
        {
            FixStoreUpdateText("- ���� -\r\n>> ������ [E] <<");
        }
        else 
        {
            FixStoreUpdateText("- ���� -\r\n>> ���� [E] <<");
        }

        storeUI.SetActive(!storeUI.activeSelf);
    }

    public void ToggleSellUI()
    {
        if(sellUI.activeSelf)
        {
            FixSellUpdateText("- ���� -\r\n>> ������ [E] <<");
        }
        else 
        {
            FixSellUpdateText("- ���� -\r\n>> ���� [E] <<");
        }

        sellUI.SetActive(!sellUI.activeSelf);
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

    public void FixSellUpdateText(string text)
    {
        sellNoticeText.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    public void ShowStoreNoticeText()
    {
        if(storeNoticeText == null) return;
        storeNoticeText.SetActive(true);
    }

    public void HideStoreNoticeText()
    {
        if (storeNoticeText == null) return;
        storeNoticeText.SetActive(false);
    }
    public void FixStoreUpdateText(string text)
    {
        storeNoticeText.GetComponentInChildren<TextMeshProUGUI>().text = text;
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

    public void ShowWaterCanFillTextUI()
    {
        if (waterCanFillTextUI == null) return;
        waterCanFillTextUI.SetActive(true);
    }

    public void HideWaterCanFillTextUI()
    {
        if (waterCanFillTextUI == null) return;
        waterCanFillTextUI.SetActive(false);
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
        inventoryText_Fertilizer.text = $"��� X {GameManager.Instance.fertilizerCount}";
        inventoryText_Parsnip.text = $"�Ľ��� ���� X {CropManager.Instance.seedParsnip}";  
        inventoryText_Carrot.text = $"��� ���� X {CropManager.Instance.seedCarrot}";
        inventoryText_Radish.text = $"�� ���� X {CropManager.Instance.seedRadish}";
        inventoryText_Potato.text = $"���� ���� X {CropManager.Instance.seedPotato}";
        inventoryText_Eggplant.text = $"���� ���� X {CropManager.Instance.seedEggplant}";
        inventoryText_Pumpkin.text = $"ȣ�� ���� X {CropManager.Instance.seedPumpkin}";
        inventoryText_Scarecrow.text = $"����ƺ� X {GameManager.Instance.scarecrowCount}";
        inventoryText_Nutrition.text = $"������ X {GameManager.Instance.nutritionCount}";
        inventoryText_Manure.text = $"�Ÿ� X {GameManager.Instance.manureCount}";
        inventoryText_Pesticide.text = $"��� X {GameManager.Instance.pesticideCount}";
    }

    public void UpdateWaterText(float waterAmount)
    {
        waterText.text = $"���Ѹ��� (���� �� ��: {waterAmount})";
    }

    public void UpdateWeatherProbabilityText()
    {
        float probabilityPercent = WeatherManager.Instance.probability * 100f;
        weatherProbabilityText.text = $"���� Ȯ��: {probabilityPercent.ToString("F0")}%";
    }

    public void UpdateTodayEarningText()
    {
        todayEarningText.text = $"{TimeManager.Instance.CurrentDay} ����\n������ ����\n{earning} ��\n�� ����\n{GameManager.Instance.allEarnings} ��";
        earning = 0;
    }

    public void ShowScarecrowMessage(string text)
    {
        if (_updateTextCoroutine != null)
        {
            scarecrowText.SetActive(false);
            StopCoroutine(_updateTextCoroutine);
        }

        _updateTextCoroutine = StartCoroutine(UpdateScarecrowText(text));
    }
    public IEnumerator UpdateScarecrowText(string text)
    {
        scarecrowText.SetActive(true);
        scarecrowText.GetComponentInChildren<TextMeshProUGUI>().text = text;
        yield return new WaitForSeconds(2f);
        scarecrowText.GetComponentInChildren<TextMeshProUGUI>().text = "";
        scarecrowText.SetActive(false);
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
    public void ShowConfirmationPopup(Action onYes, Action onNo, int likeability)
    {
        this.onYesAction = onYes;
        this.onNoAction = onNo;
        askHarvestUI.SetActive(true);
        likeabilityText.text = $"�ش� �۹��� ȣ����: {likeability}";
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

    public void ShowConfirmationMarry(Action onYes, Action onNo, int likeability, string cropName)
    {
        this.onMarryYesAction = onYes;
        this.onMarryNoAction = onNo;
        getMarryUI.SetActive(true);
        marryText.text = $"���� {cropName}�� ȣ������ 50 �̻� �Դϴ�\n{cropName}�� ��ȥ�Ͻðڽ��ϱ�?";
        Time.timeScale = 0f;
    }

    public void OnMarryYesClicked()
    {
        getMarryUI.SetActive(false);
        Time.timeScale = 1f;
        onMarryYesAction?.Invoke();
    }

    public void OnMarryNoClicked()
    {
        getMarryUI.SetActive(false);
        Time.timeScale = 1f;
        onMarryNoAction?.Invoke();
    }
    public void NoticeLikeability(string cropName, int cropLikeability)
    {

        string message = $"{cropName}�� ȣ������ ���� ȣ����: {cropLikeability}";
        EnqueueMessage(message);
    }


    public void NoticeSimpleLikeability(string cropName, string text)
    {
        EnqueueMessage(text);
    }

    private void EnqueueMessage(string message)
    {
        noticeQueue.Enqueue(message);

        if (noticeCoroutine == null)
        {
            noticeCoroutine = StartCoroutine(ProcessNoticeQueue());

        }
    }

    private IEnumerator ProcessNoticeQueue()
    {
        while (noticeQueue.Count > 0)
        {
            string currentMessage = noticeQueue.Dequeue();

            likeNoticeTextUI.GetComponentInChildren<TextMeshProUGUI>().text = currentMessage;
            likeNoticeTextUI.SetActive(true);

            yield return new WaitForSeconds(1.5f);
        }
        likeNoticeTextUI.SetActive(false);
        noticeCoroutine = null;
    }

    /*public void UpdateTalkText(string cropName, int cropLikeability)
    {
        if (likeFadeCoroutine != null)
        {
            StopCoroutine(likeFadeCoroutine);
        } 
        likeFadeCoroutine = StartCoroutine(TalkFadeRoutine(cropName, cropLikeability));
    }

    private IEnumerator TalkFadeRoutine(string cropName, int cropLikeability)
    {
        likeNoticeText.text = $"{cropName}�� ȣ������ ����߽��ϴ�! ( ȣ����: {cropLikeability} )";
        likeNoticeText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        likeNoticeText.gameObject.SetActive(false);
    }*/
}
