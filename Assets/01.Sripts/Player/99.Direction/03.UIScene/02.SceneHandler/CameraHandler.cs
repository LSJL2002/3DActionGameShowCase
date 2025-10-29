using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public GameObject[] objectsToEnableGameplay;
    public GameObject[] objectsToEnableInventory;
    public GameObject[] objectsToEnableCutscene;

    void OnEnable()
    {
        UISceneManager.OnModeChanged += HandleMode;
    }

    void OnDisable()
    {
        UISceneManager.OnModeChanged -= HandleMode;
    }

    void HandleMode(UISceneManager.CameraMode mode)
    {
        // 기본 전부 끄고
        SetActiveArray(objectsToEnableGameplay, false);
        SetActiveArray(objectsToEnableInventory, false);
        SetActiveArray(objectsToEnableCutscene, false);

        // 해당 모드 켜기
        switch (mode)
        {
            case UISceneManager.CameraMode.Gameplay:
                SetActiveArray(objectsToEnableGameplay, true);
                break;
            case UISceneManager.CameraMode.Inventory:
                SetActiveArray(objectsToEnableInventory, true);
                break;
            case UISceneManager.CameraMode.Cutscene:
                SetActiveArray(objectsToEnableCutscene, true);
                break;
        }
    }

    void SetActiveArray(GameObject[] objs, bool state)
    {
        foreach (var o in objs)
            if (o) o.SetActive(state);
    }
}
