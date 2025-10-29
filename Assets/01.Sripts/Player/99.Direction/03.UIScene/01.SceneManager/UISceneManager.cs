using UnityEngine;

public class UISceneManager : MonoBehaviour
{
    public static UISceneManager Instance;

    public enum CameraMode { Gameplay, Inventory, Cutscene }
    public CameraMode currentMode;

    public delegate void CameraModeChanged(CameraMode mode);
    public static event CameraModeChanged OnModeChanged;

    void Awake() => Instance = this;

    public void SetMode(CameraMode mode)
    {
        currentMode = mode;
        OnModeChanged?.Invoke(mode);
    }

    // 예: 키 입력으로 전환
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            SetMode(currentMode == CameraMode.Inventory ? CameraMode.Gameplay : CameraMode.Inventory);
    }
}
