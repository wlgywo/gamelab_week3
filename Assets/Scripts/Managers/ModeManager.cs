using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public enum FarmingState
{
    Dirt, 
    Water,
    Harvest,
    PlantParsnip,
    PlantCarrot,
    PlantRadish,
    PlantPotato,
    PlantEggplant,
    PlantPumpkin,
    Fertilize, 
    Scarecrow,
    Talk, 
    Nutrition,
    Manure,
    Pesticide
}
public class ModeManager : MonoBehaviour
{
    /*public static ModeManager Instance { get; private set; }
    private int currentMode = 0;
    public FarmingState currentWork = FarmingState.Dirt;
    public event Action<int> ChangeModeText;
    // --- 추가된 변수들 ---
    // 마우스 휠로 순환시킬 농사 모드 목록
    private List<FarmingState> scrollableModes;
    // scrollableModes 리스트의 현재 인덱스
    private int currentModeIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

    }
    void Start()
    {
        scrollableModes = new List<FarmingState>
        {
            FarmingState.Dirt,
            FarmingState.Water,
            FarmingState.Harvest,
            FarmingState.Fertilize,
            FarmingState.PlantParsnip,
            FarmingState.PlantCarrot,
            FarmingState.PlantRadish,
            FarmingState.PlantPotato,
            FarmingState.PlantEggplant,
            FarmingState.PlantPumpkin,
            FarmingState.Scarecrow,
            FarmingState.Talk

        };
        InputManager.Instance.ChangeModeType += GetModeInput;
        InputManager.Instance.ChangeModeScroll += OnModeScroll;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.ChangeModeType -= GetModeInput;
            InputManager.Instance.ChangeModeScroll -= OnModeScroll;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetModeInput(int mode)
    {
        currentMode = mode;

        switch (currentMode)
        {
            case 1:
                currentWork = FarmingState.Dirt;
                break;
            case 2:
                currentWork = FarmingState.Water;
                break;
            case 3:
                currentWork = FarmingState.Harvest;
                break;
            case 4:
                currentWork = FarmingState.Fertilize;
                break;
            case 5:
                currentWork = FarmingState.PlantParsnip;
                break;
            case 6:
                currentWork = FarmingState. PlantCarrot;
                break;
            case 7:
                currentWork = FarmingState.PlantRadish;
                break;
            case 8:
                currentWork = FarmingState.PlantPotato;
                break;
            case 9:
                currentWork = FarmingState.PlantEggplant;
                break;
            case 0:
                currentWork = FarmingState.PlantPumpkin;
                break;
            case 11:
                currentWork = FarmingState.Scarecrow;
                break;
            case 12:
                currentWork = FarmingState.Talk;
                break;
            default:
                currentWork = FarmingState.Dirt;
                break;
        }

        ChangeModeText?.Invoke(currentMode);
    }

    private void OnModeScroll(Vector2 input)
    {
        Vector2 scrollValue = input;

        if (scrollValue.y > 0) // 휠 위로
        {
            currentModeIndex--;
            if (currentModeIndex < 0)
            {
                currentModeIndex = scrollableModes.Count - 1; // 마지막으로 순환
            }
        }
        else if (scrollValue.y < 0) // 휠 아래로
        {
            currentModeIndex++;
            if (currentModeIndex >= scrollableModes.Count)
            {
                currentModeIndex = 0; // 처음으로 순환
            }
        }

        // 현재 인덱스에 맞는 모드로 변경
        currentWork = scrollableModes[currentModeIndex];

        // UI 업데이트를 위해 현재 모드에 맞는 숫자 값을 찾아 전달
        ChangeModeText?.Invoke(currentMode);
    }*/
    public static ModeManager Instance { get; private set; }

    // private int currentMode = 0; // 더 이상 이 멤버 변수는 필요 없습니다.
    public FarmingState currentWork = FarmingState.Dirt;
    public event Action<int> ChangeModeText;

