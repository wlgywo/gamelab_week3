using System.Collections.Generic;
using UnityEngine;

public class SellScript : MonoBehaviour
{
    [SerializeField] private bool isPlayerInRange = false;
    // [SerializeField] private int parsnipPrice = 50;

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

        var inventory = CropManager.Instance.inventory;
        if (inventory.Count == 0)
        {
            return;
        }

        int totalEarnings = 0;
        foreach (KeyValuePair<string, int> item in inventory)
        {
            string itemID = item.Key;
            int amount = item.Value;

            Seed cropData = CropDatabaseManager.Instance.GetCropDataByID(itemID);

            if (cropData != null)
            {
                int earnings = amount * cropData.sellPrice;
                totalEarnings += earnings;
            }
        }

        if (totalEarnings > 0)
        {

            GameManager.Instance.AddMoney(totalEarnings);
            inventory.Clear();
            UIManager.Instance.UpdateHarvestText();
            UIManager.Instance.earning = totalEarnings;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.ShowSellNoticeText();
            isPlayerInRange = true;
            InputManager.Instance.isPlayerInputLocked = UIManager.Instance.sellUI.activeSelf;
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
                InputManager.Instance.isPlayerInputLocked = UIManager.Instance.sellUI.activeSelf;
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
