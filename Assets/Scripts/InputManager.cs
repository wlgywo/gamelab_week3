using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public bool isPlayerInputLocked = false; 
    public PlayerInput playerInput; // { get; private set; }
    public event Action OnFill;
    public event Action StopFill;
    public event Action PressA;
    public event Action PressD;
    public event Action ReleaseA;
    public event Action ReleaseD;
    public event Action TimingChecker;
    public event Action<float> MovementAD;
    public event Action<float> StopMovementAD;
    public event Action SpacePressed;
    public event Action SpaceReleased;
    public event Action HitClick;
    public event Action DoPlant;
    public event Action<int> ChangeModeType;

    [SerializeField] public bool isDragging = false; // 드래그 여부
    [SerializeField] public bool isClick = false; // 클릭 여부
    public Vector2 moveInput; // 대각선 무시 이동 방향
    private Camera mainCamera; // 메인 카메라
    public GameObject draggedObject; // 현재 드래그 중인 오브젝트
    public GameObject clickableObject; // 클릭 가능한 오브젝트
    public Rect mouseClampArea = new Rect(0, 0, 1920, 1080); // 게임 화면
    public LayerMask blockingMask;


    private void Awake()
    {
        playerInput = new PlayerInput(); 
        //TO-DO : 정상 작동 시 삭제
        mainCamera = Camera.main;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.Jump.started += SpaceStarted;
        playerInput.Player.Jump.performed += OnJmup;
        playerInput.Player.Jump.canceled += SpaceCanceled;
        playerInput.Player.Pause.performed += OnPause;
        playerInput.Player.PrimaryClick.started += StartDrag;
        playerInput.Player.PrimaryClick.canceled += EndDrag;
        playerInput.Player.PrimaryClick.performed += OnLeftClick;
        //playerInput.Player.HorizontalLeft.performed += OnPressA;
        //playerInput.Player.HorizontalRight.performed += OnPressD;
        //playerInput.Player.HorizontalLeft.canceled += OffPressA;
        //playerInput.Player.HorizontalRight.canceled += OffPressD;
        playerInput.Player.MovementHoriAxis.performed += OnHoriAxis;
        playerInput.Player.MovementHoriAxis.canceled += OnHoriAxis;
        playerInput.Player.Interaction.performed += OnInteract;
        playerInput.Player.ModeSwitch.performed += SwitchMode;
        playerInput.UI.SpaceContinue.performed += OnContinue;

    }

    private void OnDisable()
    {
        playerInput.Player.Jump.started -= SpaceStarted;
        playerInput.Player.Jump.performed -= OnJmup;
        playerInput.Player.Jump.canceled -= SpaceCanceled;
        playerInput.Player.Pause.performed -= OnPause; 
        playerInput.Player.PrimaryClick.started -= StartDrag;
        playerInput.Player.PrimaryClick.canceled -= EndDrag;
        //playerInput.Player.PrimaryClick.performed -= OnLeftClick;
        //playerInput.Player.HorizontalLeft.performed -= OnPressA;
        //playerInput.Player.HorizontalRight.performed -= OnPressD;
        //playerInput.Player.HorizontalLeft.canceled -= OffPressA;
        //playerInput.Player.HorizontalRight.canceled -= OffPressD;
        playerInput.Player.MovementHoriAxis.performed -= OnHoriAxis;
        playerInput.Player.MovementHoriAxis.canceled -= OnHoriAxis;
        playerInput.Player.Interaction.performed -= OnInteract;
        playerInput.Player.ModeSwitch.performed -= SwitchMode;
        playerInput.UI.SpaceContinue.performed -= OnContinue;

        playerInput.Player.Disable();
    }

    private void TestDisable()
    {
        playerInput.Player.PrimaryClick.started -= StartDrag;
        playerInput.Player.PrimaryClick.canceled -= EndDrag;
        playerInput.Player.PrimaryClick.performed -= OnLeftClick;
        //playerInput.Player.HorizontalLeft.performed -= OnPressA;
        //playerInput.Player.HorizontalRight.performed -= OnPressD;
        //playerInput.Player.HorizontalLeft.canceled -= OffPressA;
        //playerInput.Player.HorizontalRight.canceled -= OffPressD;
        playerInput.Player.MovementHoriAxis.performed -= OnHoriAxis;
        playerInput.Player.MovementHoriAxis.canceled -= OnHoriAxis;
        playerInput.Player.Jump.started -= SpaceStarted;
        playerInput.Player.Jump.performed -= OnJmup;
        playerInput.Player.Jump.canceled -= SpaceCanceled;
        
        // 2. Action 자체를 비활성화 (핵심!)
        playerInput.Player.PrimaryClick.Disable();
        playerInput.Player.HorizontalLeft.Disable();
        playerInput.Player.HorizontalRight.Disable();
        playerInput.Player.MovementHoriAxis.Disable();
        playerInput.Player.Jump.Disable();
        playerInput.Player.Move.Disable();
        
        // 3. 드래그 상태 초기화
        isDragging = false;
        draggedObject = null;
        isClick = false;
        clickableObject = null;

    }
    private void TestAble()
    {
        // 1. Action 다시 활성화
        playerInput.Player.PrimaryClick.Enable();
        playerInput.Player.HorizontalLeft.Enable();
        playerInput.Player.HorizontalRight.Enable();
        playerInput.Player.MovementHoriAxis.Enable();
        playerInput.Player.Jump.Enable();
        playerInput.Player.Move.Enable();
    
        // 2. 이벤트 다시 구독
        playerInput.Player.Jump.started += SpaceStarted;
        playerInput.Player.Jump.performed += OnJmup;
        playerInput.Player.Jump.canceled += SpaceCanceled;
        playerInput.Player.PrimaryClick.started += StartDrag;
        playerInput.Player.PrimaryClick.canceled += EndDrag;
        playerInput.Player.PrimaryClick.performed += OnLeftClick;
        //playerInput.Player.HorizontalLeft.performed += OnPressA;
        //playerInput.Player.HorizontalRight.performed += OnPressD;
        //playerInput.Player.HorizontalLeft.canceled += OffPressA;
        //playerInput.Player.HorizontalRight.canceled += OffPressD;
        playerInput.Player.MovementHoriAxis.performed += OnHoriAxis;
        playerInput.Player.MovementHoriAxis.canceled += OnHoriAxis;
    }
    //TO- DO  : DISABLE 도 통일
    private void OnDestroy()
    {
        DeleteEvents();
    }

    private void DeleteEvents()
    {
        playerInput.Player.Jump.performed -= OnJmup;
        playerInput.Player.Pause.performed -= OnPause; 
        // playerInput.Player.PrimaryClick.started -= StartDrag;
        playerInput.Player.PrimaryClick.canceled -= EndDrag;
        playerInput.Player.PrimaryClick.performed -= OnLeftClick;
        playerInput.Player.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (playerInput.Player.PrimaryClick.IsPressed())
            isDragging = true;
        else
            isDragging = false;*/
        // GetCurrentMousePosition();
        // GetMoveInput();
    }
    // 왼쪽 클릭 - 좌클릭 입력
    public void OnLeftClick(InputAction.CallbackContext context)
    {
        HitClick?.Invoke();
        Vector2 mouseScreenPos = playerInput.Player.PointerPosition.ReadValue<Vector2>();
        if (mainCamera == null)
            mainCamera = Camera.main;
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(mouseScreenPos), Vector2.zero);
        Debug.Log("Perform");
        
        if (hit.collider != null)
        {
           
        }
    }

    // 이동 입력
    public Vector2 GetMoveInput()
    {
        Vector2 moveDirection = playerInput.Player.Move.ReadValue<Vector2>();

        return moveDirection;
    }

    // 점프 - 스페이스
    public void OnJmup(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnFill?.Invoke();
            TimingChecker?.Invoke();
        }
    }

    // 일시정지 - ESC
    public void OnPause(InputAction.CallbackContext context)
    {/*
        if (context.performed)
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            if(Time.timeScale > 0)
            {
                TestAble();
            }
            else if(Time.timeScale <=0)
            {
                Cursor.visible = true;
                TestDisable();
            }
            Debug.Log("Pause");
        }*/
    }


    // 마우스 드래그 시작
    private void StartDrag(InputAction.CallbackContext context)
    {
        isDragging = true;
        Debug.Log("Start");
        Vector2 mouseScreenPos = playerInput.Player.PointerPosition.ReadValue<Vector2>();
        if (mainCamera == null)
            mainCamera = Camera.main;
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(mouseScreenPos), Vector2.zero);
        if (hit.collider != null)
        {
            
        } 
        
    }

    // 마우스 드래그 종료
    private void EndDrag(InputAction.CallbackContext context)
    {
        Debug.Log("End");
        isDragging = false;
        // 드래그 중인 오브젝트 해제
         draggedObject = null;
    }


    // 마우스 
    public Vector2 GetMousePointerWorldPos()
    {
        /*Vector2 mouseScreenPos = playerInput.Player.PointerPosition.ReadValue<Vector2>();
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        return mouseWorldPos;*/

        Vector2 mouseScreenPos = playerInput.Player.PointerPosition.ReadValue<Vector2>();
        mouseScreenPos.x = Mathf.Clamp(mouseScreenPos.x, mouseClampArea.xMin, mouseClampArea.xMax);
        mouseScreenPos.y = Mathf.Clamp(mouseScreenPos.y, mouseClampArea.yMin, mouseClampArea.yMax);
        Vector3 mouseWorldPosWithZ = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane));
        return mouseWorldPosWithZ;
    }
    private void SpaceStarted(InputAction.CallbackContext context)
    {
        SpacePressed?.Invoke();
        UIManager.Instance.canProceedToNextDay = true;
    }
    private void SpaceCanceled(InputAction.CallbackContext context)
    {
        StopFill?.Invoke();
        SpaceReleased?.Invoke();
    }

   /*private void OnPressA(InputAction.CallbackContext context)
   {
        Debug.Log("A Pressed");
        if (context.performed)
        {
            PressA?.Invoke();
        }
   }

    private void OnPressD(InputAction.CallbackContext context)
    {
        Debug.Log("D Pressed");
        if (context.performed)
        {
            PressD?.Invoke();
        }
    }

    private void OffPressA(InputAction.CallbackContext context)
    {
        Debug.Log("A Released");
        ReleaseA?.Invoke();
    }

    private void OffPressD(InputAction.CallbackContext context)
    {
        Debug.Log("D Released");
        ReleaseD?.Invoke();
    }*/

    private void OnHoriAxis(InputAction.CallbackContext context)
     {
        if(isPlayerInputLocked) return;
        float movement = context.ReadValue<float>();
        MovementAD?.Invoke(movement);
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact Pressed");

        DoPlant?.Invoke();
        
    }

    private void SwitchMode(InputAction.CallbackContext context)
    {
        string keyName = context.control.name;
        if (int.TryParse(keyName, out int numberValue))
        {
            ChangeModeType?.Invoke(numberValue);
        }
        else
        {
            switch (keyName)
            {
                case "f1":
                    numberValue = 11;
                    ChangeModeType?.Invoke(numberValue);
                    break;
                case "f2":
                    numberValue = 12;
                    ChangeModeType?.Invoke(numberValue);
                    break;
                default:
                    Debug.LogWarning($"처리할 수 없는 키 입력입니다: '{context.control.name}'");
                    break;
            }
        }
    }

    private void OnContinue(InputAction.CallbackContext context)
    {
       
            TimeManager.Instance.canProceedToNextDay = true;
        
    }
    //public void ClearAllInputs()
    //{
    //    MovementAD?.Invoke(0f);
    //}
}
