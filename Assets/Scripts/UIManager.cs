using System.Collections.Generic;
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
    public TMP_Text inventoryText;


    public GameObject storeNoticeText;
    public GameObject sellNoticeText;
    public GameObject storeUI;
    public GameObject sellUI;

    [Header("텍스트 색상")]
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
        ModeManager.Instance.ChangeModeText += UpdateModeText;
        UpdateTimeDisplay(TimeManager.Instance.CurrentHour);
        UpdateInventoryText();
    }

    void OnDisable()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnHourChanged -= UpdateTimeDisplay;
            TimeManager.Instance.OnDayStart -= UpdateTimeDisplay;
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
        for (int i = 0; i < modeTexts.Count; i++)
        {
            if (i == mode-1)
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
    }

    public void ToggleSellUI()
    {
        sellUI.SetActive(!sellUI.activeSelf);
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
        storeNoticeText.SetActive(true);
    }

    public void HideStoreNoticeText()
    {
        storeNoticeText.SetActive(false);
    }

    public void UpdateMoneyText()
    {
        moneyText.text = $"지갑: {GameManager.Instance.playerMoney}원";
    }

    public void UpdateHarvestText()
    {
        harvetText.text = $"파스닙: {CropManager.Instance.harvestAmountParsnip} 개\n" +
                        $"무: 0 개\n";
        // 여기다가 다른 작물 추가
    }

    public void UpdateInventoryText()
    {
        inventoryText.text = $"4. 파스닙 씨앗 X {CropManager.Instance.seedParsnip}";
    }
}
