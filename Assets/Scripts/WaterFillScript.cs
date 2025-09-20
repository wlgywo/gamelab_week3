using UnityEngine;

public class WaterFillScript : MonoBehaviour
{
    public float maxWaterAmount = 100f;
    public float currentWaterAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentWaterAmount = maxWaterAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
