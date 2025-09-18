using UnityEngine;
using System.Collections;
public class SpawnSqueeze : MonoBehaviour
{
    GameObject prefabSprite;

    [Header("Settings - Squash and Stretch")]
    [SerializeField, Tooltip("Width Squeeze, Height Squeeze, Duration")] Vector3 jumpSquashSettings;
    [SerializeField, Tooltip("Width Squeeze, Height Squeeze, Duration")] Vector3 landSquashSettings;
    [SerializeField, Tooltip("How powerful should the effect be?")] public float landSqueezeMultiplier;
    [SerializeField, Tooltip("How powerful should the effect be?")] public float jumpSqueezeMultiplier;
    [SerializeField] float landDrop = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        prefabSprite = transform.gameObject;
        StartCoroutine(StartSqueeze(landSquashSettings.x * landSqueezeMultiplier, landSquashSettings.y / landSqueezeMultiplier, landSquashSettings.z, landDrop, false));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartSqueeze(float xSqueeze, float ySqueeze, float seconds, float dropAmount, bool jumpSqueeze)
    {

        Vector3 originalSize = prefabSprite.transform.localScale;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);

        Vector3 originalPosition = prefabSprite.transform.localPosition;
        Vector3 newPosition = originalPosition - Vector3.up * dropAmount;

        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            prefabSprite.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            prefabSprite.transform.localPosition = Vector3.Lerp(originalPosition, newPosition, t);
            yield return null;
        }

        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            prefabSprite.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            prefabSprite.transform.localPosition = Vector3.Lerp(newPosition, originalPosition, t);
            yield return null;
        }
    }
}
