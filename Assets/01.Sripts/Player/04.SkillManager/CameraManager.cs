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

    private Transform lockOnTarget;
    private Transform player;

    private void Awake()
    {
        MainCamera = Camera.main.transform;
        Volume = MainCamera.gameObject.GetComponent<PostProcessVolume>();
        var freeLooks = GetComponentsInChildren<CinemachineFreeLook>();
        FreeLook = freeLooks[0]; // 첫 번째
        // FreeLook = freeLooks.FirstOrDefault(f => f.name == "PlayerCam"); // 이름으로 골라내기

        player = transform;
    }

    // ===================== Lock-On =====================
    public void SetLockOnTarget(Transform target)
    {
        lockOnTarget = target;
        if (lockOnTarget != null)
        {
            // 다크소울식: 카메라의 LookAt을 타겟으로 변경
            FreeLook.LookAt = lockOnTarget;
            FreeLook.Follow = player; // 카메라는 플레이어를 따라다님
        }
        else
        {
            // Lock-On 해제 → 기본은 플레이어 중심
            FreeLook.LookAt = player;
            FreeLook.Follow = player;
        }
    }

    public bool HasTarget() => lockOnTarget != null;
    public Transform GetLockOnTarget() => lockOnTarget;


    // ===================== 카메라 보정 회전 =====================
    public void RotateTowardsTarget()
    {
    }


    // ===================== 카메라 입력 잠금 =====================
    public void SetCameraInputEnabled(bool enabled)
    {
        if (FreeLook == null) return;
        // X, Y 축 입력 이름으로 받는 경우
        if (enabled)
        {
            FreeLook.m_XAxis.m_InputAxisName = "Mouse X"; // 원래 입력 축 이름
            FreeLook.m_YAxis.m_InputAxisName = "Mouse Y";
        }
        else
        {
            FreeLook.m_XAxis.m_InputAxisName = ""; // 빈 문자열로 입력 끊기
            FreeLook.m_YAxis.m_InputAxisName = "";
        }
        // 만약 다른 방식(Input System 직접 제어)이라면 아래처럼도 가능
        // FreeLook.m_XAxis.m_InputAxisValue = 0f;
        // FreeLook.m_YAxis.m_InputAxisValue = 0f;
    }
}