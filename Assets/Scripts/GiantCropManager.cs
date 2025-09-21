using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GiantCropManager : MonoBehaviour
{
    public static GiantCropManager Instance { get; private set; }

    /*[Header("거대 작물 설정")]
    [Tooltip("거대 파스닙 프리팹을 연결하세요.")]
    public GameObject giantParsnipPrefab;*/

    [Tooltip("거대 작물이 나타날 확률 (0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    public float giantCropChance = 0.1f; // 10% 확률
    public float spawnOffsetY = 1f; // 타일 위로 약간 띄우기 위한 오프셋

    private List<TilePrefabs> allTiles = new List<TilePrefabs>();

    public void RegisterTile(TilePrefabs tile)
    {
        if (!allTiles.Contains(tile))
        {
            allTiles.Add(tile);
        }
    }


    public void UnregisterTile(TilePrefabs tile)
    {
        if (allTiles.Contains(tile))
        {
            allTiles.Remove(tile);
        }
    }

    public List<TilePrefabs> GetAllTiles()
    {
        return new List<TilePrefabs>(allTiles); // 원본 리스트를 보호하기 위해 사본을 반환
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*public void CheckForGiantCrops()
    {
        List<TilePrefabs> availableTiles = allTiles.Where(t => !t.isOccupiedByGiantCrop).ToList();
        HashSet<TilePrefabs> processedTiles = new HashSet<TilePrefabs>();

        foreach (TilePrefabs middleTile in availableTiles)
        {
            if (processedTiles.Contains(middleTile)) continue;

            CropBehaviour middleCrop = middleTile.GetContainedCrop();

            // ▼▼▼ 3. 중앙 타일 조건 확인 (수정된 부분) ▼▼▼
            // 다 자랐는지 + "오늘 물을 줬는지" + 작물 이름이 맞는지 확인
            if (middleCrop == null || !middleCrop.IsFullyGrown() || middleCrop.isEaten || !middleTile.isWatered || middleCrop.cropData.cropName != "Parsnip")
            {
                continue;
            }

            TilePrefabs leftTile = FindNeighborTile(middleTile, Vector2.left, availableTiles);
            TilePrefabs rightTile = FindNeighborTile(middleTile, Vector2.right, availableTiles);

            // ▼▼▼ 4. 주변 타일 조건 확인 (수정된 부분) ▼▼▼
            // 주변 타일이 존재하고, "아직 다른 거대 작물에 포함되지 않았는지" 확인
            if (leftTile != null && rightTile != null && !processedTiles.Contains(leftTile) && !processedTiles.Contains(rightTile))
            {
                CropBehaviour leftCrop = leftTile.GetContainedCrop();
                CropBehaviour rightCrop = rightTile.GetContainedCrop();
       
                if (leftCrop != null && rightCrop != null &&
                    leftCrop.cropData.cropName == "Parsnip" && rightCrop.cropData.cropName == "Parsnip")
                {
                    if (leftCrop.isEaten || rightCrop.isEaten) continue;
                    if (Random.Range(0f, 1f) <= giantCropChance)
                    {
                        SpawnGiantCrop(giantParsnipPrefab, leftTile, middleTile, rightTile, middleTile.gameObject);

                        // 사용된 타일들을 "처리됨" 목록에 추가
                        processedTiles.Add(leftTile);
                        processedTiles.Add(middleTile);
                        processedTiles.Add(rightTile);
                    }
                }
            }
        }
    }*/

    public void CheckForGiantCrops()
    {
        List<TilePrefabs> availableTiles = allTiles.Where(t => !t.isOccupiedByGiantCrop).ToList();
        HashSet<TilePrefabs> processedTiles = new HashSet<TilePrefabs>();

        foreach (TilePrefabs middleTile in availableTiles)
        {
            if (processedTiles.Contains(middleTile)) continue;

            CropBehaviour middleCrop = middleTile.GetContainedCrop();

            // 1. 기본 조건 확인 (다 자랐는지, 물은 줬는지 등)
            if (middleCrop == null || !middleCrop.IsFullyGrown() || middleCrop.isEaten || !middleTile.isWatered)
            {
                continue;
            }

            // 2. (수정) 이 작물이 거대 작물이 될 수 있는지 Seed 데이터를 통해 확인
            if (middleCrop.cropData.giantVersionPrefab == null)
            {
                continue; // 거대 작물 버전이 등록 안되어 있으면 건너뛰기
            }

            TilePrefabs leftTile = FindNeighborTile(middleTile, Vector2.left, availableTiles);
            TilePrefabs rightTile = FindNeighborTile(middleTile, Vector2.right, availableTiles);

            if (leftTile != null && rightTile != null && !processedTiles.Contains(leftTile) && !processedTiles.Contains(rightTile))
            {
                CropBehaviour leftCrop = leftTile.GetContainedCrop();
                CropBehaviour rightCrop = rightTile.GetContainedCrop();

                if (leftCrop != null && rightCrop != null && !leftCrop.isEaten && !rightCrop.isEaten)
                {
                    // 3. (수정) 주변 작물들이 중앙 작물과 '같은 종류'인지 ItemID로 확인
                    string middleCropID = middleCrop.cropData.harvestedItemID;
                    if (leftCrop.cropData.harvestedItemID == middleCropID && rightCrop.cropData.harvestedItemID == middleCropID)
                    {
                        if (Random.Range(0f, 1f) <= giantCropChance)
                        {
                            // 4. (수정) 생성할 거대 작물 프리팹을 Seed 데이터에서 직접 가져와 사용
                            SpawnGiantCrop(middleCrop.cropData.giantVersionPrefab, leftTile, middleTile, rightTile);

                            processedTiles.Add(leftTile);
                            processedTiles.Add(middleTile);
                            processedTiles.Add(rightTile);
                        }
                    }
                }
            }
        }
    }

    private TilePrefabs FindNeighborTile(TilePrefabs origin, Vector2 direction, List<TilePrefabs> allTiles)
    {
        Vector2 targetPosition = (Vector2)origin.transform.position + direction;
        return allTiles.FirstOrDefault(t => (Vector2)t.transform.position == targetPosition);
    }

    private void SpawnGiantCrop(GameObject giantCropPrefab, TilePrefabs left, TilePrefabs middle, TilePrefabs right)
    {
        Debug.Log("거대 작물 생성!");

        Destroy(left.GetComponentInChildren<CropBehaviour>().gameObject);
        Destroy(middle.GetComponentInChildren<CropBehaviour>().gameObject);
        Destroy(right.GetComponentInChildren<CropBehaviour>().gameObject);

        left.isOccupiedByGiantCrop = true;
        middle.isOccupiedByGiantCrop = true;
        right.isOccupiedByGiantCrop = true;

        Instantiate(giantCropPrefab, middle.transform.position + Vector3.up*spawnOffsetY, Quaternion.identity, middle.gameObject.transform);
    }
}
