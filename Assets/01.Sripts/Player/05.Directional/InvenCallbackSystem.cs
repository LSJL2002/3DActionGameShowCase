using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;

public class InvenCallbackSystem : MonoBehaviour
{
    [Header("Objects to deactivate")]
    [SerializeField] private GameObject[] objectsToDeactivate;

    [Header("Virtual Cameras")]
    [SerializeField] private CinemachineVirtualCamera camera1;
    [SerializeField] private CinemachineVirtualCamera camera2;
    [SerializeField] private CinemachineVirtualCamera camera3;
    [SerializeField] private CinemachineVirtualCamera camera4;
    [SerializeField] private CinemachineVirtualCamera camera5;

    private CinemachineVirtualCamera[] allCameras;

    private void Awake()
    {
        // 모든 카메라 배열로 관리
        allCameras = new CinemachineVirtualCamera[] { camera1, camera2, camera3, camera4, camera5 };
    }

    // -------------------------
    // 배열에서 특정 인덱스만 켜고 나머지는 OFF
    private void ActivateObjectByIndex(int index)
    {
        for (int i = 0; i < objectsToDeactivate.Length; i++)
        {
            if (objectsToDeactivate[i] == null) continue;
            objectsToDeactivate[i].SetActive(i == index);
        }
    }

    // -------------------------
    // 특정 카메라 + 특정 오브젝트 인덱스 활성화
    // objectIndexToActivate = -1 → 모든 오브젝트 OFF
    private void ActivateSingleCamera(CinemachineVirtualCamera camToActivate, int objectIndexToActivate)
    {
        // 오브젝트 처리
        if (objectIndexToActivate >= 0) ActivateObjectByIndex(objectIndexToActivate);
        else DeactivateObjects();

        // 카메라 Priority 처리
        foreach (var cam in allCameras)
        {
            if (cam != null)
                cam.Priority = (cam == camToActivate) ? 10 : 0;
        }
    }

    // -------------------------
    // Animation Event용 함수
    public void ActivateCamera1() => ActivateSingleCamera(camera1, 0); // 배열의 0번째 오브젝트만 ON
    public void ActivateCamera2() => ActivateSingleCamera(camera2, -1);
    public void ActivateCamera3() => ActivateSingleCamera(camera3, 1);
    public void ActivateCamera4() => ActivateSingleCamera(camera4, -1);
    public void ActivateCamera5() => ActivateSingleCamera(camera5, -1);


    // -------------------------
    public void DeactivateObjects()
    {
        foreach (var obj in objectsToDeactivate)
            if (obj != null) obj.SetActive(false);
    }

    public void ActivateObjects()
    {
        foreach (var obj in objectsToDeactivate)
            if (obj != null) obj.SetActive(true);
    }

    // 초기화
    public void ResetAllCamerasAndObjects()
    {
        foreach (var cam in allCameras)
            if (cam != null) cam.Priority = 0;
        ActivateObjects();
    }
}