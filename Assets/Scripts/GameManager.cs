using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int playerMoney = 0; 
    public int fertilizerCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 돈을 추가하는 함수
    public void AddMoney(int amount)
    {
        playerMoney += amount;
        UIManager.Instance.UpdateMoneyText();
    }
}