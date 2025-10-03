using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;

public class InvenCallbackSystem : MonoBehaviour
{
    [Header("Objects to deactivate")]
    [SerializeField] private GameObject[] objectsToDeactivate;

    [Header("Virtual Cameras")]               // 버츄얼 카메라 배열
    [SerializeField] private CinemachineVirtualCamera[] virtualCameras;

    [Header("Priority Settings")]
    [SerializeField] private int activePriority = 20;   // 켜질 때 우선순위
    [SerializeField] private int inactivePriority = 0;  // 꺼질 때 우선순위


    // -------------------------
    // 배열에서 특정 인덱스만 켜고 나머지는 OFF
    public void ActivateObjectByIndex(int index)
    {
        for (int i = 0; i < objectsToDeactivate.Length; i++)
        {
            if (objectsToDeactivate[i] == null) continue;
            objectsToDeactivate[i].SetActive(i == index);
        }
    }

    // -------------------------
    // 모든 오브젝트 OFF
    public void DeactivateObjects()
    {
        foreach (var obj in objectsToDeactivate)
            if (obj != null) obj.SetActive(false);
    }

    // 모든 오브젝트 ON
    public void ActivateObjects()
    {
        foreach (var obj in objectsToDeactivate)
            if (obj != null) obj.SetActive(true);
    }

    // -------------------------
    // 초기화
    public void ResetAllObjects()
    {
        ActivateObjects();
    }

    // =========================
    // Animation Event용 함수
    // 특정 카메라만 우선순위 ↑, 나머지는 ↓
    public void SetCameraPriorityByIndex(int index)
    {
        if (virtualCameras == null || virtualCameras.Length == 0) return;

        for (int i = 0; i < virtualCameras.Length; i++)
        {
            if (virtualCameras[i] == null) continue;
            virtualCameras[i].Priority = (i == index) ? activePriority : inactivePriority;
        }
    }

    // 모든 카메라를 비활성화 우선순위로 초기화
    public void ResetCameraPriorities()
    {
        foreach (var cam in virtualCameras)
            if (cam != null) cam.Priority = inactivePriority;
    }
}