    private List<FarmingState> scrollableModes;
    private int currentModeIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        scrollableModes = new List<FarmingState>
        {
            FarmingState.Dirt,
            FarmingState.Water,
            FarmingState.Harvest,
            FarmingState.Fertilize,
            FarmingState.PlantParsnip,
            FarmingState.PlantCarrot,
            FarmingState.PlantRadish,
            FarmingState.PlantPotato,
            FarmingState.PlantEggplant,
            FarmingState.PlantPumpkin,
            FarmingState.Scarecrow,
            FarmingState.Talk,
            FarmingState.Nutrition,
            FarmingState.Manure,
            FarmingState.Pesticide
        };

        // InputManager 이벤트 구독
        if (InputManager.Instance != null)
        {
            InputManager.Instance.ChangeModeType += GetModeInput;
            InputManager.Instance.ChangeModeScroll += OnModeScroll;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.ChangeModeType -= GetModeInput;
            InputManager.Instance.ChangeModeScroll -= OnModeScroll;
        }
    }

    private void GetModeInput(int mode)
    {
        switch (mode)
        {
            case 1: currentWork = FarmingState.Dirt; break;
            case 2: currentWork = FarmingState.Water; break;
            case 3: currentWork = FarmingState.Harvest; break;
            case 4: currentWork = FarmingState.Fertilize; break;
            case 5: currentWork = FarmingState.PlantParsnip; break;
            case 6: currentWork = FarmingState.PlantCarrot; break;
            case 7: currentWork = FarmingState.PlantRadish; break;
            case 8: currentWork = FarmingState.PlantPotato; break;
            case 9: currentWork = FarmingState.PlantEggplant; break;
            case 10: currentWork = FarmingState.PlantPumpkin; break; // 0번 대신 10번 사용을 권장
            case 11: currentWork = FarmingState.Scarecrow; break;
            case 12: currentWork = FarmingState.Talk; break;
            case 13: currentWork = FarmingState.Pesticide; break;
            case 14: currentWork = FarmingState.Manure; break;
            case 15: currentWork = FarmingState.Nutrition; break;
            default: currentWork = FarmingState.Dirt; break;
        }

        // --- 수정된 부분 ---
        // 키보드 입력으로 모드가 바뀌었을 때, 스크롤 인덱스도 동기화
        currentModeIndex = scrollableModes.IndexOf(currentWork);

        // UI 업데이트 (입력받은 mode 값을 그대로 전달)
        ChangeModeText?.Invoke(mode);
    }

    // --- 여기서는 Vector2 대신 InputAction.CallbackContext를 받는 것이 더 표준적입니다 ---
    private void OnModeScroll(Vector2 context)
    {
        // Vector2 scrollValue = input; // 이전 코드
        Vector2 scrollValue = context;

        if (scrollValue.y > 0)
        {
            currentModeIndex--;
            if (currentModeIndex < 0)
            {
                currentModeIndex = scrollableModes.Count - 1;
            }
        }
        else if (scrollValue.y < 0)
        {
            currentModeIndex++;
            if (currentModeIndex >= scrollableModes.Count)
            {
                currentModeIndex = 0;
            }
        }

        currentWork = scrollableModes[currentModeIndex];

        // --- 수정된 부분 ---
        // 현재 FarmingState에 맞는 숫자 값을 찾아서 UI에 전달
        int modeForUI = GetModeIntFromState(currentWork);
        ChangeModeText?.Invoke(modeForUI);
    }

    // --- 새로 추가된 헬퍼(도우미) 메서드 ---
    // FarmingState를 UI에 표시할 int 값으로 변환
    private int GetModeIntFromState(FarmingState state)
    {
        switch (state)
        {
            case FarmingState.Dirt: return 1;
            case FarmingState.Water: return 2;
            case FarmingState.Harvest: return 3;
            case FarmingState.Fertilize: return 4;
            case FarmingState.PlantParsnip: return 5;
            case FarmingState.PlantCarrot: return 6;
            case FarmingState.PlantRadish: return 7;
            case FarmingState.PlantPotato: return 8;
            case FarmingState.PlantEggplant: return 9;
            case FarmingState.PlantPumpkin: return 10;
            case FarmingState.Scarecrow: return 11;
            case FarmingState.Talk: return 12;           
            case FarmingState.Nutrition: return 13;
            case FarmingState.Manure: return 14;
            case FarmingState.Pesticide: return 15;
            default: return 1; // 기본값
        }
    }
}
