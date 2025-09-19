using System.Collections.Generic;
using UnityEngine;

public class RavenSpanwer : MonoBehaviour
{
    public GameObject objectToSpawn;
    public string spawnPointTag = "Crop";

    void Start()
    {
        TimeManager.Instance.OnDayStart += SpawnObjectAtRandomPoint;
    }

    void OnDisable()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnDayStart -= SpawnObjectAtRandomPoint;
        }
    }

    public void SpawnObjectAtRandomPoint(int newDay)
    {
        if (objectToSpawn == null)
        {
            return;
        }

        GameObject[] allSpawnPoints = GameObject.FindGameObjectsWithTag(spawnPointTag);

        List<GameObject> validSpawnPoints = new List<GameObject>();

        foreach (GameObject point in allSpawnPoints)
        {
            CropBehaviour crop = point.GetComponent<CropBehaviour>();

            if (crop != null && !crop.isEaten && IsPositionSafeToSpawn(point.transform.position))
            {
                validSpawnPoints.Add(point);
            }
        }
        if (validSpawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, validSpawnPoints.Count);
            GameObject chosenPoint = validSpawnPoints[randomIndex];

            Instantiate(objectToSpawn, chosenPoint.transform.position, Quaternion.identity);
            chosenPoint.GetComponent<CropBehaviour>().Harmed();

            Debug.Log(chosenPoint.name + " 위치에 까마귀를 스폰했습니다.");
        }
        else
        {
        }
    }

    private bool IsPositionSafeToSpawn(Vector2 position)
    {
        foreach (ScarecrowScript scarecrow in ScarecrowScript.AllScarecrows)
        {
            Vector2 scarecrowPosition = scarecrow.transform.position;

            float distanceX = Mathf.Abs(position.x - scarecrowPosition.x);
            float distanceY = Mathf.Abs(position.y - scarecrowPosition.y);

            if (distanceX < scarecrow.squareHalfSize && distanceY < scarecrow.squareHalfSize)
            {
                return false;
            }
        }
        return true; 
    }
}
