using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// This script manually updates all Cinemachine virtual cameras
/// using Time.unscaledDeltaTime. This allows the camera to move
/// and respond to input even when Time.timeScale is set to 0.
/// </summary>
public class UpdateCineCamera : MonoBehaviour
{
    /*private CinemachineBrain cinemachineBrain;

    void Awake()
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
    }

    void LateUpdate()
    {
        // Only run this logic if the brain is in Manual Update mode.
        if (cinemachineBrain.UpdateMethod != CinemachineBrain.UpdateMethod.ManualUpdate)
        {
            return;
        }

        // Get the currently active virtual camera.
        ICinemachineCamera activeCamera = cinemachineBrain.ActiveVirtualCamera;

        // If a virtual camera is active, update it manually.
        if (activeCamera != null)
        {
            // This is the correct, modern static method to call.
            CinemachineCore.UpdateVirtualCameras(
                activeCamera,
                Vector3.up,
                Time.unscaledDeltaTime
            );
        }
    }*/
}