using TMPro;
using UnityEngine;

public class StoreScript : MonoBehaviour
{

    [SerializeField] private bool isPlayerInRange = false;

    [Header("상점 아이템 가역")]
    [SerializeField] private int fertilizerPrice = 50;
    [SerializeField] private int parsnipSeedPrice = 20;
    [SerializeField] private int carrotSeedPrice = 30;
    [SerializeField] private int radishSeedPrice = 40;
    [SerializeField] private int potatoSeedPrice = 40;
    [SerializeField] private int eggplantSeedPrice = 40;
    [SerializeField] private int pumpkinSeedPrice = 40;
    [SerializeField] private int prayRainPrice = 100;
    [SerializeField] private int scarecrowPrice = 200;
    void Start()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.DoPlant += TryOpenStore;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.DoPlant -= TryOpenStore;
        }
    }

    private void TryOpenStore()
    {
        if (isPlayerInRange)
        {
            UIManager.Instance.ToggleStoreUI();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.ShowStoreNoticeText();
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.HideStoreNoticeText();
            if (UIManager.Instance.storeUI.activeSelf)
            {
                UIManager.Instance.storeUI.SetActive(false);
            }
            isPlayerInRange = false;
        }
    }

    public void BuyParsnipSeed()
    {
        int currentPlayerMoney = GameManager.Instance.playerMoney;

        if (currentPlayerMoney >= parsnipSeedPrice)
        {
            GameManager.Instance.playerMoney -= parsnipSeedPrice;
            CropManager.Instance.seedParsnip++;

            UIManager.Instance.UpdateMoneyText();
            UIManager.Instance.UpdateInventoryText();
        }
        else
        {
            // "돈 부족" 팝업을 여기에 추가
        }
    }

    public void BuyFertilizer()
    {
        int currentPlayerMoney = GameManager.Instance.playerMoney;
        if (currentPlayerMoney >= fertilizerPrice)
        {
            GameManager.Instance.playerMoney -= fertilizerPrice;
            GameManager.Instance.fertilizerCount++;

            UIManager.Instance.UpdateMoneyText();
            UIManager.Instance.UpdateInventoryText();
        }
        else
        {
            // "돈 부족" 팝업을 여기에 추가
        }
    }

    public void BuyCarrotSeed()
    {
        int currentPlayerMoney = GameManager.Instance.playerMoney;
        if (currentPlayerMoney >= carrotSeedPrice)
        {
            GameManager.Instance.playerMoney -= carrotSeedPrice;
            CropManager.Instance.seedCarrot++;

            UIManager.Instance.UpdateMoneyText();
            UIManager.Instance.UpdateInventoryText();
        }
        else
        {
            // "돈 부족" 팝업을 여기에 추가
        }
    }

    public void BuyRadishSeed()
    {
        int currentPlayerMoney = GameManager.Instance.playerMoney;
        if (currentPlayerMoney >= radishSeedPrice)
        {
            GameManager.Instance.playerMoney -= radishSeedPrice;
            CropManager.Instance.seedRadish++;

            UIManager.Instance.UpdateMoneyText();
            UIManager.Instance.UpdateInventoryText();
        }
        else
        {
            // "돈 부족" 팝업을 여기에 추가
        }
    }

    public void BuyPotatoSeed()
    {
        int currentPlayerMoney = GameManager.Instance.playerMoney;
        if (currentPlayerMoney >= potatoSeedPrice)
        {
            GameManager.Instance.playerMoney -= potatoSeedPrice;
            CropManager.Instance.seedPotato++;

            UIManager.Instance.UpdateMoneyText();
            UIManager.Instance.UpdateInventoryText();
        }
        else
        {
            // "돈 부족" 팝업을 여기에 추가
        }
    }

    public void BuyEggplantSeed()
    {
        int currentPlayerMoney = GameManager.Instance.playerMoney;
        if (currentPlayerMoney >= eggplantSeedPrice)
        {
            GameManager.Instance.playerMoney -= eggplantSeedPrice;
            CropManager.Instance.seedEggplant++;

            UIManager.Instance.UpdateMoneyText();
            UIManager.Instance.UpdateInventoryText();
        }
        else
        {
            // "돈 부족" 팝업을 여기에 추가
        }
    }

    public void BuyPumpkinSeed()
    {
        int currentPlayerMoney = GameManager.Instance.playerMoney;
        if (currentPlayerMoney >= pumpkinSeedPrice)
        {
            GameManager.Instance.playerMoney -= pumpkinSeedPrice;
            CropManager.Instance.seedPumpkin++;
            UIManager.Instance.UpdateMoneyText();
            UIManager.Instance.UpdateInventoryText();
        }
        else
        {
            // "돈 부족" 팝업을 여기에 추가
        }
    }

    public void BuyPrayRain()
    {
        int currentPlayerMoney = GameManager.Instance.playerMoney;
        if (currentPlayerMoney >= prayRainPrice && WeatherManager.Instance.probability < 0.8f)
        {
            GameManager.Instance.playerMoney -= prayRainPrice; 
            WeatherManager.Instance.probability += 0.05f;
            UIManager.Instance.UpdateMoneyText();
            UIManager.Instance.UpdateWeatherProbabilityText();
        }
        else if(WeatherManager.Instance.probability >= 0.8f)
        {
            UIManager.Instance.storeNoticeText.GetComponent<TextMeshProUGUI>().text = "강수 확률이 최대입니다!";
        }
        else
        {
            // "돈 부족" 팝업을 여기에 추가
        }
    }

    public void BuyScarecrow()
    {
        int currentPlayerMoney = GameManager.Instance.playerMoney;
        if (currentPlayerMoney >= scarecrowPrice)
        {
            GameManager.Instance.playerMoney -= scarecrowPrice;
            GameManager.Instance.scarecrowCount++;
            UIManager.Instance.UpdateMoneyText();
            UIManager.Instance.UpdateInventoryText();
        }
        else
        {

        }
    }
}