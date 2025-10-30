using System;
using System.Reflection;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineUnscaledUpdater : MonoBehaviour
{
    void LateUpdate()
    {
        CinemachineCore.UniformDeltaTimeOverride = Time.unscaledDeltaTime;
    }

    void OnDisable()
    {
        CinemachineCore.UniformDeltaTimeOverride = -1f;
    }
}