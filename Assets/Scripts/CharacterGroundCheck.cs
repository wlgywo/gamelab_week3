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

    private bool onGround;
    private bool onWater;

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

        onWater = (hit1.collider != null && hit1.collider.CompareTag("Pond")) ||
                  (hit2.collider != null && hit2.collider.CompareTag("Pond"));
    }

    public bool GetOnGround() { return onGround; }
    public bool GetOnWater() { return onWater; }
}
