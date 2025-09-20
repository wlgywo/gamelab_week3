using System;
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
    Fertilize
}
public class ModeManager : MonoBehaviour
{
    public static ModeManager Instance { get; private set; }
    private int currentMode = 0;
    public FarmingState currentWork = FarmingState.Dirt;
    public event Action<int> ChangeModeText;
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
        InputManager.Instance.ChangeModeType += GetModeInput;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.ChangeModeType -= GetModeInput;
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
            default:
                currentWork = FarmingState.Dirt;
                break;
        }

        ChangeModeText?.Invoke(currentMode);
    }
}
