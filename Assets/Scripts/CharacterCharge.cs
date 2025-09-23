using UnityEngine;

public class CharacterCharge : MonoBehaviour
{
    [Header("����")]
    [SerializeField, Tooltip("�ִ�� �������� ���� X, Y ������")]
    private Vector2 maxSqueezeScale = new Vector2(1.3f, 0.7f);

    [SerializeField, Tooltip("���� ũ��� ���ƿ��� �ӵ�")]
    private float returnSpeed = 10f;

    private Vector3 originalScale;
    private bool isReturning = false;

    void Awake()
    {
        // ������ �� ���� ũ�⸦ ������ �Ӵϴ�.
        originalScale = transform.localScale;
    }

    void Update()
    {
        // ���� ũ��� ���ư��� �� �� �ε巴�� ������ŵ�ϴ�.
        if (isReturning)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * returnSpeed);

            // ���� ���� ũ��� ���ƿ����� ������ �ߴ��մϴ�.
            if (Vector3.Distance(transform.localScale, originalScale) < 0.01f)
            {
                transform.localScale = originalScale;
                isReturning = false;
            }
        }
    }

    /// <summary>
    /// ������ ������ �� ȣ���ϴ� �Լ�.
    /// </summary>
    public void StartCharge()
    {
        isReturning = false;
        // �ʿ��ϴٸ� ���⿡ ���� ���� ���峪 ��ƼŬ�� ���� �� �ֽ��ϴ�.
    }

    /// <summary>
    /// Ű�� ������ ���� �� ������ ȣ���ϸ� ���� ����(0.0 ~ 1.0)�� �����ϴ� �Լ�.
    /// </summary>
    /// <param name="chargeRatio">0.0 (����) ���� 1.0 (�ִ�) ������ ���� ����</param>
    public void UpdateCharge(float chargeRatio)
    {
        isReturning = false;
        // ���� ������ ���� ������(����) ������ ����մϴ�.
        float targetScaleX = Mathf.Lerp(originalScale.x, maxSqueezeScale.x, chargeRatio);
        float targetScaleY = Mathf.Lerp(originalScale.y, maxSqueezeScale.y, chargeRatio);

        transform.localScale = new Vector3(targetScaleX, targetScaleY, originalScale.z);
    }

    /// <summary>
    /// ������ �ߴ��ϰų� Ű���� ���� ���� �� ȣ���ϴ� �Լ�.
    /// </summary>
    public void ReleaseCharge()
    {
        isReturning = true;
        // �ʿ��ϴٸ� ���⿡ ���� �ߴ�/���� ���带 ���� �� �ֽ��ϴ�.
    }
}
