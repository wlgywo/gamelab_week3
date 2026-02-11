using System.Collections.Generic;
using UnityEngine;

public class CropDatabaseManager : MonoBehaviour
{
    public static CropDatabaseManager Instance { get; private set; }
    public List<Seed> allCropData;
    private Dictionary<string, Seed> cropDataDictionary = new Dictionary<string, Seed>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (Seed data in allCropData)
            {
                if (!cropDataDictionary.ContainsKey(data.harvestedItemID))
                {
                    cropDataDictionary.Add(data.harvestedItemID, data);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Seed GetCropDataByID(string itemID)
    {
        if (cropDataDictionary.TryGetValue(itemID, out Seed data))
        {
            return data;
        }
        return null;
    }
}
