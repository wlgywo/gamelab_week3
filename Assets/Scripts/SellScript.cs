using UnityEngine;

public class SellScript : MonoBehaviour
{
    [SerializeField] private bool isPlayerInRange = false;
    [SerializeField] private int parsnipPrice = 50;

    void Start()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.DoPlant += OpenSellUI;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.DoPlant -= OpenSellUI;
        }
    }

    private void TrySellCrops()
    {
        if (!isPlayerInRange) return;

        int amountToSell = CropManager.Instance.harvestAmountParsnip;

        if (amountToSell > 0)
        {
            int earnings = amountToSell * parsnipPrice;
            GameManager.Instance.AddMoney(earnings);
            CropManager.Instance.harvestAmountParsnip = 0;
            UIManager.Instance.UpdateHarvestText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.ShowSellNoticeText();
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.HideSellNoticeText();

            if (UIManager.Instance.sellUI.activeSelf)
            {
                UIManager.Instance.sellUI.SetActive(false);
            }
            isPlayerInRange = false;
        }
    }

    private void OpenSellUI()
    {
        if (isPlayerInRange)
        {
            UIManager.Instance.ToggleSellUI();
        }
    }
    public void OnClickSellButton()
    {
        TrySellCrops();
    }
}
