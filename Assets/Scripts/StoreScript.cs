using UnityEngine;

public class StoreScript : MonoBehaviour
{

    [SerializeField] private bool isPlayerInRange = false;

    [Header("상점 아이템 가역")]
    [SerializeField] private int fertilizerPrice = 50;
    [SerializeField] private int parsnipSeedPrice = 20;
    [SerializeField] private int carrotSeedPrice = 30;
    [SerializeField] private int radishSeedPrice = 40;

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
        if (currentPlayerMoney >= radishSeedPrice)
        {
            GameManager.Instance.playerMoney -= radishSeedPrice;
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
        if (currentPlayerMoney >= radishSeedPrice)
        {
            GameManager.Instance.playerMoney -= radishSeedPrice;
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
        if (currentPlayerMoney >= radishSeedPrice)
        {
            GameManager.Instance.playerMoney -= radishSeedPrice;
            CropManager.Instance.seedPumpkin++;
            UIManager.Instance.UpdateMoneyText();
            UIManager.Instance.UpdateInventoryText();
        }
        else
        {
            // "돈 부족" 팝업을 여기에 추가
        }
    }
}