using UnityEngine;

[CreateAssetMenu(fileName = "New Crop Data", menuName = "Crop/Crop Data")]
public class Seed : ScriptableObject
{
    [Header("�⺻ ����")]
    public string cropName;
    public string cropDescription;

    [Header("���� ����")]
    public int growthDays;
    // public GameObject harvestedItemPrefab; 
}
