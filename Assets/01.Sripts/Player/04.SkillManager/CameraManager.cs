using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraManager : Singleton<CameraManager>
{
    public Transform MainCamera { get; set; }
    public PostProcessVolume volume { get; set; }


    private void Awake()
    {
        MainCamera = Camera.main.transform;
        volume = MainCamera.gameObject.GetComponent<PostProcessVolume>();

    }
}
