using UnityEngine;

public class TrakerCrop : MonoBehaviour
{
    // 추적 상태를 정의
    private enum FollowMode { Direct, Path }
    private FollowMode currentMode = FollowMode.Direct;

    [Header("따라갈 대상")]
    [SerializeField] private Transform playerTransform;

    [Header("추적 설정")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float stoppingDistance = 2f;
    [Tooltip("이 거리 이상 벌어지면 경로 추적을 시작합니다.")]
    [SerializeField] private float pathFollowThreshold = 5f;

    [Header("장애물 감지")]
    [Tooltip("장애물로 인식할 레이어를 선택하세요 (예: Ground, Platform)")]
    [SerializeField] private LayerMask obstacleLayer;

    private SpriteRenderer[] childSprites;
    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        childSprites = GetComponentsInChildren<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (playerTransform == null) return;

        // --- 모드 결정 로직 ---
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // 추적자와 플레이어 사이에 장애물이 있는지 확인 (Raycast)
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);

        // 장애물이 있거나, 너무 멀어지면 '경로 추적' 모드로 변경
        if (hit.collider != null || distanceToPlayer > pathFollowThreshold)
        {
            currentMode = FollowMode.Path;
        }
        else
        {
            currentMode = FollowMode.Direct;
        }

        // --- 실제 이동 로직 ---
        if (currentMode == FollowMode.Direct)
        {
            // [직선 추적 모드] 부드럽게 플레이어를 따라감
            if (distanceToPlayer > stoppingDistance)
            {
                transform.position = Vector3.SmoothDamp(transform.position, playerTransform.position, ref velocity, 1 / moveSpeed);
            }
        }
        else // currentMode == FollowMode.Path
        {
            // [경로 추적 모드] 플레이어가 남긴 경로를 따라감
            if (CharacterTrack.pathPoints.Count > 1) // 최소 2개의 경로 포인트가 있을 때
            {
                Vector3 targetPathPoint = CharacterTrack.pathPoints.Peek();
                transform.position = Vector3.MoveTowards(transform.position, targetPathPoint, moveSpeed * Time.deltaTime);

                // 목표 경로 지점에 도착하면 해당 지점은 큐에서 제거
                if (Vector3.Distance(transform.position, targetPathPoint) < 0.1f)
                {
                    CharacterTrack.pathPoints.Dequeue();
                }
            }
        }

        // --- 스프라이트 방향 전환 ---
        HandleSpriteFlip();
    }

    void HandleSpriteFlip()
    {
        bool shouldFlip = false;
        // 목표 지점을 기준으로 방향 전환
        if (currentMode == FollowMode.Direct)
        {
            shouldFlip = (playerTransform.position.x < transform.position.x);
        }
        else if (CharacterTrack.pathPoints.Count > 0)
        {
            shouldFlip = (CharacterTrack.pathPoints.Peek().x < transform.position.x);
        }

        foreach (var sprite in childSprites)
        {
            sprite.flipX = shouldFlip;
        }
    }
}
