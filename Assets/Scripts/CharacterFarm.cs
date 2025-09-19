using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
public class CharacterFarm : MonoBehaviour
{
    public float interactionRange = 1.5f;
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

    /*public void Harvest()
    {
        Collider2D[] nearbyCrops = Physics2D.OverlapCircleAll(transform.position, interactionRange, canHarvestLayer);

        if (nearbyCrops.Length == 0) return;
        Collider2D closestCrop = null;
        float minDistance = float.MaxValue;

        foreach (var cropCollider in nearbyCrops)
        {
            float distance = Vector3.Distance(transform.position, cropCollider.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestCrop = cropCollider;
            }
        }
        if (closestCrop == null)
            return;
        if (closestCrop.CompareTag("Crop") && ModeManager.Instance.currentWork == FarmingState.Harvest)
        {
            if (!closestCrop.GetComponentInChildren<CropBehaviour>().IsFullyGrown() || closestCrop.GetComponentInChildren<CropBehaviour>().isEaten)
                return;
            CropManager.Instance.harvestAmountParsnip++;
            UIManager.Instance.harvetText.text = $"ÆÄ½º´Õ : {CropManager.Instance.harvestAmountParsnip}";
            Destroy(closestCrop.gameObject);
        }
    }*/

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
            if (!closestTile.GetComponentInChildren<CropBehaviour>().IsFullyGrown() || closestTile.GetComponentInChildren<CropBehaviour>().isEaten)
                return;
            CropManager.Instance.harvestAmountParsnip++;
            UIManager.Instance.harvetText.text = $"ÆÄ½º´Õ : {CropManager.Instance.harvestAmountParsnip}";
            foreach (Transform child in closestTile.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
