using System.Collections.Generic;
using UnityEngine;
public class CharacterFarm : MonoBehaviour
{
    #region OldCode
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

    #endregion

    [Header("��ȣ�ۿ� ����")]
    public float interactionRange = 1.5f;
    public LayerMask plantableTileLayer;
    public bool isHarvestingGiantCrop = false;

    [Header("������ �� ����Ʈ")]
    public GameObject dirtPrefabs;
    public GameObject parsnipPrefabs;
    public GameObject carrotPrefabs;
    public GameObject radishPrefabs;
    public GameObject potatoPrefabs;
    public GameObject eggplantPrefabs;
    public GameObject pumpkinPrefabs;
    public GameObject scarecrowPrefabs;
    public float dirtSpawnHeight = 0.5f;
    public float seedSpawnHeight = 1.0f;
    public ParticleSystem waterEffect;
    WaterFillScript waterFill;
    CharacterGroundCheck groundCheck;

    [Header("������Ʈ ���۷���")]
    [SerializeField] GameObject characterSprite;

    [Header("���ο� �� ����")]
    public Vector3 newDayStartPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.Instance.DoPlant += OnFarmingAction;
        FairyManager.Instance.OnFairyEventStarted += NextDayStart;
        {
            if (waterFill != null)
            {
                waterFill.currentWaterAmount = waterFill.maxWaterAmount;
                UIManager.Instance.UpdateWaterText(waterFill.currentWaterAmount);
            }
        };

