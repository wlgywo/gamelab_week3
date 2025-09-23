using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class FairyManager : MonoBehaviour
{
    public static FairyManager Instance { get; private set; }

    [Header("���� �̺�Ʈ ����")]
    [SerializeField, Range(0f, 1f)]
    public float fairyAppearChance = 0.1f; // ���� ���� Ȯ�� (10%)

    [Header("���� ���� ����")]
    public GameObject fairyPrefab; // �ν����Ϳ��� ���� �������� �Ҵ�
    public float spawnOffsetY = 1.0f; // ������ Ÿ�� ���� ������ ����

    // ... (���� ���⿡ �ʿ��� �ٸ� ������) ...
    [Header("���� ���� ����")]
    [SerializeField] private float fairyMoveDuration = 3.0f;
    public GameObject cineCamera;
    public GameObject player;
    public Camera camera;

    public event Action<int> OnFairyEventStarted;

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

    /// <summary>
    /// DayTransitionSequence���� ȣ���� ���� �ڷ�ƾ.
    /// </summary>
    public IEnumerator CheckAndRunFairyEvent(int newday)
    {
        OnFairyEventStarted?.Invoke(newday);
        // 1. Ȯ�� üũ. ��÷���� ������ �ƹ��͵� �� �ϰ� ��� ����.
        if (UnityEngine.Random.value > fairyAppearChance)
        {
            yield break; // �ڷ�ƾ ��� �ߴ�
        }

        // 2. ���� ������ 7ĭ ������ �ִ��� Ž��
        List<TilePrefabs> allTiles = GiantCropManager.Instance.GetAllTiles();
        List<List<TilePrefabs>> validLines = FindValidSevenTileLines(allTiles);

        // 3. ��ȿ�� ������ ������ �ƹ��͵� �� �ϰ� ��� ����.
        if (validLines.Count == 0)
        {
            Debug.Log("���� �̺�Ʈ�� ��÷�Ǿ�����, ���� ������ 7ĭ ������ �����ϴ�.");
            yield break;
        }

        // --- ���⼭���ʹ� �̺�Ʈ �߻� ������ ��� ������ ��� ---
        Debug.Log("���� �̺�Ʈ �߻�! ������ �����մϴ�.");

        // 4. ��ȿ�� ���� �� �ϳ��� �������� ����
        int randomIndex = UnityEngine.Random.Range(0, validLines.Count);
        List<TilePrefabs> chosenLine = validLines[randomIndex];
        TilePrefabs middleTile = chosenLine[3]; // �߾� Ÿ��

        // 5. ȭ���� �ٽ� ��� �ϰ� ���� ����
        yield return StartCoroutine(UIManager.Instance.FadeIn());

        // 6. �߾� Ÿ�Ͽ� ���� ���� �� ���� ����
        GameObject fairyInstance = SpawnFairyAt(middleTile);
        yield return StartCoroutine(FairyCinematic(fairyInstance)); // ���� ���� �ڷ�ƾ

        // 7. �Ĺ� ���� �� ������
        GrowAllPlantsInLine(chosenLine); // ���õ� ������ �Ĺ����� �����Ŵ
        yield return new WaitForSecondsRealtime(1.0f);
        Destroy(fairyInstance); // ������ ���� ���� ������Ʈ �ı�

        // 8. ������ ������ ȭ���� �ٽ� ��Ӱ� ��
        yield return StartCoroutine(UIManager.Instance.FadeOut());
    }

    /// <summary>
    /// ������ ������ �� ���� ������ ����ϴ� �ڷ�ƾ
    /// </summary>
    private IEnumerator FairyCinematic(GameObject fairy)
    {
        CinemachineCamera cineCam = cineCamera.GetComponent<CinemachineCamera>();
        CinemachineBrain cineBrain = camera.GetComponent<CinemachineBrain>();

        // ManualUpdate�� ��ȯ�ϰ� IgnoreTimeScale�� ����
        cineBrain.IgnoreTimeScale = true;
        //cineBrain.UpdateMethod = CinemachineBrain.UpdateMethods.ManualUpdate;

        // Ÿ�� ����
        cineCam.Follow = fairy.transform;
        cineCam.LookAt = fairy.transform;

        // ù ������ ���� ������Ʈ
        cineBrain.ManualUpdate();

        Vector3 startPos = fairy.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < fairyMoveDuration)
        {
            float deltaTime = Time.unscaledDeltaTime;
            elapsedTime += deltaTime;

            // ���Ʒ��� ���ٴϴ� ������
            float yOffset = Mathf.Sin(elapsedTime * 2f) * 0.2f;
            fairy.transform.position = startPos + new Vector3(0, yOffset, 0);

            // �� �ٽ�: ManualUpdate�� �� ������ ȣ��
            cineBrain.ManualUpdate();
            Camera.main.transform.position = new Vector3(fairy.transform.position.x, fairy.transform.position.y, -10);
            yield return null;
        }

        // �÷��̾�� ����
        cineCam.Follow = player.transform;
        cineCam.LookAt = player.transform;

        // ������ ������Ʈ
        cineBrain.ManualUpdate();

        // ���� �������� ����
        // cineBrain.UpdateMethod = CinemachineBrain.UpdateMethods.SmartUpdate;
        cineBrain.IgnoreTimeScale = false;
    }


    /// <summary>
    /// �۹��� �ɾ��� ���ӵ� 7ĭ Ÿ�� ������ ��� ã�� ����Ʈ�� ��ȯ�մϴ�.
    /// </summary>
    private List<List<TilePrefabs>> FindValidSevenTileLines(List<TilePrefabs> allTiles)
    {
        /* var validLines = new List<List<TilePrefabs>>();
         HashSet<TilePrefabs> processedTiles = new HashSet<TilePrefabs>();
         var sortedTiles = allTiles.OrderBy(t => t.transform.position.x).ThenBy(t => t.transform.position.y).ToList();

         foreach (TilePrefabs startTile in sortedTiles)
         {
             if (processedTiles.Contains(startTile)) continue;

             List<TilePrefabs> currentLine = new List<TilePrefabs>();
             for (int i = 0; i < 7; i++)
             {
                 Vector2 targetPosition = (Vector2)startTile.transform.position + new Vector2(i, 0);
                 TilePrefabs foundTile = sortedTiles.FirstOrDefault(t => (Vector2)t.transform.position == targetPosition);
                 if (foundTile != null && foundTile.GetContainedCrop() != null)
                 {
                     currentLine.Add(foundTile);
                 }
                 else
                 {
                     currentLine.Clear();
                     break;
                 }
             }

             if (currentLine.Count == 7)
             {
                 validLines.Add(currentLine);
                 foreach (var tile in currentLine)
                 {
                     processedTiles.Add(tile);
                 }
             }
         }
         return validLines;*/

        var validLines = new List<List<TilePrefabs>>();
        HashSet<TilePrefabs> processedTiles = new HashSet<TilePrefabs>();
        // Ÿ���� x, y ��ǥ ������ �����Ͽ� Ž�� ȿ���� ���Դϴ�.
        var sortedTiles = allTiles.OrderBy(t => t.transform.position.x).ThenBy(t => t.transform.position.y).ToList();

        foreach (TilePrefabs startTile in sortedTiles)
        {
            if (processedTiles.Contains(startTile)) continue;

            List<TilePrefabs> currentLine = new List<TilePrefabs>();
            // ���η� ���ӵ� 7ĭ Ž��
            for (int i = 0; i < 7; i++)
            {
                Vector2 targetPosition = (Vector2)startTile.transform.position + new Vector2(i, 0);
                TilePrefabs foundTile = sortedTiles.FirstOrDefault(t => (Vector2)t.transform.position == targetPosition);

                // Ÿ���� �����ϰ�, �ش� Ÿ�Ͽ� �۹��� �ɾ��� �־�� ���ο� �߰�
                if (foundTile != null && foundTile.GetContainedCrop() != null)
                {
                    currentLine.Add(foundTile);
                }
                else
                {
                    // ���ӵ� ������ ����� �ʱ�ȭ�ϰ� �ߴ�
                    currentLine.Clear();
                    break;
                }
            }

            // 7ĭ ������ ���������� ã���� ���
            if (currentLine.Count == 7)
            {
                // �� �ٽ� ������: ���ο� �ִ� �۹� �� �ϳ��� �� �ڶ� ���� �ִ��� Ȯ��
                if (currentLine.Any(tile => !tile.GetContainedCrop().IsFullyGrown()))
                {
                    // �� �ڶ� �۹��� �ϳ��� �ִٸ�, ��ȿ�� �������� �߰�
                    validLines.Add(currentLine);
                    foreach (var tile in currentLine)
                    {
                        processedTiles.Add(tile);
                    }
                }
            }
        }
        return validLines;
    }

    /// <summary>
    /// ������ Ÿ�� ��ġ�� ������ �����ϰ�, ������ ���ӿ�����Ʈ�� ��ȯ�մϴ�.
    /// </summary>
    private GameObject SpawnFairyAt(TilePrefabs targetTile)
    {
        if (fairyPrefab == null) return null;
        Vector3 spawnPosition = targetTile.transform.position + new Vector3(0, spawnOffsetY, 0);
        GameObject fairyInstance = Instantiate(fairyPrefab, spawnPosition, Quaternion.identity);
        Debug.Log($"������ {targetTile.name} ��ġ�� �����Ǿ����ϴ�.");
        return fairyInstance;
    }

    /// <summary>
    /// �־��� ������ ��� �۹��� �� �ڶ�� �մϴ�.
    /// </summary>
    private void GrowAllPlantsInLine(List<TilePrefabs> line)
    {
        Debug.Log("������ �ູ���� ���õ� ������ ��� �۹��� �ڶ��ϴ�!");
        foreach (var tile in line)
        {
            CropBehaviour crop = tile.GetContainedCrop();
            if (crop != null && !crop.IsFullyGrown())
            {
                crop.daysSincePlanted = crop.cropData.growthDays-1;
            }
        }
    }
}
