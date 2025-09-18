using UnityEngine;
using UnityEngine.InputSystem;
public enum FarmingState
{
    Dirt, 
    Seed,
    Water
}
public class ModeManager : MonoBehaviour
{
    public static ModeManager Instance { get; private set; }
    private int currentMode = 0;
    public FarmingState currentWork = FarmingState.Dirt;
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
            case 0:
                currentWork = FarmingState.Dirt;
                break;
            case 1:
                currentWork = FarmingState.Seed;
                break;
            case 2:
                currentWork = FarmingState.Water;
                break;
            default:
                currentWork = FarmingState.Dirt;
                break;
        }
    }
}
