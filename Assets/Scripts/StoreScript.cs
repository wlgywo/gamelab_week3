using UnityEngine;

public class StoreScript : MonoBehaviour
{

    [SerializeField] private bool isPlayerInRange = false;

    [Header("상점 아이템 가역")]
    [SerializeField] private int parsnipSeedPrice = 20;
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
}