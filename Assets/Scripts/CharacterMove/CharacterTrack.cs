using System.Collections.Generic;
using UnityEngine;

public class CharacterTrack : MonoBehaviour
{
    public static Queue<Vector3> pathPoints = new Queue<Vector3>();

    [Header("경로 설정")]
    [Tooltip("경로를 얼마나 상세하게 기록할지 (값이 작을수록 부드러움)")]
    [SerializeField] private float recordDistance = 0.5f;

    [Tooltip("최대 몇 개의 위치를 기억할지 (메모리 관리)")]
    [SerializeField] private int maxPoints = 100;

    private Vector3 lastRecordedPosition;

    void Start()
    {
        // 시작 위치를 기록
        lastRecordedPosition = transform.position;
        pathPoints.Enqueue(lastRecordedPosition);
    }

    void Update()
    {
        // 마지막으로 기록된 위치로부터 일정 거리 이상 움직였을 때만 새로운 위치를 기록
        if (Vector3.Distance(transform.position, lastRecordedPosition) > recordDistance)
        {
            lastRecordedPosition = transform.position;
            pathPoints.Enqueue(lastRecordedPosition); // 현재 위치를 Queue에 추가 (Enqueue)

            // 경로가 너무 길어지지 않도록 최대 개수를 넘으면 가장 오래된 위치부터 삭제
            if (pathPoints.Count > maxPoints)
            {
                pathPoints.Dequeue(); // 가장 먼저 들어간 위치를 Queue에서 제거 (Dequeue)
            }
        }
    }
}
