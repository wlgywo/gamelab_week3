using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class FairyManager : MonoBehaviour
{
    public static FairyManager Instance { get; private set; }

    [Header("요정 이벤트 설정")]
    [SerializeField, Range(0f, 1f)]
    public float fairyAppearChance = 0.1f; // 요정 등장 확률 (10%)

    [Header("요정 스폰 설정")]
    public GameObject fairyPrefab; // 인스펙터에서 요정 프리팹을 할당
    public float spawnOffsetY = 1.0f; // 요정이 타일 위로 떠오를 높이

    // ... (요정 연출에 필요한 다른 변수들) ...
    [Header("요정 연출 설정")]
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
    /// DayTransitionSequence에서 호출할 메인 코루틴.
    /// </summary>
    public IEnumerator CheckAndRunFairyEvent(int newday)
    {
        OnFairyEventStarted?.Invoke(newday);
        // 1. 확률 체크. 당첨되지 않으면 아무것도 안 하고 즉시 종료.
        if (UnityEngine.Random.value > fairyAppearChance)
        {
            yield break; // 코루틴 즉시 중단
        }

        // 2. 스폰 가능한 7칸 라인이 있는지 탐색
        List<TilePrefabs> allTiles = GiantCropManager.Instance.GetAllTiles();
        List<List<TilePrefabs>> validLines = FindValidSevenTileLines(allTiles);

        // 3. 유효한 라인이 없으면 아무것도 안 하고 즉시 종료.
        if (validLines.Count == 0)
        {
            Debug.Log("요정 이벤트에 당첨되었으나, 스폰 가능한 7칸 라인이 없습니다.");
            yield break;
        }

        // --- 여기서부터는 이벤트 발생 조건이 모두 충족된 경우 ---
        Debug.Log("요정 이벤트 발생! 연출을 시작합니다.");

        // 4. 유효한 라인 중 하나를 무작위로 선택
        int randomIndex = UnityEngine.Random.Range(0, validLines.Count);
        List<TilePrefabs> chosenLine = validLines[randomIndex];
        TilePrefabs middleTile = chosenLine[3]; // 중앙 타일

        // 5. 화면을 다시 밝게 하고 연출 시작
        yield return StartCoroutine(UIManager.Instance.FadeIn());

        // 6. 중앙 타일에 요정 스폰 및 연출 실행
        GameObject fairyInstance = SpawnFairyAt(middleTile);
        yield return StartCoroutine(FairyCinematic(fairyInstance)); // 요정 연출 코루틴

        // 7. 식물 성장 및 마무리
        GrowAllPlantsInLine(chosenLine); // 선택된 라인의 식물들을 성장시킴
        yield return new WaitForSecondsRealtime(1.0f);
        Destroy(fairyInstance); // 연출이 끝난 요정 오브젝트 파괴

        // 8. 연출이 끝나면 화면을 다시 어둡게 함
        yield return StartCoroutine(UIManager.Instance.FadeOut());
    }

    /// <summary>
    /// 요정의 움직임 등 실제 연출을 담당하는 코루틴
    /// </summary>
    private IEnumerator FairyCinematic(GameObject fairy)
    {
        CinemachineCamera cineCam = cineCamera.GetComponent<CinemachineCamera>();
        CinemachineBrain cineBrain = camera.GetComponent<CinemachineBrain>();

        // ManualUpdate로 전환하고 IgnoreTimeScale도 설정
        cineBrain.IgnoreTimeScale = true;
        //cineBrain.UpdateMethod = CinemachineBrain.UpdateMethods.ManualUpdate;

        // 타겟 변경
        cineCam.Follow = fairy.transform;
        cineCam.LookAt = fairy.transform;

        // 첫 프레임 강제 업데이트
        cineBrain.ManualUpdate();

        Vector3 startPos = fairy.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < fairyMoveDuration)
        {
            float deltaTime = Time.unscaledDeltaTime;
            elapsedTime += deltaTime;

            // 위아래로 떠다니는 움직임
            float yOffset = Mathf.Sin(elapsedTime * 2f) * 0.2f;
            fairy.transform.position = startPos + new Vector3(0, yOffset, 0);

            // ★ 핵심: ManualUpdate를 매 프레임 호출
            cineBrain.ManualUpdate();
            Camera.main.transform.position = new Vector3(fairy.transform.position.x, fairy.transform.position.y, -10);
            yield return null;
        }

        // 플레이어로 복귀
        cineCam.Follow = player.transform;
        cineCam.LookAt = player.transform;

        // 마지막 업데이트
        cineBrain.ManualUpdate();

        // 원래 설정으로 복구
        // cineBrain.UpdateMethod = CinemachineBrain.UpdateMethods.SmartUpdate;
        cineBrain.IgnoreTimeScale = false;
    }


    /// <summary>
    /// 작물이 심어진 연속된 7칸 타일 라인을 모두 찾아 리스트로 반환합니다.
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
        // 타일을 x, y 좌표 순으로 정렬하여 탐색 효율을 높입니다.
        var sortedTiles = allTiles.OrderBy(t => t.transform.position.x).ThenBy(t => t.transform.position.y).ToList();

        foreach (TilePrefabs startTile in sortedTiles)
        {
            if (processedTiles.Contains(startTile)) continue;

            List<TilePrefabs> currentLine = new List<TilePrefabs>();
            // 가로로 연속된 7칸 탐색
            for (int i = 0; i < 7; i++)
            {
                Vector2 targetPosition = (Vector2)startTile.transform.position + new Vector2(i, 0);
                TilePrefabs foundTile = sortedTiles.FirstOrDefault(t => (Vector2)t.transform.position == targetPosition);

                // 타일이 존재하고, 해당 타일에 작물이 심어져 있어야 라인에 추가
                if (foundTile != null && foundTile.GetContainedCrop() != null)
                {
                    currentLine.Add(foundTile);
                }
                else
                {
                    // 연속된 라인이 끊기면 초기화하고 중단
                    currentLine.Clear();
                    break;
                }
            }

            // 7칸 라인을 성공적으로 찾았을 경우
            if (currentLine.Count == 7)
            {
                // ★ 핵심 변경점: 라인에 있는 작물 중 하나라도 덜 자란 것이 있는지 확인
                if (currentLine.Any(tile => !tile.GetContainedCrop().IsFullyGrown()))
                {
                    // 덜 자란 작물이 하나라도 있다면, 유효한 라인으로 추가
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
    /// 지정된 타일 위치에 요정을 스폰하고, 생성된 게임오브젝트를 반환합니다.
    /// </summary>
    private GameObject SpawnFairyAt(TilePrefabs targetTile)
    {
        if (fairyPrefab == null) return null;
        Vector3 spawnPosition = targetTile.transform.position + new Vector3(0, spawnOffsetY, 0);
        GameObject fairyInstance = Instantiate(fairyPrefab, spawnPosition, Quaternion.identity);
        Debug.Log($"요정이 {targetTile.name} 위치에 스폰되었습니다.");
        return fairyInstance;
    }

    /// <summary>
    /// 주어진 라인의 모든 작물을 다 자라게 합니다.
    /// </summary>
    private void GrowAllPlantsInLine(List<TilePrefabs> line)
    {
        Debug.Log("요정의 축복으로 선택된 라인의 모든 작물이 자랍니다!");
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
