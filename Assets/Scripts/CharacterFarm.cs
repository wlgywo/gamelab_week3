using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
public class CharacterFarm : MonoBehaviour
{
    /*public float interactionRange = 1.5f;
    public LayerMask plantableTileLayer;
    public LayerMask canHarvestLayer;

    [SerializeField] float tiltAmount = 15f;
    [SerializeField] float seconds = 1f;
    [SerializeField] public ParticleSystem waterEffect;
    [SerializeField] GameObject characterSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.Instance.DoPlant += DoWater;
        InputManager.Instance.DoPlant += Harvest;
    }

    void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.DoPlant -= DoWater;
            InputManager.Instance.DoPlant -= Harvest;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoWater()
    {
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

        if (closestTile != null&& closestTile.CompareTag("Tile")&& ModeManager.Instance.currentWork == FarmingState.Water)
        {
            Material originMaterial = closestTile.GetComponent<SpriteRenderer>().material;
            closestTile.GetComponent<TilePrefabs>().GetWetByPlayer();
            waterEffect.Play();  
        }
        // InputManager.Instance.ClearAllInputs();
    }

    public void Harvest()
    {
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
        if (closestTile == null)
            return;
        if (closestTile.CompareTag("Tile") && ModeManager.Instance.currentWork == FarmingState.Harvest)
        {
            if (closestTile.GetComponentInChildren<CropBehaviour>() == null)
                return;

            if (closestTile.GetComponentInChildren<CropBehaviour>().isEaten)
            {
                foreach (Transform child in closestTile.gameObject.transform)
                {
                    Destroy(child.gameObject);
                }
                closestTile.GetComponent<TilePrefabs>().isDirtSpawned = false;
                closestTile.GetComponent<TilePrefabs>().isSeedSpawned = false;
            }

            if (!closestTile.GetComponentInChildren<CropBehaviour>().IsFullyGrown())
                return;
            CropManager.Instance.harvestAmountParsnip++;
            UIManager.Instance.UpdateHarvestText();
            foreach (Transform child in closestTile.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
            closestTile.GetComponent<TilePrefabs>().isDirtSpawned = false;
            closestTile.GetComponent<TilePrefabs>().isSeedSpawned = false;
        }
    }*/

    [Header("상호작용 설정")]
    public float interactionRange = 1.5f;
    public LayerMask plantableTileLayer;

    [Header("프리팹 및 이펙트")]
    public GameObject dirtPrefabs; // SpawnDirt에서 가져온 변수
    public GameObject seedPrefabs; // SpawnDirt에서 가져온 변수
    public float dirtSpawnHeight = 0.5f;
    public float seedSpawnHeight = 1.0f;
    public ParticleSystem waterEffect;

    [Header("오브젝트 레퍼런스")]
    [SerializeField] GameObject characterSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.Instance.DoPlant += PerformFarmingAction;
    }

    void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.DoPlant -= PerformFarmingAction;
        }
    }
    private void PerformFarmingAction()
    {
        Collider2D closestTile = GetClosestTile();
        if (closestTile == null || !closestTile.CompareTag("Tile")) return;

        switch (ModeManager.Instance.currentWork)
        {
            case FarmingState.Dirt: 
                DoPlow(closestTile);
                break;
            case FarmingState.Plant:
                DoPlantSeed(closestTile);
                break;
            case FarmingState.Water:
                DoWater(closestTile);
                break;
            case FarmingState.Harvest:
                Harvest(closestTile);
                break;
        }
    }

    public void DoPlow(Collider2D closestTile)
    {
        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();
        if (tileInfo.isDirtSpawned) return; 

        Vector3 spawnPosition = closestTile.transform.position;
        Instantiate(dirtPrefabs, spawnPosition + Vector3.up * dirtSpawnHeight, Quaternion.identity, closestTile.transform);
        closestTile.GetComponent<TilePrefabs>().isDirtSpawned = true;
        tileInfo.isDirtSpawned = true;
        Debug.Log("땅을 갈았습니다.");
    }

    public void DoPlantSeed(Collider2D closestTile)
    {
        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();

        if (!tileInfo.isDirtSpawned || tileInfo.isSeedSpawned) return;

        if (CropManager.Instance.seedParsnip <= 0)
        {
            Debug.Log("파스닙 씨앗이 없습니다!");
            return;
        }

        Vector3 spawnPosition = closestTile.transform.position;
        Instantiate(seedPrefabs, spawnPosition + Vector3.up * seedSpawnHeight, Quaternion.identity, closestTile.transform);
        tileInfo.isSeedSpawned = true;
        
        CropManager.Instance.seedParsnip--;
        UIManager.Instance.UpdateInventoryText();
        Debug.Log($"파스닙 씨앗을 심었습니다. 남은 씨앗: {CropManager.Instance.seedParsnip}개");
    }

    public void DoWater(Collider2D closestTile)
    {
        closestTile.GetComponent<TilePrefabs>().GetWetByPlayer();
        waterEffect.Play();
    }

    public void Harvest(Collider2D closestTile)
    {
        CropBehaviour crop = closestTile.GetComponentInChildren<CropBehaviour>();
        if (crop == null) return;

        if (closestTile.GetComponentInChildren<CropBehaviour>().isEaten)
        {
            foreach (Transform child in closestTile.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
            closestTile.GetComponent<TilePrefabs>().isDirtSpawned = false;
            closestTile.GetComponent<TilePrefabs>().isSeedSpawned = false;
        }

        if (crop==null || !crop.IsFullyGrown()) return;

        CropManager.Instance.harvestAmountParsnip++;
        UIManager.Instance.UpdateHarvestText();

        foreach (Transform child in closestTile.transform)
        {
            Destroy(child.gameObject);
        }

        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();
        tileInfo.isDirtSpawned = false;
        tileInfo.isSeedSpawned = false;
    }

    private Collider2D GetClosestTile()
    {
        Collider2D[] nearbyTiles = Physics2D.OverlapCircleAll(transform.position, interactionRange, plantableTileLayer);
        if (nearbyTiles.Length == 0) return null;

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
        return closestTile;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
