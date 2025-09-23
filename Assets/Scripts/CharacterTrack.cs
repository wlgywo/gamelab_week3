using System.Collections.Generic;
using UnityEngine;

public class CharacterTrack : MonoBehaviour
{
    public static Queue<Vector3> pathPoints = new Queue<Vector3>();

    [Header("��� ����")]
    [Tooltip("��θ� �󸶳� ���ϰ� ������� (���� �������� �ε巯��)")]
    [SerializeField] private float recordDistance = 0.5f;

    [Tooltip("�ִ� �� ���� ��ġ�� ������� (�޸� ����)")]
    [SerializeField] private int maxPoints = 100;

    private Vector3 lastRecordedPosition;

    void Start()
    {
        // ���� ��ġ�� ���
        lastRecordedPosition = transform.position;
        pathPoints.Enqueue(lastRecordedPosition);
    }

    void Update()
    {
        // ���������� ��ϵ� ��ġ�κ��� ���� �Ÿ� �̻� �������� ���� ���ο� ��ġ�� ���
        if (Vector3.Distance(transform.position, lastRecordedPosition) > recordDistance)
        {
            lastRecordedPosition = transform.position;
            pathPoints.Enqueue(lastRecordedPosition); // ���� ��ġ�� Queue�� �߰� (Enqueue)

            // ��ΰ� �ʹ� ������� �ʵ��� �ִ� ������ ������ ���� ������ ��ġ���� ����
            if (pathPoints.Count > maxPoints)
            {
                pathPoints.Dequeue(); // ���� ���� �� ��ġ�� Queue���� ���� (Dequeue)
            }
        }
    }
}
