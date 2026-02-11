using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CharacterJump : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    private CharacterGroundCheck ground;
    public Vector2 velocity;
    private CharacterJuice juice;
    private CharacterMove move;

    [Header("Jumping Stats")]
    public float jumpHeight = 7.3f;
    public float timeToJumpApex;
    public float upwardMovementMultiplier = 1f;
    public float downwardMovementMultiplier = 6f;
    public int maxAirJumps = 0;

    [Header("Options")]
    public bool variablejumpHeight;
    public float jumpCutOff;
    public float speedLimit;
    public float coyoteTime = 0.15f;
    public float jumpBuffer = 0.15f;

    [Header("Calculations")]
    public float jumpSpeed;
    private float defaultGravityScale;
    public float gravMultiplier;

    [Header("Current State")]
    public bool canJumpAgain = false;
    private bool desiredJump;
    private float jumpBufferCounter;
    private float coyoteTimeCounter = 0;
    private bool pressingJump;
    public bool onGround;
    private bool currentlyJumping;

    [Header("Platform Drop Down")]
    private Collider2D currentPlatform = null; // 현재 밟고 있는 발판의 콜라이더
    private bool isJumpingDown = false; // 아래로 점프 중인지 확인

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<CharacterGroundCheck>();
        juice = GetComponent<CharacterJuice>();
        move = GetComponent<CharacterMove>();
        defaultGravityScale = 1f;
    }

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.SpacePressed += OnJump;
            InputManager.Instance.SpaceReleased += OffJump;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.SpacePressed -= OnJump;
            InputManager.Instance.SpaceReleased -= OffJump;
        }
    }

    public void OnJump()
    {
        Debug.Log("스페이스 누르기 시작");
        if (MovementLimiter.Instance.CharacterCanMove)
        {
            desiredJump = true;
            pressingJump = true;
        }
    }

    public void OffJump()
    {
        Debug.Log("스페이스 누르기 끝");
        if (MovementLimiter.Instance.CharacterCanMove)
        {
            pressingJump = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        setPhysics();
        onGround = ground.GetOnGround();

        // 아래 방향 키를 눌렀을 때
        // GetKey"Down"은 키를 누르는 첫 프레임만 감지합니다.
        // Input System을 사용한다면 해당 액션에 연결된 메서드에서 호출합니다.
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentPlatform != null)
            {
                StartCoroutine(DisablePlatformCollision());
            }
        }

        // 널널한 점프
        if (jumpBuffer > 0)
        {
            if (desiredJump)
            {
                jumpBufferCounter += Time.deltaTime;

                if (jumpBufferCounter > jumpBuffer)
                {
                    desiredJump = false;
                    jumpBufferCounter = 0;
                }
            }
        }
        // 코요테
        if (!currentlyJumping && !onGround)
        {
            coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            coyoteTimeCounter = 0;
        }
    }


    private void setPhysics()
    {
        Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));
        rb.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier;
    }

    private void FixedUpdate()
    {
        velocity = rb.linearVelocity;

        if (desiredJump)
        {
            DoAJump();
            rb.linearVelocity = velocity;
            return;
        }

        calculateGravity();
    }

    private void calculateGravity()
    {

        if (rb.linearVelocity.y > 0.01f)
        {
            if (onGround)
            {
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                if (variablejumpHeight)
                {
                    if (pressingJump && currentlyJumping)
                    {
                        gravMultiplier = upwardMovementMultiplier;
                    }
                    else
                    {
                        gravMultiplier = jumpCutOff;
                    }
                }
                else
                {
                    gravMultiplier = upwardMovementMultiplier;
                }
            }
        }
        else if (rb.linearVelocity.y < -0.01f)
        {

            if (onGround)
            {
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                gravMultiplier = downwardMovementMultiplier;
            }

        }
        else
        {
            if (onGround)
            {
                currentlyJumping = false;
                canJumpAgain = false;
            }

            gravMultiplier = defaultGravityScale;
        }
        rb.linearVelocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100));
    }

    public void ResetJumpState()
    {
        canJumpAgain = false;
        currentlyJumping = false;
        desiredJump = false;
        pressingJump = false;
        jumpBufferCounter = 0;
        coyoteTimeCounter = 0;
    }

    private void DoAJump()
    {
        if (onGround || (coyoteTimeCounter > 0.03f && coyoteTimeCounter < coyoteTime) || canJumpAgain)
        {
            desiredJump = false;
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            canJumpAgain = (maxAirJumps == 1 && canJumpAgain == false);

            //Determine the power of the jump, based on our gravity and stats
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * rb.gravityScale * jumpHeight);

            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(rb.linearVelocity.y);
            }

            velocity.y += jumpSpeed;
            currentlyJumping = true;

            if (juice != null)
            {
                juice.jumpEffects();
            }
        }

        if (jumpBuffer == 0)
        {
            desiredJump = false;
        }
    }

    public void bounceUp(float bounceAmount)
    {
        rb.AddForce(Vector2.up * bounceAmount, ForceMode2D.Impulse);
    }


    // 낮점 로직
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트에 Platform Effector 2D가 있는지 확인
        if (collision.gameObject.GetComponent<PlatformEffector2D>() != null)
        {
            // 현재 밟고 있는 발판으로 설정
            currentPlatform = collision.collider;
        }
    }

    // 발판에서 떨어졌을 때 호출
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 떨어지려는 발판이 현재 밟고 있는 발판 정보와 일치하면 초기화
        if (collision.collider == currentPlatform)
        {
            currentPlatform = null;
        }
    }

    // 발판 콜라이더를 잠시 끄는 코루틴
    private IEnumerator DisablePlatformCollision()
    {
        // 아래로 점프하는 동안에는 발판을 감지하지 못하도록 함
        Collider2D platformToFallThrough = currentPlatform;
        isJumpingDown = true;

        // 발판의 콜라이더를 비활성화
        platformToFallThrough.enabled = false;

        // 아주 짧은 시간(0.3초) 동안 기다립니다. 플레이어가 발판을 통과하기에 충분한 시간입니다.
        yield return new WaitForSeconds(0.3f);

        // 다시 콜라이더를 활성화해서 다른 오브젝트들이나 플레이어가 다시 밟을 수 있게 합니다.
        platformToFallThrough.enabled = true;
        isJumpingDown = false;
    }
}

