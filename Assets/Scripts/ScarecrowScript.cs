using System.Collections.Generic;
using UnityEngine;

public class ScarecrowScript : MonoBehaviour
{
    public static List<ScarecrowScript> AllScarecrows = new List<ScarecrowScript>();

    [Tooltip("정사각형 범위의 절반 크기입니다. 8x8을 원하면 4로 설정하세요.")]
    public float squareHalfSize = 4f;

    private void OnEnable()
    {
        if (!AllScarecrows.Contains(this))
        {
            AllScarecrows.Add(this);
        }
    }

    private void OnDisable()
    {
        AllScarecrows.Remove(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        // 2D이므로 z 크기는 0으로 설정합니다.
        Vector3 boxSize = new Vector3(squareHalfSize * 2, squareHalfSize * 2, 0);
        Gizmos.DrawWireCube(transform.position, boxSize);
    }

}
