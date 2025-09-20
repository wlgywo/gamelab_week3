using UnityEngine;

[CreateAssetMenu(fileName = "New Crop Data", menuName = "Crop/Crop Data")]
public class Seed : ScriptableObject
{
    [Header("기본 정보")]
    public string cropName;
    public string cropDescription;

    // ▼▼▼ 추가된 데이터 ▼▼▼
    [Tooltip("수확 시 인벤토리에 들어갈 아이템의 ID. CropManager에서 이 ID를 사용합니다.")]
    public string harvestedItemID; // 예: "parsnip_item", "carrot_item"

    [Header("성장 정보")]
    public int growthDays;

    [Header("수확 정보")]
    [Tooltip("일반 수확 시 최소 수확량")]
    public int minYield = 1;
    [Tooltip("일반 수확 시 최대 수확량 (min과 max가 같으면 고정 수확량)")]
    public int maxYield = 1;

    [Tooltip("비료 사용 시 최소 수확량")]
    public int minFertilizedYield = 2;
    [Tooltip("비료 사용 시 최대 수확량")]
    public int maxFertilizedYield = 4;

    [Header("거대 작물 정보")]
    [Tooltip("이 작물이 거대 작물 자체인지 여부")]
    public bool isGiantCrop = false;
    [Tooltip("거대 작물일 경우 고정 수확량")]
    public int giantCropYield = 10;

    public int sellPrice;

    [Tooltip("이 작물이 변할 수 있는 거대 작물 프리팹. 거대 작물이 없다면 비워두세요.")]
    public GameObject giantVersionPrefab;
}
