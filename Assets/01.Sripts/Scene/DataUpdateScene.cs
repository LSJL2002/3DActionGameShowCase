using UnityEngine;

public class DataUpdateScene : SceneBase
{
    protected override void Awake()
    {
        base.Awake();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
