using UnityEngine;

public class SpawnDirt : MonoBehaviour
{

    public GameObject dirtPrefabs;
    public GameObject seedPrefabs;
    public float dirtSpawnHeight = 0.5f;
    public float seedSpawnHeight = 1.0f;
    public float interactionRange = 1.5f;
    public LayerMask plantableTileLayer;

    void Start()
    {
        //InputManager.Instance.TileClick += SpawningDirt;
        InputManager.Instance.DoPlant += DoAnything;
    }

    void OnDisable()
    {
        /*
        if (InputManager.Instance != null)
        {
            InputManager.Instance.TileClick -= SpawningDirt;
        }*/
        if (InputManager.Instance != null)
        {
            InputManager.Instance.DoPlant -= DoAnything;
        }
    }

    void Update()
    {
    }

    void PlantOnTile(GameObject targetTile)
    {

        if (targetTile == null) return;

        TilePrefabs tileInfo = targetTile.GetComponent<TilePrefabs>();
        if (tileInfo == null) return;

        Vector3 spawnPosition = targetTile.transform.position;
        if (!targetTile.GetComponent<TilePrefabs>().isSeedSpawned && targetTile.GetComponent<TilePrefabs>().isDirtSpawned)
        {
            Instantiate(seedPrefabs, spawnPosition + Vector3.up * seedSpawnHeight, Quaternion.identity);
            targetTile.GetComponent<TilePrefabs>().isSeedSpawned = true;
        }

        if (!targetTile.GetComponent<TilePrefabs>().isDirtSpawned)
        {
            Instantiate(dirtPrefabs, spawnPosition + Vector3.up * dirtSpawnHeight, Quaternion.identity);
            targetTile.GetComponent<TilePrefabs>().isDirtSpawned = true;
        }
        
    }
    public void SpawningDirt(RaycastHit2D hitInfo)
    {
        // hitInfo에서 GameObject를 추출한 뒤, 핵심 로직 함수를 호출합니다.
        GameObject currentTile = hitInfo.collider.gameObject;
        PlantOnTile(currentTile);
    }

    public void SpawningDirt(Collider2D currentTile)
    {
        PlantOnTile(currentTile.gameObject);
    }

    private void DoAnything()
    {
        Debug.Log("DoAnything 호출됨");
        Collider2D[] nearbyTiles = Physics2D.OverlapCircleAll(transform.position, interactionRange, plantableTileLayer);

        if (nearbyTiles.Length == 0) return;
        Collider2D closestTile = null;
        float minDistance = float.MaxValue;

        foreach (var tileCollider in nearbyTiles)
        {
            float distance = Vector3.Distance(transform.position, tileCollider.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTile = tileCollider;
            }
        }

        if (closestTile != null)
        {
            if (closestTile.CompareTag("Tile"))
            {
                PlantOnTile(closestTile.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
