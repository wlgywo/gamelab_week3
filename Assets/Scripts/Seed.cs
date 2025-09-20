using UnityEngine;

[CreateAssetMenu(fileName = "New Crop Data", menuName = "Crop/Crop Data")]
public class Seed : ScriptableObject
{
    [Header("�⺻ ����")]
    public string cropName;
    public string cropDescription;

    // ���� �߰��� ������ ����
    [Tooltip("��Ȯ �� �κ��丮�� �� �������� ID. CropManager���� �� ID�� ����մϴ�.")]
    public string harvestedItemID; // ��: "parsnip_item", "carrot_item"

    [Header("���� ����")]
    public int growthDays;

    [Header("��Ȯ ����")]
    [Tooltip("�Ϲ� ��Ȯ �� �ּ� ��Ȯ��")]
    public int minYield = 1;
    [Tooltip("�Ϲ� ��Ȯ �� �ִ� ��Ȯ�� (min�� max�� ������ ���� ��Ȯ��)")]
    public int maxYield = 1;

    [Tooltip("��� ��� �� �ּ� ��Ȯ��")]
    public int minFertilizedYield = 2;
    [Tooltip("��� ��� �� �ִ� ��Ȯ��")]
    public int maxFertilizedYield = 4;

    [Header("�Ŵ� �۹� ����")]
    [Tooltip("�� �۹��� �Ŵ� �۹� ��ü���� ����")]
    public bool isGiantCrop = false;
    [Tooltip("�Ŵ� �۹��� ��� ���� ��Ȯ��")]
    public int giantCropYield = 10;

    public int sellPrice;

    [Tooltip("�� �۹��� ���� �� �ִ� �Ŵ� �۹� ������. �Ŵ� �۹��� ���ٸ� ����μ���.")]
    public GameObject giantVersionPrefab;
}
