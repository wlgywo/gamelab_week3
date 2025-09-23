using UnityEngine;

public class OneRoomScript : MonoBehaviour
{
    [SerializeField] private bool isPlayerInRange = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.DoPlant += TryGoBed;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.DoPlant -= TryGoBed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryGoBed()
    {
        if (isPlayerInRange)
        {
            UIManager.Instance.ToggleOneRoomUI();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            // UIManager.Instance가 null이 아니고, oneRoomNoticeText도 null이 아닐 때만 실행
            if (UIManager.Instance != null && UIManager.Instance.oneRoomNoticeText != null)
            {
                UIManager.Instance.ShowOneRoomNoticeText();
                isPlayerInRange = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            UIManager.Instance.HideOneRoomNoticeText();
            if(UIManager.Instance != null && UIManager.Instance.oneRoomUI != null && UIManager.Instance.oneRoomUI.activeSelf)
                if (UIManager.Instance.oneRoomUI.activeSelf)
                {
                    UIManager.Instance.oneRoomUI.SetActive(false);
                }
            isPlayerInRange = false;
        }
    }

    public void DontSleeping()
    {
        UIManager.Instance.ToggleOneRoomUI();
    }
    public void Sleeping()
    {
        StartCoroutine(TimeManager.Instance.DayTransitionSequence());
    }
}
