using TMPro;
using UnityEngine;

public class StoreScript : MonoBehaviour
{

    [SerializeField] private bool isPlayerInRange = false;

    [Header("상점 아이템 가역")]
    [SerializeField] public int fertilizerPrice = 90;
    [SerializeField] public int parsnipSeedPrice = 15;
    [SerializeField] public int carrotSeedPrice = 30;
    [SerializeField] public int radishSeedPrice = 50;
    [SerializeField] public int potatoSeedPrice = 80;
    [SerializeField] public int eggplantSeedPrice = 130;
    [SerializeField] public int pumpkinSeedPrice = 200;
    [SerializeField] public int prayRainPrice = 1000;
    [SerializeField] public int scarecrowPrice = 450;
    [SerializeField] public int pesticidePrice = 500;
    [SerializeField] public int nutritionPrice = 200;
    [SerializeField] public int manurePrice = 350;
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
            InputManager.Instance.isPlayerInputLocked = UIManager.Instance.storeUI.activeSelf;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(UIManager.Instance == null) return;
            UIManager.Instance.HideStoreNoticeText();
            if (UIManager.Instance.storeUI.activeSelf)
            {
                UIManager.Instance.storeUI.SetActive(false);
                InputManager.Instance.isPlayerInputLocked = UIManager.Instance.storeUI.activeSelf;
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

    public void BuyPesticide()
    {
        int currentPlayerMoney = GameManager.Instance.playerMoney;
        if (currentPlayerMoney >= pesticidePrice)
        {
            GameManager.Instance.playerMoney -= pesticidePrice;
            GameManager.Instance.pesticideCount++;

            UIManager.Instance.UpdateMoneyText();
            UIManager.Instance.UpdateInventoryText();
        }
        else
        {
        }
    }

    public void BuyNutrition()
    {
        int currentPlayerMoney = GameManager.Instance.playerMoney;
        if (currentPlayerMoney >= nutritionPrice)
        {
            GameManager.Instance.playerMoney -= nutritionPrice;
            GameManager.Instance.nutritionCount++;

            UIManager.Instance.UpdateMoneyText();
            UIManager.Instance.UpdateInventoryText();
        }
        else
        {
        }
    }

    public void BuyManure()
    {
        int currentPlayerMoney = GameManager.Instance.playerMoney;
        if (currentPlayerMoney >= manurePrice)
        {
            GameManager.Instance.playerMoney -= manurePrice;
            GameManager.Instance.manureCount++;
            UIManager.Instance.UpdateMoneyText();
            UIManager.Instance.UpdateInventoryText();
        }
        else
        {
        }
    }
}