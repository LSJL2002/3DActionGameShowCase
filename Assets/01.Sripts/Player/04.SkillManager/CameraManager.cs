using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraManager : MonoBehaviour
{
    public Transform MainCamera { get; set; }
    public PostProcessVolume Volume { get; set; }
    public CinemachineFreeLook FreeLook { get; set; }



    private void Awake()
    {
        MainCamera = Camera.main.transform;
        Volume = MainCamera.gameObject.GetComponent<PostProcessVolume>();
        var freeLooks = GetComponentsInChildren<CinemachineFreeLook>();
        FreeLook = freeLooks[0]; // 첫 번째
        // FreeLook = freeLooks.FirstOrDefault(f => f.name == "PlayerCam"); // 이름으로 골라내기
    }
}