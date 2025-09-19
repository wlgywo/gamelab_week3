using UnityEngine;

public class CropManager : MonoBehaviour
{
    public static CropManager Instance { get; private set; }
    public int harvestAmountParsnip = 0;
    public int seedParsnip = 0;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
