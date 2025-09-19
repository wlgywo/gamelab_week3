using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GiantCropManager : MonoBehaviour
{
    public static GiantCropManager Instance { get; private set; }

    [Header("�Ŵ� �۹� ����")]
    [Tooltip("�Ŵ� �Ľ��� �������� �����ϼ���.")]
    public GameObject giantParsnipPrefab;

    [Tooltip("�Ŵ� �۹��� ��Ÿ�� Ȯ�� (0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    public float giantCropChance = 0.1f; // 10% Ȯ��
    public float spawnOffsetY = 1f; // Ÿ�� ���� �ణ ���� ���� ������

    private List<TilePrefabs> allTiles = new List<TilePrefabs>();

    // Ÿ���� �ڽ��� ����Ʈ�� �߰��� �� �ֵ��� ����(public) �Լ��� ����ϴ�.
    public void RegisterTile(TilePrefabs tile)
    {
        if (!allTiles.Contains(tile))
        {
            allTiles.Add(tile);
        }
    }

    // Ÿ���� �ı��� �� �ڽ��� ����Ʈ���� ������ �� �ֵ��� �Լ��� ����ϴ�.
    public void UnregisterTile(TilePrefabs tile)
    {
        if (allTiles.Contains(tile))
        {
            allTiles.Remove(tile);
        }
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

    public void CheckForGiantCrops()
    {
        List<TilePrefabs> availableTiles = allTiles.Where(t => !t.isOccupiedByGiantCrop).ToList();
        HashSet<TilePrefabs> processedTiles = new HashSet<TilePrefabs>();

        foreach (TilePrefabs middleTile in availableTiles)
        {
            if (processedTiles.Contains(middleTile)) continue;

            CropBehaviour middleCrop = middleTile.GetContainedCrop();

            // ���� 3. �߾� Ÿ�� ���� Ȯ�� (������ �κ�) ����
            // �� �ڶ����� + "���� ���� �����" + �۹� �̸��� �´��� Ȯ��
            if (middleCrop == null || !middleCrop.IsFullyGrown() || !middleTile.isWateredToday || middleCrop.cropData.cropName != "Parsnip")
            {
                continue;
            }

            TilePrefabs leftTile = FindNeighborTile(middleTile, Vector2.left, availableTiles);
            TilePrefabs rightTile = FindNeighborTile(middleTile, Vector2.right, availableTiles);

            // ���� 4. �ֺ� Ÿ�� ���� Ȯ�� (������ �κ�) ����
            // �ֺ� Ÿ���� �����ϰ�, "���� �ٸ� �Ŵ� �۹��� ���Ե��� �ʾҴ���" Ȯ��
            if (leftTile != null && rightTile != null && !processedTiles.Contains(leftTile) && !processedTiles.Contains(rightTile))
            {
                CropBehaviour leftCrop = leftTile.GetContainedCrop();
                CropBehaviour rightCrop = rightTile.GetContainedCrop();

                if (leftCrop != null && rightCrop != null &&
                    leftCrop.cropData.cropName == "Parsnip" && rightCrop.cropData.cropName == "Parsnip")
                {
                    if (Random.Range(0f, 1f) <= giantCropChance)
                    {
                        SpawnGiantCrop(giantParsnipPrefab, leftTile, middleTile, rightTile, middleTile.gameObject);

                        // ���� Ÿ�ϵ��� "ó����" ��Ͽ� �߰�
                        processedTiles.Add(leftTile);
                        processedTiles.Add(middleTile);
                        processedTiles.Add(rightTile);
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

    private void SpawnGiantCrop(GameObject giantCropPrefab, TilePrefabs left, TilePrefabs middle, TilePrefabs right, GameObject closestTile)
    {
        Debug.Log("�Ŵ� �۹� ����!");

        Destroy(left.GetComponentInChildren<CropBehaviour>().gameObject);
        Destroy(middle.GetComponentInChildren<CropBehaviour>().gameObject);
        Destroy(right.GetComponentInChildren<CropBehaviour>().gameObject);

        left.isOccupiedByGiantCrop = true;
        middle.isOccupiedByGiantCrop = true;
        right.isOccupiedByGiantCrop = true;

        Instantiate(giantCropPrefab, middle.transform.position + Vector3.up*spawnOffsetY, Quaternion.identity, middle.gameObject.transform);
    }
}
