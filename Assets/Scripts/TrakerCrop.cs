using UnityEngine;

public class TrakerCrop : MonoBehaviour
{
    // ���� ���¸� ����
    private enum FollowMode { Direct, Path }
    private FollowMode currentMode = FollowMode.Direct;

    [Header("���� ���")]
    [SerializeField] private Transform playerTransform;

    [Header("���� ����")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float stoppingDistance = 2f;
    [Tooltip("�� �Ÿ� �̻� �������� ��� ������ �����մϴ�.")]
    [SerializeField] private float pathFollowThreshold = 5f;

    [Header("��ֹ� ����")]
    [Tooltip("��ֹ��� �ν��� ���̾ �����ϼ��� (��: Ground, Platform)")]
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

        // --- ��� ���� ���� ---
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // �����ڿ� �÷��̾� ���̿� ��ֹ��� �ִ��� Ȯ�� (Raycast)
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);

        // ��ֹ��� �ְų�, �ʹ� �־����� '��� ����' ���� ����
        if (hit.collider != null || distanceToPlayer > pathFollowThreshold)
        {
            currentMode = FollowMode.Path;
        }
        else
        {
            currentMode = FollowMode.Direct;
        }

        // --- ���� �̵� ���� ---
        if (currentMode == FollowMode.Direct)
        {
            // [���� ���� ���] �ε巴�� �÷��̾ ����
            if (distanceToPlayer > stoppingDistance)
            {
                transform.position = Vector3.SmoothDamp(transform.position, playerTransform.position, ref velocity, 1 / moveSpeed);
            }
        }
        else // currentMode == FollowMode.Path
        {
            // [��� ���� ���] �÷��̾ ���� ��θ� ����
            if (CharacterTrack.pathPoints.Count > 1) // �ּ� 2���� ��� ����Ʈ�� ���� ��
            {
                Vector3 targetPathPoint = CharacterTrack.pathPoints.Peek();
                transform.position = Vector3.MoveTowards(transform.position, targetPathPoint, moveSpeed * Time.deltaTime);

                // ��ǥ ��� ������ �����ϸ� �ش� ������ ť���� ����
                if (Vector3.Distance(transform.position, targetPathPoint) < 0.1f)
                {
                    CharacterTrack.pathPoints.Dequeue();
                }
            }
        }

        // --- ��������Ʈ ���� ��ȯ ---
        HandleSpriteFlip();
    }

    void HandleSpriteFlip()
    {
        bool shouldFlip = false;
        // ��ǥ ������ �������� ���� ��ȯ
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
