using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.Universal.PixelPerfectCamera;
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
            case FarmingState.Talk:
                Talk2GiantCrop(closestTile);
                break;
            case FarmingState.Pesticide:
                GivePesticide(closestTile);
                break;
            case FarmingState.Manure:
                GiveManure(closestTile);
                break;
            case FarmingState.Nutrition:
                GiveNutrition(closestTile);
                break;
        }
    }

    public void Talk2GiantCrop(Collider2D closestTile)
    {
        TilePrefabs tileInfo = closestTile.GetComponent<TilePrefabs>();
        Debug.Log("Talk to Giant Crop");

        if (tileInfo == null || tileInfo.GetComponentInChildren<CropBehaviour>() == null) return;

        Seed cropData = tileInfo.GetComponentInChildren<CropBehaviour>()?.cropData;
        SeedLike cropLikeInstance = tileInfo.GetComponentInChildren<CropBehaviour>().cropLikeInstance;

        if(cropData == null || cropLikeInstance == null)
            return;

        if (closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.isTodayAlreadyTalk)
        {
            string text = $"오늘 {cropData.cropName}에게 이미 말을 걸었습니다. ( 현재 호감도: {cropLikeInstance.likeability} )";
            UIManager.Instance.NoticeSimpleLikeability(cropData.name, text);
            return;
        }

        // 호감도 이상 결혼 ui 띄우기
        if (cropLikeInstance.likeability >=50 && cropData.isGiantCrop)
        {
            closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.isTodayAlreadyTalk = true;

            UIManager.Instance.ShowConfirmationMarry(
                onYes: () => {
                    UIManager.Instance.NoticeSimpleLikeability(cropData.cropName, SelectRandomMarryDialogue(closestTile));
                    if(cropData.cropName == "거대무")
                        UIManager.Instance.NoticeSimpleLikeability(cropData.cropName, $"{cropData.cropName}와(과) 결혼했습니다!  - 게임 클리어 -");
                    else
                        UIManager.Instance.NoticeSimpleLikeability(cropData.cropName, $"{cropData.cropName}와(과) 결혼했습니다!");
                },
                onNo: () => {
                    cropLikeInstance.likeability = -10;
                    string text = $"파혼을 시도하여 {cropData.cropName}의 호감도가 크게 감소했습니다. ( 호감도: {cropLikeInstance.likeability} )";
                    UIManager.Instance.NoticeSimpleLikeability(name, SelectRandomHateDialogue(closestTile));
                    UIManager.Instance.NoticeSimpleLikeability(name, text);
                }
            , cropLikeInstance.likeability, cropData.cropName);
            return;
        }

        // 기본 말걸기
        if (tileInfo.GetComponentInChildren<CropBehaviour>() != null && tileInfo.GetComponentInChildren<CropBehaviour>().cropData.isGiantCrop)
        {
            string name = cropData.cropName;

            closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.likeability += 10;
            closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.isTodayAlreadyTalk = true;
            

            string dialogue = "";
            if (cropLikeInstance.likeability >= 0 && cropLikeInstance.likeability < 10)
                dialogue = SelectRandomSmallLikeDialogue(closestTile);
            else if (cropLikeInstance.likeability >= 10 && cropLikeInstance.likeability < 30)
                dialogue = SelectRandomMidLikeDialogue(closestTile);
            else if (cropLikeInstance.likeability >= 30 && cropLikeInstance.likeability < 50)
                dialogue = SelectRandomVeryLikeDialogue(closestTile);
            else if (cropLikeInstance.likeability < 0)
                dialogue = SelectRandomHateDialogue(closestTile);

            UIManager.Instance.NoticeSimpleLikeability(name, dialogue);
            UIManager.Instance.NoticeLikeability(closestTile.GetComponentInChildren<CropBehaviour>().cropData.cropName,
                    closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.likeability);
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

        if (groundCheck.GetOnWater())
        {
            // 물 위에 있다면, 물 양을 가득 채웁니다.
            waterFill.currentWaterAmount = waterFill.maxWaterAmount;
            UIManager.Instance.UpdateWaterText(waterFill.currentWaterAmount);
            return; // 충전했으니 함수를 종료합니다.
        }

        // 2. (물 위에 있지 않을 경우에만 이 코드가 실행됨)
        // 물 양이 0 이하면 더 이상 진행하지 않고 함수를 종료합니다.
        // (이 코드는 물을 사용하는 로직으로 추정됩니다.)
        if (waterFill.currentWaterAmount <= 0)
        {
            return;
        }

        if (closestTile == null) return;
        if (closestTile.gameObject.GetComponentInChildren<CropBehaviour>() != null && closestTile.GetComponentInChildren<CropBehaviour>().cropData.isGiantCrop)
        {
            Seed cropData = closestTile.GetComponentInChildren<CropBehaviour>().cropData;
            SeedLike cropLikeData = closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance;

            string name = cropData.cropName;
            if (closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.isTodayLikeGiven)
            {
                string text = $"오늘 {name}에게 이미 물을 주었습니다. ( 현재 호감도: {cropLikeData.likeability} )";
                UIManager.Instance.NoticeSimpleLikeability(name, text);
                return; 
            }

            closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.likeability += 1;
            UIManager.Instance.NoticeLikeability(closestTile.GetComponentInChildren<CropBehaviour>().cropData.cropName, 
                closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.likeability);

            string dialogue = "";
            if (cropLikeData.likeability >= 0 && cropLikeData.likeability < 10)
                dialogue = SelectRandomSmallLikeDialogue(closestTile);
            else if (cropLikeData.likeability >= 10 && cropLikeData.likeability < 30)
                dialogue = SelectRandomMidLikeDialogue(closestTile);
            else if (cropLikeData.likeability >= 30 && cropLikeData.likeability < 50)
                dialogue = SelectRandomVeryLikeDialogue(closestTile);
            else if (cropLikeData.likeability < 0)
                dialogue = SelectRandomHateDialogue(closestTile);

            UIManager.Instance.NoticeSimpleLikeability(name, dialogue);
            closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.isTodayLikeGiven = true;
        }
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

        if (cropData.isGiantCrop)
        {
            SeedLike seedLike = closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance;

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
                    Seed giantCrop = closestTile.GetComponentInChildren<CropBehaviour>().cropData;
                    
                    string name = giantCrop.cropName;
                    seedLike.likeability -= 5;
                    string text = $"수확을 시도하여 {name}의 호감도가 크게 감소했습니다. ( 호감도: {seedLike.likeability} )";
                    UIManager.Instance.NoticeSimpleLikeability(name, text);
                    UIManager.Instance.NoticeSimpleLikeability(name, SelectRandomHateDialogue(closestTile));

                }
            , seedLike.likeability);
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

        if (tileInfo.isOccupiedByScarecrow)
        {
            ScarecrowScript scarecrow = tileInfo.GetComponentInChildren<ScarecrowScript>();

            if (scarecrow != null)
            {
                Destroy(scarecrow.gameObject);
            }

            tileInfo.isOccupiedByScarecrow = false;
            GameManager.Instance.scarecrowCount++; 
            UIManager.Instance.UpdateInventoryText();

            UIManager.Instance.ShowScarecrowMessage("허수아비를 제거했습니다.");
            return;
        }

        // 2. 허수아비가 없을 경우 -> '설치' 로직 수행

        // 기능 추가: 설치할 허수아비가 남아있는지 확인
        if (GameManager.Instance.scarecrowCount <= 0)
        {
            UIManager.Instance.ShowScarecrowMessage("허수아비가 부족합니다.");
            return; 
        }
        if(tileInfo.isSeedSpawned == true || tileInfo.isDirtSpawned == true)
        {
            UIManager.Instance.ShowScarecrowMessage("빈 타일에만 설치할 수 있습니다.");
            return;
        }

        Vector3 spawnPosition = closestTile.transform.position;
        Instantiate(scarecrowPrefabs, spawnPosition + Vector3.up * seedSpawnHeight, Quaternion.identity, closestTile.transform);
        tileInfo.isOccupiedByScarecrow = true;
        GameManager.Instance.scarecrowCount--; // 허수아비 개수 차감
        UIManager.Instance.UpdateInventoryText(); // UI 업데이트
    }

    private string SelectRandomMarryDialogue(Collider2D closestTile)
    {
        string dialogue = "";
        Seed cropData = closestTile.GetComponentInChildren<CropBehaviour>().cropData;
        if (cropData.dialoguesForMarry.Length == 0)
        {
            dialogue = "대화할 수 있는 대사가 없습니다.";
            return dialogue;
        }
        int randomIndex = Random.Range(0, cropData.dialoguesForMarry.Length);
        dialogue = cropData.dialoguesForMarry[randomIndex];
        UIManager.Instance.cropImage.sprite = closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.marryImage;
        UIManager.Instance.cropImage.transform.localScale = new Vector3(0.8f, 1f, 1f);
        return dialogue;
    }

    private string SelectRandomSmallLikeDialogue(Collider2D closestTile)
    {
        string dialogue = "";
        Seed cropData = closestTile.GetComponentInChildren<CropBehaviour>().cropData;
        if (cropData.dialoguesForSmallLike.Length == 0)
        {
            dialogue = "대화할 수 있는 대사가 없습니다.";
            return dialogue;
        }
        int randomIndex = Random.Range(0, cropData.dialoguesForSmallLike.Length);
        dialogue = cropData.dialoguesForSmallLike[randomIndex];
        UIManager.Instance.cropImage.sprite = closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.smallLikeImage;
        return dialogue;
    }

    private string SelectRandomHateDialogue(Collider2D closestTile)
    {
        string dialogue = "";
        Seed cropData = closestTile.GetComponentInChildren<CropBehaviour>().cropData;
        if (cropData.dialoguesForHate.Length == 0)
        {
            dialogue = "대화할 수 있는 대사가 없습니다.";
            return dialogue;
        }
        int randomIndex = Random.Range(0, cropData.dialoguesForHate.Length);
        dialogue = cropData.dialoguesForHate[randomIndex];
        UIManager.Instance.cropImage.sprite = closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.hateImage;
        return dialogue;
    }

    private string SelectRandomMidLikeDialogue(Collider2D closestTile)
    {
        string dialogue = "";
        Seed cropData = closestTile.GetComponentInChildren<CropBehaviour>().cropData;
        if (cropData.dialoguesForMidLike.Length == 0)
        {
            dialogue = "대화할 수 있는 대사가 없습니다.";
            return dialogue;
        }
        int randomIndex = Random.Range(0, cropData.dialoguesForMidLike.Length);
        dialogue = cropData.dialoguesForMidLike[randomIndex];
        UIManager.Instance.cropImage.sprite = closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.midLikeImage;
        return dialogue;
    }

    private string SelectRandomVeryLikeDialogue(Collider2D closestTile)
    {
        string dialogue = "";
        Seed cropData = closestTile.GetComponentInChildren<CropBehaviour>().cropData;
        if (cropData.dialoguesForVeryLike.Length == 0)
        {
            dialogue = "대화할 수 있는 대사가 없습니다.";
            return dialogue;
        }
        int randomIndex = Random.Range(0, cropData.dialoguesForVeryLike.Length);
        dialogue = cropData.dialoguesForVeryLike[randomIndex];
        UIManager.Instance.cropImage.sprite = closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.veryLikeImage;
        return dialogue;
    }

    private string SelectRandomGoodStuffDialogue(Collider2D closestTile)
    {
        string dialogue = "";
        Seed cropData = closestTile.GetComponentInChildren<CropBehaviour>().cropData;

        SeedLike cropLikeData = closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance;
        cropLikeData.likeability += 10;
        dialogue = $"{cropData.cropName}의 호감도가 10 증가했습니다. ( 현재 호감도: {cropLikeData.likeability} )";
        UIManager.Instance.NoticeSimpleLikeability(cropData.cropName, dialogue);
        
        if (cropData.dialoguesForGoodStuff.Length == 0)
        {
            dialogue = "대화할 수 있는 대사가 없습니다.";
            return dialogue;
        }
        int randomIndex = Random.Range(0, cropData.dialoguesForGoodStuff.Length);
        dialogue = cropData.dialoguesForGoodStuff[randomIndex];
        UIManager.Instance.cropImage.sprite = cropLikeData.veryLikeImage;
        return dialogue;
    }

    private string SelectRandomBadStuffDialogue(Collider2D closestTile)
    {
        string dialogue = "";
        Seed cropData = closestTile.GetComponentInChildren<CropBehaviour>().cropData;

        SeedLike cropLikeData = closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance;
        cropLikeData.likeability -= 10;
        dialogue = $"{cropData.cropName}의 호감도가 10 감소했습니다. ( 현재 호감도: {cropLikeData.likeability} )";
        UIManager.Instance.NoticeSimpleLikeability(cropData.cropName, dialogue);

        if (cropData.dialoguesForBadStuff.Length == 0)
        {
            dialogue = "대화할 수 있는 대사가 없습니다.";
            return dialogue;
        }
        int randomIndex = Random.Range(0, cropData.dialoguesForBadStuff.Length);
        dialogue = cropData.dialoguesForBadStuff[randomIndex];
        UIManager.Instance.cropImage.sprite = cropLikeData.hateImage;
        return dialogue;
    }

    private string SelectRandomNormalStuffDialogue(Collider2D closestTile)
    {
        string dialogue = "";
        Seed cropData = closestTile.GetComponentInChildren<CropBehaviour>().cropData;

        SeedLike cropLikeData = closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance;
        cropLikeData.likeability += 3;
        dialogue = $"{cropData.cropName}의 호감도가 3 증가했습니다. ( 현재 호감도: {cropLikeData.likeability} )";
        UIManager.Instance.NoticeSimpleLikeability(cropData.cropName, dialogue);

        if (cropData.dialoguesForNormalStuff.Length == 0)
        {
            dialogue = "대화할 수 있는 대사가 없습니다.";
            return dialogue;
        }
        int randomIndex = Random.Range(0, cropData.dialoguesForNormalStuff.Length);
        dialogue = cropData.dialoguesForNormalStuff[randomIndex];
        UIManager.Instance.cropImage.sprite = cropLikeData.smallLikeImage;
        return dialogue;
    }

    public void GivePesticide(Collider2D closestTile)
    {
        if(closestTile != null && closestTile.GetComponentInChildren<CropBehaviour>() != null)
        {
            Seed cropData = closestTile.GetComponentInChildren<CropBehaviour>().cropData;
            SeedLike cropLikeData = closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance;
            if(cropData == null || cropLikeData == null)
                return;

            if (cropLikeData.isTodayAlreadyGift)
            {
                string text = $"오늘 이미 {cropData.cropName}에게 선물을 주었습니다. ( 현재 호감도: {cropLikeData.likeability} )";
                UIManager.Instance.NoticeSimpleLikeability(cropData.name, text);
                return;
            }

            if(cropData.isGiantCrop)
            {
                cropLikeData.isTodayAlreadyGift = true;
                string dialogue = $"{cropData.cropName}에게 농약을 주었습니다.";
                UIManager.Instance.NoticeSimpleLikeability(name, dialogue);
                
               if(cropData.cropName == "거대파스닙")
                {
                    dialogue = SelectRandomNormalStuffDialogue(closestTile);
                }
                else if(cropData.cropName == "거대당근")
                {
                    dialogue = SelectRandomNormalStuffDialogue(closestTile);
                }
                else if(cropData.cropName == "거대무")
                {
                    dialogue = SelectRandomGoodStuffDialogue(closestTile);
                }
                else if(cropData.cropName == "거대감자")
                {
                    dialogue = SelectRandomGoodStuffDialogue(closestTile);
                }
                else if(cropData.cropName == "거대가지")
                {
                    dialogue = SelectRandomBadStuffDialogue(closestTile);
                }
                else if(cropData.cropName == "거대호박")
                {
                    dialogue = SelectRandomBadStuffDialogue(closestTile);
                }

                UIManager.Instance.NoticeSimpleLikeability(name, dialogue);

                UIManager.Instance.NoticeLikeability(closestTile.GetComponentInChildren<CropBehaviour>().cropData.cropName,
                        closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.likeability);
            }
            GameManager.Instance.pesticideCount--;
        }
    }

    public void GiveManure(Collider2D closestTile)
    {
        if (closestTile != null && closestTile.GetComponentInChildren<CropBehaviour>() != null)
        {
            Seed cropData = closestTile.GetComponentInChildren<CropBehaviour>().cropData;
            SeedLike cropLikeData = closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance;
            if (cropData == null || cropLikeData == null)
                return;

            if (cropLikeData.isTodayAlreadyGift)
            {
                string text = $"오늘 이미 {cropData.cropName}에게 선물을 주었습니다. ( 현재 호감도: {cropLikeData.likeability} )";
                UIManager.Instance.NoticeSimpleLikeability(cropData.name, text);
                return;
            }

            if (cropData.isGiantCrop)
            {
                cropLikeData.isTodayAlreadyGift = true;
                string dialogue = $"{cropData.cropName}에게 거름을 주었습니다.";
                UIManager.Instance.NoticeSimpleLikeability(name, dialogue);

                if (cropData.cropName == "거대파스닙")
                {
                    dialogue = SelectRandomGoodStuffDialogue(closestTile);
                }
                else if (cropData.cropName == "거대당근")
                {
                    dialogue = SelectRandomGoodStuffDialogue(closestTile);
                }
                else if (cropData.cropName == "거대무")
                {
                    dialogue = SelectRandomBadStuffDialogue(closestTile);
                }
                else if (cropData.cropName == "거대감자")
                {
                    dialogue = SelectRandomBadStuffDialogue(closestTile);
                }
                else if (cropData.cropName == "거대가지")
                {
                    dialogue = SelectRandomNormalStuffDialogue(closestTile);
                }
                else if (cropData.cropName == "거대호박")
                {
                    dialogue = SelectRandomNormalStuffDialogue(closestTile);
                }

                UIManager.Instance.NoticeSimpleLikeability(name, dialogue);
                UIManager.Instance.NoticeLikeability(closestTile.GetComponentInChildren<CropBehaviour>().cropData.cropName,
                        closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.likeability);
            }
            GameManager.Instance.manureCount--;
        }
    }

    public void GiveNutrition(Collider2D closestTile)
    {
        if (closestTile != null && closestTile.GetComponentInChildren<CropBehaviour>() != null)
        {
            Seed cropData = closestTile.GetComponentInChildren<CropBehaviour>().cropData;
            SeedLike cropLikeData = closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance;
            if (cropData == null || cropLikeData == null)
                return;

            if (cropLikeData.isTodayAlreadyGift)
            {
                string text = $"오늘 이미 {cropData.cropName}에게 선물을 주었습니다. ( 현재 호감도: {cropLikeData.likeability} )";
                UIManager.Instance.NoticeSimpleLikeability(cropData.name, text);
                return;
            }

            if (cropData.isGiantCrop)
            {
                string dialogue = $"{cropData.cropName}에게 영양제를 주었습니다.";
                cropLikeData.isTodayAlreadyGift = true;
                UIManager.Instance.NoticeSimpleLikeability(name, dialogue);

                if (cropData.cropName == "거대파스닙")
                {
                    dialogue = SelectRandomBadStuffDialogue(closestTile);
                }
                else if (cropData.cropName == "거대당근")
                {
                    dialogue = SelectRandomBadStuffDialogue(closestTile);
                }
                else if (cropData.cropName == "거대무")
                {
                    dialogue = SelectRandomNormalStuffDialogue(closestTile);
                }
                else if (cropData.cropName == "거대감자")
                {
                    dialogue = SelectRandomNormalStuffDialogue(closestTile);
                }
                else if (cropData.cropName == "거대가지")
                {
                    dialogue = SelectRandomGoodStuffDialogue(closestTile);
                }
                else if (cropData.cropName == "거대호박")
                {
                    dialogue = SelectRandomGoodStuffDialogue(closestTile);
                }

                UIManager.Instance.NoticeSimpleLikeability(name, dialogue);
                UIManager.Instance.NoticeLikeability(closestTile.GetComponentInChildren<CropBehaviour>().cropData.cropName,
                        closestTile.GetComponentInChildren<CropBehaviour>().cropLikeInstance.likeability);
            }
            GameManager.Instance.nutritionCount--;
        }
    }

}
