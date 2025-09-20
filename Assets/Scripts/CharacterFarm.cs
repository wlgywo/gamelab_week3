using System.Collections.Generic;
using UnityEngine;
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
    public GameObject parsnipPrefabs;
    public GameObject carrotPrefabs; // SpawnDirt에서 가져온 변수
    public GameObject radishPrefabs;
    public GameObject potatoPrefabs;
    public GameObject eggplantPrefabs;
    public GameObject pumpkinPrefabs;
    public float dirtSpawnHeight = 0.5f;
    public float seedSpawnHeight = 1.0f;
    public ParticleSystem waterEffect;
    WaterFillScript waterFill;
    CharacterGroundCheck groundCheck;

    [Header("오브젝트 레퍼런스")]
    [SerializeField] GameObject characterSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.Instance.DoPlant += PerformFarmingAction;
        waterFill = gameObject.GetComponent<WaterFillScript>();
        groundCheck = gameObject.GetComponent<CharacterGroundCheck>();
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
            case FarmingState.PlantParsnip:
                DoPlantParsnip(closestTile);
                break;
            case FarmingState.PlantCarrot:
                DoPlantCarrot(closestTile);
                break;
            case FarmingState.PlantRadish:
                DoPlantRadish(closestTile);
                break;
            case FarmingState.PlantPotato:
                DoPlantPotato(closestTile);
                break;
            case FarmingState.PlantEggplant:
                DoPlantEggplant(closestTile);
                break;
            case FarmingState.PlantPumpkin:
                DoPlantPumpkin(closestTile);
                break;
            case FarmingState.Water:
                DoWater(closestTile);
                break;
            case FarmingState.Harvest:
                Harvest(closestTile);
                break;
            case FarmingState.Fertilize:
                Fertilize(closestTile);
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

    public void DoPlantParsnip(Collider2D closestTile)
    {
        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();

        if (!tileInfo.isDirtSpawned || tileInfo.isSeedSpawned) return;

        if (CropManager.Instance.seedParsnip <= 0)
        {
            return;
        }

        Vector3 spawnPosition = closestTile.transform.position;
        Instantiate(parsnipPrefabs, spawnPosition + Vector3.up * seedSpawnHeight, Quaternion.identity, closestTile.transform);
        tileInfo.isSeedSpawned = true;
        
        CropManager.Instance.seedParsnip--;
        UIManager.Instance.UpdateInventoryText();
    }

    public void DoPlantCarrot(Collider2D closestTile)
    {
        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();

        if (!tileInfo.isDirtSpawned || tileInfo.isSeedSpawned) return;

        if (CropManager.Instance.seedCarrot <= 0)
        {
            return;
        }

        Vector3 spawnPosition = closestTile.transform.position;
        Instantiate(carrotPrefabs, spawnPosition + Vector3.up * seedSpawnHeight, Quaternion.identity, closestTile.transform);
        tileInfo.isSeedSpawned = true;

        CropManager.Instance.seedCarrot--;
        UIManager.Instance.UpdateInventoryText();
    }

    public void DoPlantRadish(Collider2D closestTile)
    {
        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();
        if (!tileInfo.isDirtSpawned || tileInfo.isSeedSpawned) return;
        if (CropManager.Instance.seedRadish <= 0)
        {
            return;
        }
        Vector3 spawnPosition = closestTile.transform.position;
        Instantiate(radishPrefabs, spawnPosition + Vector3.up * seedSpawnHeight, Quaternion.identity, closestTile.transform);
        tileInfo.isSeedSpawned = true;
        CropManager.Instance.seedRadish--;
        UIManager.Instance.UpdateInventoryText();
    }

    public void DoPlantPotato(Collider2D closestTile)
    {
        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();
        if (!tileInfo.isDirtSpawned || tileInfo.isSeedSpawned) return;
        if (CropManager.Instance.seedPotato <= 0)
        {
            return;
        }
        Vector3 spawnPosition = closestTile.transform.position;
        Instantiate(potatoPrefabs, spawnPosition + Vector3.up * seedSpawnHeight, Quaternion.identity, closestTile.transform);
        tileInfo.isSeedSpawned = true;
        CropManager.Instance.seedPotato--;
        UIManager.Instance.UpdateInventoryText();
    }

    public void DoPlantEggplant(Collider2D closestTile)
    {
        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();
        if (!tileInfo.isDirtSpawned || tileInfo.isSeedSpawned) return;
        if (CropManager.Instance.seedEggplant <= 0)
        {
            return;
        }
        Vector3 spawnPosition = closestTile.transform.position;
        Instantiate(eggplantPrefabs, spawnPosition + Vector3.up * seedSpawnHeight, Quaternion.identity, closestTile.transform);
        tileInfo.isSeedSpawned = true;
        CropManager.Instance.seedEggplant--;
        UIManager.Instance.UpdateInventoryText();
    }

    public void DoPlantPumpkin(Collider2D closestTile)
    {
        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();
        if (!tileInfo.isDirtSpawned || tileInfo.isSeedSpawned) return;
        if (CropManager.Instance.seedPumpkin <= 0)
        {
            return;
        }
        Vector3 spawnPosition = closestTile.transform.position;
        Instantiate(pumpkinPrefabs, spawnPosition + Vector3.up * seedSpawnHeight, Quaternion.identity, closestTile.transform);
        tileInfo.isSeedSpawned = true;
        CropManager.Instance.seedPumpkin--;
        UIManager.Instance.UpdateInventoryText();
    }

    public void DoWater(Collider2D closestTile)
    {
        //closestTile.GetComponent<TilePrefabs>().GetWetByPlayer();
        //waterEffect.Play();
        //UIManager.Instance.UpdateWaterText(waterFill.currentWaterAmount);

        if (groundCheck.GetOnWater() && waterFill.currentWaterAmount < waterFill.maxWaterAmount)
        {
            waterFill.currentWaterAmount = waterFill.maxWaterAmount;
            UIManager.Instance.UpdateWaterText(waterFill.currentWaterAmount);
            return; 
        }

        if (waterFill.currentWaterAmount <= 0)
        {
            waterFill.currentWaterAmount = waterFill.maxWaterAmount;
            UIManager.Instance.UpdateWaterText(waterFill.currentWaterAmount);
            return;
        }

        if (closestTile == null) return;

        closestTile.GetComponent<TilePrefabs>().GetWetByPlayer();
        waterEffect.Play();
        waterFill.currentWaterAmount -= 5;

        UIManager.Instance.UpdateWaterText(waterFill.currentWaterAmount);
    }

    /*public void Harvest(Collider2D closestTile)
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

        if( crop.cropData.cropName == "Parsnip"&&!closestTile.GetComponent<TilePrefabs>().isFertilized) 
            CropManager.Instance.harvestAmountParsnip++;

        else if (crop.cropData.cropName == "Parsnip" && closestTile.GetComponent<TilePrefabs>().isFertilized)
        {
            int randomHarvestAmount = Random.Range(1, 5);
            CropManager.Instance.harvestAmountParsnip += randomHarvestAmount;
        }

        if (crop.cropData.cropName == "GiantParsnip")
        {
            // 3. 거대 작물이라면, 왼쪽과 오른쪽 타일을 찾습니다.
            // 왼쪽 타일 찾기
            Vector2 leftPos = (Vector2)closestTile.transform.position + Vector2.left;
            Collider2D leftTileCollider = Physics2D.OverlapPoint(leftPos, plantableTileLayer);
            if (leftTileCollider != null)
            {
                leftTileCollider.GetComponent<TilePrefabs>().isOccupiedByGiantCrop = false;
                leftTileCollider.GetComponent<TilePrefabs>().isSeedSpawned = false;
                leftTileCollider.GetComponent<TilePrefabs>().RemoveFertilizer();
            }

            Vector2 rightPos = (Vector2)closestTile.transform.position + Vector2.right;
            Collider2D rightTileCollider = Physics2D.OverlapPoint(rightPos, plantableTileLayer);
            if (rightTileCollider != null)
            {
                rightTileCollider.GetComponent<TilePrefabs>().isOccupiedByGiantCrop = false;
                rightTileCollider.GetComponent<TilePrefabs>().isSeedSpawned = false;
                rightTileCollider.GetComponent<TilePrefabs>().RemoveFertilizer();
            }

            // 수확량 증가
            CropManager.Instance.harvestAmountParsnip += 10;
        }
        UIManager.Instance.UpdateHarvestText();

        foreach (Transform child in closestTile.transform)
        {
            Destroy(child.gameObject);
        }

        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();
        tileInfo.isOccupiedByGiantCrop = false;
        tileInfo.isDirtSpawned = false;
        tileInfo.isSeedSpawned = false;
        tileInfo.RemoveFertilizer();
    }*/

    public void Harvest(Collider2D closestTile)
    {
        CropBehaviour crop = closestTile.GetComponentInChildren<CropBehaviour>();
        if (crop == null) return;

        if (crop.isEaten)
        {
            foreach (Transform child in closestTile.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
            closestTile.GetComponent<TilePrefabs>().isDirtSpawned = false;
            closestTile.GetComponent<TilePrefabs>().isSeedSpawned = false;
        }

        if(!crop.IsFullyGrown()) return;

        Seed cropData = crop.cropData;
        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();

        int amountToHarvest = 0;

        if (cropData.isGiantCrop)
        {
            amountToHarvest = cropData.giantCropYield;
        }
        else
        {
            if (tileInfo.isFertilized)
            {
                amountToHarvest = Random.Range(cropData.minFertilizedYield, cropData.maxFertilizedYield + 1);
            }
            else
            {
                amountToHarvest = Random.Range(cropData.minYield, cropData.maxYield + 1);
            }
        }

        if (amountToHarvest > 0)
       {
            CropManager.Instance.AddHarvestedItem(cropData.harvestedItemID, amountToHarvest);
        }

        List<TilePrefabs> tilesToReset = new List<TilePrefabs> { tileInfo };

        if (cropData.isGiantCrop)
        {
            Vector2 leftPos = (Vector2)closestTile.transform.position + Vector2.left;
            Collider2D leftTileCollider = Physics2D.OverlapPoint(leftPos, plantableTileLayer);
            if (leftTileCollider != null) tilesToReset.Add(leftTileCollider.GetComponent<TilePrefabs>());

            Vector2 rightPos = (Vector2)closestTile.transform.position + Vector2.right;
            Collider2D rightTileCollider = Physics2D.OverlapPoint(rightPos, plantableTileLayer);
            if (rightTileCollider != null) tilesToReset.Add(rightTileCollider.GetComponent<TilePrefabs>());
        }

        foreach (var tile in tilesToReset)
        {
            foreach (Transform child in tile.transform)
            {
                Destroy(child.gameObject);
            }
            tile.isDirtSpawned = false;
            tile.isSeedSpawned = false;
            tile.isOccupiedByGiantCrop = false;
            tile.RemoveFertilizer(); // 비료 제거 함수가 있다면 호출
        }

        // UIManager.Instance.UpdateHarvestText(); // 이젠 AddHarvestedItem 내부에서 처리하는게 더 좋음
    }

    public void Fertilize(Collider2D closestTile)
    {
        if (closestTile.GetComponent<TilePrefabs>().isFertilized || !closestTile.GetComponent<TilePrefabs>().isDirtSpawned || GameManager.Instance.fertilizerCount == 0)
            return;
        closestTile.GetComponent<TilePrefabs>().GetFertilized();  
        GameManager.Instance.fertilizerCount--;
        UIManager.Instance.UpdateInventoryText();
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
