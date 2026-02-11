using System.Collections.Generic;
using UnityEngine;

public class CropManager : MonoBehaviour
{
    public static CropManager Instance { get; private set; }

    // µñ¼Å³Ê¸®·Î °ü¸®
    // public int harvestAmountParsnip = 0;
    // public int harvestAmountCarrot = 0;

    public int seedParsnip = 0;   
    public int seedCarrot = 0;
    public int seedRadish = 0;
    public int seedPotato = 0;
    public int seedEggplant = 0;
    public int seedPumpkin = 0;

    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddHarvestedItem(string itemID, int amount)
    {
        if (inventory.ContainsKey(itemID))
        {
            inventory[itemID] += amount;
        }
        else
        {
            inventory.Add(itemID, amount);
        }

        UIManager.Instance.UpdateHarvestText();
    }
}
