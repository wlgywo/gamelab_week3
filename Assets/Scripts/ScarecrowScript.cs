using System.Collections.Generic;
using UnityEngine;

public class ScarecrowScript : MonoBehaviour
{
    public static List<ScarecrowScript> AllScarecrows = new List<ScarecrowScript>();

    [Tooltip("���簢�� ������ ���� ũ���Դϴ�. 8x8�� ���ϸ� 4�� �����ϼ���.")]
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
        // 2D�̹Ƿ� z ũ��� 0���� �����մϴ�.
        Vector3 boxSize = new Vector3(squareHalfSize * 2, squareHalfSize * 2, 0);
        Gizmos.DrawWireCube(transform.position, boxSize);
    }

}
