using UnityEngine;

[CreateAssetMenu(fileName = "New Crop Data", menuName = "Crop/Crop Data")]
public class Seed : ScriptableObject
{
    [Header("기본 정보")]
    public string cropName;
    public string cropDescription;

    [Header("성장 정보")]
    public int growthDays;
    // public GameObject harvestedItemPrefab; 
}
