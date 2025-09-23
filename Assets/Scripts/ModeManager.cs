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
    // --- �߰��� ������ ---
    // ���콺 �ٷ� ��ȯ��ų ��� ��� ���
    private List<FarmingState> scrollableModes;
    // scrollableModes ����Ʈ�� ���� �ε���
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

        if (scrollValue.y > 0) // �� ����
        {
            currentModeIndex--;
            if (currentModeIndex < 0)
            {
                currentModeIndex = scrollableModes.Count - 1; // ���������� ��ȯ
            }
        }
        else if (scrollValue.y < 0) // �� �Ʒ���
        {
            currentModeIndex++;
            if (currentModeIndex >= scrollableModes.Count)
            {
                currentModeIndex = 0; // ó������ ��ȯ
            }
        }

        // ���� �ε����� �´� ���� ����
        currentWork = scrollableModes[currentModeIndex];

        // UI ������Ʈ�� ���� ���� ��忡 �´� ���� ���� ã�� ����
        ChangeModeText?.Invoke(currentMode);
    }*/
    public static ModeManager Instance { get; private set; }

    // private int currentMode = 0; // �� �̻� �� ��� ������ �ʿ� �����ϴ�.
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

        // InputManager �̺�Ʈ ����
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
            case 10: currentWork = FarmingState.PlantPumpkin; break; // 0�� ��� 10�� ����� ����
            case 11: currentWork = FarmingState.Scarecrow; break;
            case 12: currentWork = FarmingState.Talk; break;
            case 13: currentWork = FarmingState.Pesticide; break;
            case 14: currentWork = FarmingState.Manure; break;
            case 15: currentWork = FarmingState.Nutrition; break;
            default: currentWork = FarmingState.Dirt; break;
        }

        // --- ������ �κ� ---
        // Ű���� �Է����� ��尡 �ٲ���� ��, ��ũ�� �ε����� ����ȭ
        currentModeIndex = scrollableModes.IndexOf(currentWork);

        // UI ������Ʈ (�Է¹��� mode ���� �״�� ����)
        ChangeModeText?.Invoke(mode);
    }

    // --- ���⼭�� Vector2 ��� InputAction.CallbackContext�� �޴� ���� �� ǥ�����Դϴ� ---
    private void OnModeScroll(Vector2 context)
    {
        // Vector2 scrollValue = input; // ���� �ڵ�
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

        // --- ������ �κ� ---
        // ���� FarmingState�� �´� ���� ���� ã�Ƽ� UI�� ����
        int modeForUI = GetModeIntFromState(currentWork);
        ChangeModeText?.Invoke(modeForUI);
    }

    // --- ���� �߰��� ����(�����) �޼��� ---
    // FarmingState�� UI�� ǥ���� int ������ ��ȯ
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
            default: return 1; // �⺻��
        }
    }
}
