using UnityEngine;

public class CharacterGroundCheck : MonoBehaviour
{

    //private bool onGround;

    //[Header("Ground Collider")]
    //[SerializeField] private float groundLength = 1.0f;
    //[SerializeField] private Vector3 colliderOffset;

    //[Header("Layer Masks ")]
    //[SerializeField] private LayerMask groundLayer;
    //// Start is called once before the first execution of Update after the MonoBehaviour is created

    //// Update is called once per frame
    //void Update()
    //{
    //    onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
    //}

    //public bool GetOnGround() { return onGround; }

    [SerializeField] private bool onGround;
    [SerializeField] private bool onWater;
    public float WaterOffset = 0f;

    [Header("Ground Collider")]
    [SerializeField] private float groundLength = 1.0f;
    [SerializeField] private Vector3 colliderOffset;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer;

    void Update()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        onGround = hit1.collider != null || hit2.collider != null;


        Vector2 checkPoint = new Vector2(transform.position.x, transform.position.y - WaterOffset);

        Collider2D waterCollider = Physics2D.OverlapPoint(checkPoint);

        onWater = waterCollider != null && waterCollider.CompareTag("Pond");
        if (onWater)
        {
            UIManager.Instance.ShowWaterCanFillTextUI();
        }
        else
        {
            UIManager.Instance.HideWaterCanFillTextUI();
        }
    }

    public bool GetOnGround() { return onGround; }
    public bool GetOnWater() { return onWater; }
}
