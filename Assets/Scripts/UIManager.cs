using System.Collections.Generic;
using System.Text;
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
    public TMP_Text inventoryText_Fertilizer;
    public TMP_Text inventoryText_Parsnip;
    public TMP_Text inventoryText_Carrot;
    public TMP_Text inventoryText_Radish;
    public TMP_Text inventoryText_Potato;  
    public TMP_Text inventoryText_Eggplant;
    public TMP_Text inventoryText_Pumpkin;


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
        UpdateWaterText(100f);
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
    }

    public void UpdateWaterText(float waterAmount)
    {
        waterText.text = $"2. 물뿌리개 (현재 물 양: {waterAmount})";
    }
}
