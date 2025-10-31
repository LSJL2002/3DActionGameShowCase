using UnityEngine;

public class ScreenSetting : MonoBehaviour
{
    void Awake()
    {
        Screen.SetResolution(1920, 1080, false);

        SetResolutionToScreen();
    }

    void SetResolutionToScreen()
    {
        int width = Screen.width;
        int height = Screen.height;
        bool fullscreen = false; // WebGL은 보통 창 모드

        Screen.SetResolution(width, height, fullscreen);

        Debug.Log($"SetResolution called with {width}x{height}, fullscreen={fullscreen}");
    }
}
