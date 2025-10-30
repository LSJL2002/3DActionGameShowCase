using Unity.Cinemachine;
using UnityEngine;

public class CameraEventListener : MonoBehaviour
{
    void OnEnable()
    {
        // 구독
        //CinemachineCore.CameraActivatedEvent.AddListener(OnCameraActivated);
        //CinemachineCore.CameraDeactivatedEvent.AddListener(OnCameraDeactivated);
    }

    void OnDisable()
    {
        // 구독 해제
        //CinemachineCore.CameraActivatedEvent.RemoveListener(OnCameraActivated);
        //CinemachineCore.CameraDeactivatedEvent.RemoveListener(OnCameraDeactivated);
    }

    private void OnCameraActivated(ICinemachineCamera cam)
    {
        Debug.Log($"[Cinemachine] Activated: {cam?.Name}");
    }

    private void OnCameraDeactivated(ICinemachineCamera cam)
    {
        Debug.Log($"[Cinemachine] Deactivated: {cam?.Name}");
    }
}
