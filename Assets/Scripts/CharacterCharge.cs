using UnityEngine;

public class CharacterCharge : MonoBehaviour
{
    [Header("설정")]
    [SerializeField, Tooltip("최대로 수축했을 때의 X, Y 스케일")]
    private Vector2 maxSqueezeScale = new Vector2(1.3f, 0.7f);

    [SerializeField, Tooltip("원래 크기로 돌아오는 속도")]
    private float returnSpeed = 10f;

    private Vector3 originalScale;
    private bool isReturning = false;

    void Awake()
    {
        // 시작할 때 원래 크기를 저장해 둡니다.
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 원래 크기로 돌아가야 할 때 부드럽게 복원시킵니다.
        if (isReturning)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * returnSpeed);

            // 거의 원래 크기로 돌아왔으면 복원을 중단합니다.
            if (Vector3.Distance(transform.localScale, originalScale) < 0.01f)
            {
                transform.localScale = originalScale;
                isReturning = false;
            }
        }
    }

    /// <summary>
    /// 차지를 시작할 때 호출하는 함수.
    /// </summary>
    public void StartCharge()
    {
        isReturning = false;
        // 필요하다면 여기에 차지 시작 사운드나 파티클을 넣을 수 있습니다.
    }

    /// <summary>
    /// 키를 누르는 동안 매 프레임 호출하며 차지 비율(0.0 ~ 1.0)을 전달하는 함수.
    /// </summary>
    /// <param name="chargeRatio">0.0 (시작) 부터 1.0 (최대) 까지의 차지 비율</param>
    public void UpdateCharge(float chargeRatio)
    {
        isReturning = false;
        // 차지 비율에 따라 스케일(수축) 정도를 계산합니다.
        float targetScaleX = Mathf.Lerp(originalScale.x, maxSqueezeScale.x, chargeRatio);
        float targetScaleY = Mathf.Lerp(originalScale.y, maxSqueezeScale.y, chargeRatio);

        transform.localScale = new Vector3(targetScaleX, targetScaleY, originalScale.z);
    }

    /// <summary>
    /// 차지를 중단하거나 키에서 손을 뗐을 때 호출하는 함수.
    /// </summary>
    public void ReleaseCharge()
    {
        isReturning = true;
        // 필요하다면 여기에 차지 중단/실패 사운드를 넣을 수 있습니다.
    }
}