        waterFill = gameObject.GetComponent<WaterFillScript>();
        groundCheck = gameObject.GetComponent<CharacterGroundCheck>();
    }

    void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.DoPlant -= OnFarmingAction;
            FairyManager.Instance.OnFairyEventStarted -= NextDayStart;
        }
    }

    private void OnFarmingAction()
    {
        if (TryRefillWater()) return;
        PerformFarmingAction();
    }

    private bool TryRefillWater()
    {
        if (ModeManager.Instance.currentWork != FarmingState.Water || !groundCheck.GetOnWater())
            return false;

        if (waterFill.currentWaterAmount >= waterFill.maxWaterAmount)
            return false;

        waterFill.currentWaterAmount = waterFill.maxWaterAmount;
        UIManager.Instance.UpdateWaterText(waterFill.currentWaterAmount);
        return true;
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
            case FarmingState.Scarecrow:
                SetScarecrow();
                break;
        }
    }

    public void DoPlow(Collider2D closestTile)
    {
        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();
        if (tileInfo.isDirtSpawned) return;
        if (tileInfo.isOccupiedByGiantCrop || tileInfo.isOccupiedByScarecrow) return;

        Vector3 spawnPosition = closestTile.transform.position;
        Instantiate(dirtPrefabs, spawnPosition + Vector3.up * dirtSpawnHeight, Quaternion.identity, closestTile.transform);
        closestTile.GetComponent<TilePrefabs>().isDirtSpawned = true;
        tileInfo.isDirtSpawned = true;
    }

    public void DoPlantParsnip(Collider2D closestTile)
    {
        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();

        if (!tileInfo.isDirtSpawned || tileInfo.isSeedSpawned || tileInfo.isOccupiedByGiantCrop || tileInfo.isOccupiedByScarecrow) return;

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

        /*if (groundCheck.GetOnWater() && waterFill.currentWaterAmount < waterFill.maxWaterAmount)
        {
            waterFill.currentWaterAmount = waterFill.maxWaterAmount;
            UIManager.Instance.UpdateWaterText(waterFill.currentWaterAmount);
            return; 
        }*/

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

    #region OldHarvestCode
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
            // 3. �Ŵ� �۹��̶��, ���ʰ� ������ Ÿ���� ã���ϴ�.
            // ���� Ÿ�� ã��
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

            // ��Ȯ�� ����
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
    #endregion

    /*public void Harvest(Collider2D closestTile)
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
            tile.RemoveFertilizer(); 
        }
    }*/

    public void Harvest(Collider2D closestTile)
    {
        // --- 유효성 검사 (초반 리턴 부분) ---
        if (closestTile == null) return;

        CropBehaviour crop = closestTile.GetComponentInChildren<CropBehaviour>();
        if (crop == null) return;

        if (crop.isEaten)
        {
            // 먹힌 작물 타일 초기화 로직 (기존과 동일)
            foreach (Transform child in closestTile.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
            TilePrefabs tile = closestTile.GetComponent<TilePrefabs>();
            tile.isDirtSpawned = false;
            tile.isSeedSpawned = false;
            return;
        }

        if (!crop.IsFullyGrown()) return;

        // --- 수확 준비 ---
        Seed cropData = crop.cropData;
        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();

        // ⭐️ 여기가 핵심적인 변경 부분입니다 ⭐️
        if (cropData.isGiantCrop)
        {
            // 거대 작물일 경우, 확인 창을 띄웁니다.
            UIManager.Instance.ShowConfirmationPopup(
                onYes: () => {
                    // "예"를 눌렀을 때만 실행될 거대 작물 수확 로직 전체
                    int amountToHarvest = cropData.giantCropYield;

                    if (amountToHarvest > 0)
                    {
                        CropManager.Instance.AddHarvestedItem(cropData.harvestedItemID, amountToHarvest);
                    }

                    List<TilePrefabs> tilesToReset = new List<TilePrefabs> { tileInfo };

                    Vector2 leftPos = (Vector2)closestTile.transform.position + Vector2.left;
                    Collider2D leftTileCollider = Physics2D.OverlapPoint(leftPos, plantableTileLayer);
                    if (leftTileCollider != null) tilesToReset.Add(leftTileCollider.GetComponent<TilePrefabs>());

                    Vector2 rightPos = (Vector2)closestTile.transform.position + Vector2.right;
                    Collider2D rightTileCollider = Physics2D.OverlapPoint(rightPos, plantableTileLayer);
                    if (rightTileCollider != null) tilesToReset.Add(rightTileCollider.GetComponent<TilePrefabs>());

                    foreach (var tile in tilesToReset)
                    {
                        foreach (Transform child in tile.transform)
                        {
                            Destroy(child.gameObject);
                        }
                        tile.isDirtSpawned = false;
                        tile.isSeedSpawned = false;
                        tile.isOccupiedByGiantCrop = false;
                        tile.RemoveFertilizer();
                    }
                },
                onNo: () => {
                    // "아니오"를 누르면 아무것도 하지 않고 로그만 남깁니다.
                    Debug.Log("거대 작물 수확을 취소했습니다.");
                }
            );
        }
        else // 일반 작물일 경우
        {
            // 기존의 일반 작물 수확 로직을 그대로 실행합니다.
            int amountToHarvest = 0;
            if (tileInfo.isFertilized)
            {
                amountToHarvest = Random.Range(cropData.minFertilizedYield, cropData.maxFertilizedYield + 1);
            }
            else
            {
                amountToHarvest = Random.Range(cropData.minYield, cropData.maxYield + 1);
            }

            if (amountToHarvest > 0)
            {
                CropManager.Instance.AddHarvestedItem(cropData.harvestedItemID, amountToHarvest);
            }

            // 일반 작물은 타일 하나만 초기화합니다.
            foreach (Transform child in tileInfo.transform)
            {
                Destroy(child.gameObject);
            }
            tileInfo.isDirtSpawned = false;
            tileInfo.isSeedSpawned = false;
            tileInfo.RemoveFertilizer();
        }
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

    public void NextDayStart(int newday)
    {
        gameObject.transform.position = newDayStartPosition;
    }

    private void SetScarecrow()
    {
        Collider2D closestTile = GetClosestTile();
        if (closestTile == null || !closestTile.CompareTag("Tile")) return;

        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();

        // 1. 해당 타일에 이미 허수아비가 있을 경우 -> '제거' 로직만 수행
        if (tileInfo.isOccupiedByScarecrow)
        {
            // 자식 오브젝트를 안전하게 찾아서 파괴
            ScarecrowScript scarecrow = tileInfo.GetComponentInChildren<ScarecrowScript>();
            if (scarecrow != null)
            {
                Destroy(scarecrow.gameObject);
            }

            tileInfo.isOccupiedByScarecrow = false;
            GameManager.Instance.scarecrowCount++; // 허수아비 개수 복구
            UIManager.Instance.UpdateInventoryText(); // UI 업데이트

            // '제거'를 했으므로 여기서 함수를 종료
            UIManager.Instance.ShowScarecrowMessage("허수아비를 제거했습니다.");
            return;
        }

        // 2. 허수아비가 없을 경우 -> '설치' 로직 수행

        // 기능 추가: 설치할 허수아비가 남아있는지 확인
        if (GameManager.Instance.scarecrowCount <= 0)
        {
            UIManager.Instance.ShowScarecrowMessage("허수아비가 부족합니다.");
            return; // 허수아비가 없으면 함수 종료
        }
        if(tileInfo.isSeedSpawned == true || tileInfo.isDirtSpawned == true)
        {
            UIManager.Instance.ShowScarecrowMessage("작물이 심어진 타일에는 허수아비를 설치할 수 없습니다.");
            return;
        }
        // 허수아비 설치
        Vector3 spawnPosition = closestTile.transform.position;
        Instantiate(scarecrowPrefabs, spawnPosition + Vector3.up * seedSpawnHeight, Quaternion.identity, closestTile.transform);
        tileInfo.isOccupiedByScarecrow = true;
        GameManager.Instance.scarecrowCount--; // 허수아비 개수 차감
        UIManager.Instance.UpdateInventoryText(); // UI 업데이트
    }
}
