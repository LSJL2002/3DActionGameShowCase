using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;

public class InvenCallbackSystem : MonoBehaviour
{
    [Header("Objects to deactivate")]
    [SerializeField] private GameObject[] objectsToDeactivate;


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
}