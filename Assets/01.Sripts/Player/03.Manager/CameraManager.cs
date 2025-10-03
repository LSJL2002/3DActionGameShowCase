using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraManager : MonoBehaviour
{
    public Transform MainCamera { get; private set; }
    public PostProcessVolume Volume { get; private set; }
    public CinemachineFreeLook FreeLookCam { get; private set; }

    public CinemachineTargetGroup targetGroup {  get; private set; }
    public CinemachineVirtualCamera targetCam { get; private set; }
    public CinemachineBasicMultiChannelPerlin noise {  get; private set; }
    private float shakeTimer;

    public Transform player; // 인스펙터에서 플레이어 위치 할당
    private Transform lockOnTarget;

    private void Awake()
    {
        MainCamera = Camera.main.transform;
        Volume = MainCamera.gameObject.GetComponent<PostProcessVolume>();
        var freeLooks = GetComponentsInChildren<CinemachineFreeLook>();
        FreeLookCam = freeLooks[0]; // 첫 번째
        // FreeLook = freeLooks.FirstOrDefault(f => f.name == "PlayerCam"); // 이름으로 골라내기

        targetGroup = GetComponentInChildren<CinemachineTargetGroup>();
        targetCam = targetGroup.GetComponentInChildren<CinemachineVirtualCamera>();
        noise = targetCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                // 흔들림 원상 복구
                noise.m_AmplitudeGain = 0f;
            }
        }
    }


    // ======================== 카메라 Lock-On ========================
    public void ToggleLockOnTarget(Transform target)
    {
        var groupCam = targetGroup.GetComponentInChildren<CinemachineVirtualCamera>(true);

        lockOnTarget = target;

        // 0번(플레이어)은 항상 유지
        var targets = targetGroup.m_Targets;

        if (targets.Length < 2)
        {
            // 초기 세팅: 플레이어 + 빈 슬롯
            targets = new CinemachineTargetGroup.Target[2];
            targets[0] = new CinemachineTargetGroup.Target { target = player, weight = 1f, radius = 1f };
            targets[1] = new CinemachineTargetGroup.Target { target = null, weight = 0f, radius = 1f };
        }

        if (target == null)
        {
            // 해제: 보스 대신 weight 0으로 비활성
            targets[1].target = null;
            targets[1].weight = 0f;

            if (groupCam != null) groupCam.Priority = 0;
            if (FreeLookCam != null) FreeLookCam.Priority = 20;
        }
        else
        {
            // 설정: 보스 교체
            targets[1].target = target;
            targets[1].weight = 1f;

            if (groupCam != null) groupCam.Priority = 20;
            if (FreeLookCam != null) FreeLookCam.Priority = 0;
        }

        // 갱신
        targetGroup.m_Targets = targets;
    }


    public bool HasTarget() => lockOnTarget != null;
    public Transform GetLockOnTarget() => lockOnTarget;

    // ===================== 카메라 흔들기 =========================
    public void Shake(float intensity, float time)
    {
        if (noise == null) return;

        noise.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }


    // ===================== 카메라 보정 회전 =====================
    public void RotateTowardsTarget()
    {
    }


    // ===================== 카메라 입력 잠금 =====================
    public void SetCameraInputEnabled(bool enabled)
    {
        if (FreeLookCam == null) return;
        // X, Y 축 입력 이름으로 받는 경우
        if (enabled)
        {
            FreeLookCam.m_XAxis.m_InputAxisName = "Mouse X"; // 원래 입력 축 이름
            FreeLookCam.m_YAxis.m_InputAxisName = "Mouse Y";
        }
        else
        {
            FreeLookCam.m_XAxis.m_InputAxisName = ""; // 빈 문자열로 입력 끊기
            FreeLookCam.m_YAxis.m_InputAxisName = "";
        }
        // 만약 다른 방식(Input System 직접 제어)이라면 아래처럼도 가능
        // FreeLook.m_XAxis.m_InputAxisValue = 0f;
        // FreeLook.m_YAxis.m_InputAxisValue = 0f;
    }
}