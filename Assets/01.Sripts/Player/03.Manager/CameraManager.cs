using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    public Transform MainCamera { get; private set; }
    public Volume Volume { get; private set; }
    [field: SerializeField] public CinemachineFreeLook FreeLookCam { get; private set; }
    [field:SerializeField] public Volume VisualVolume { get; private set; }
    private ColorAdjustments colorAdjustments;

    [field: SerializeField] public CinemachineTargetGroup TargetGroup {  get; private set; }
    [field: SerializeField] public CinemachineVirtualCamera LockOnCam { get; private set; }
    public CinemachineBasicMultiChannelPerlin Noise {  get; private set; }
    private float shakeTimer;


    private Transform player; // 기본 바닦임
    private Transform playerFace;
    private Transform lockOnTarget;

    public Transform GetLockOnTarget() => lockOnTarget;


    private void Awake()
    {
        MainCamera = Camera.main.transform;
        Volume = MainCamera.gameObject.GetComponent<Volume>();

        Noise = LockOnCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (VisualVolume != null)
            VisualVolume.profile.TryGet(out colorAdjustments);
    }


    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f && Noise != null)
                Noise.m_AmplitudeGain = 0f;
        }
    }

    // ======================= 플레이어 타겟 설정 =========================
    public void SetPlayerTarget(Transform body, Transform face)
    {
        if (body == null || face == null) return;

        player = body;
        playerFace = face;

        // === FreeLook 카메라 타깃 변경 ===
        if (FreeLookCam != null)
        {
            FreeLookCam.Follow = body; // 이동 기준 (Body)
            FreeLookCam.LookAt = face; // 시선 기준 (Face)
        }

        // TargetGroup의 플레이어 타겟 갱신
        if (TargetGroup != null)
        {
            var targets = TargetGroup.m_Targets;

            if (targets.Length == 0)
                targets = new CinemachineTargetGroup.Target[2]; // 플레이어 + 락온 슬롯

            targets[0].target = face; // 시선 기준
            targets[0].weight = 1f;
            targets[0].radius = 1f;

            TargetGroup.m_Targets = targets;
        }
    }

    // ======================== 카메라 Lock-On ========================
    public void ToggleLockOnTarget(Transform target)
    {
        if (TargetGroup == null) return;

        lockOnTarget = target;
        var targets = TargetGroup.m_Targets;

        // 최소 2개의 타겟 슬롯 확보
        if (targets.Length < 2)
        {
            System.Array.Resize(ref targets, 2);
            targets[0] = new CinemachineTargetGroup.Target { target = playerFace, weight = 1f, radius = 1f };
            targets[1] = new CinemachineTargetGroup.Target();
        }

        if (target == null)
        {
            // 🔹 락온 해제
            targets[1].target = null;
            targets[1].weight = 0f;

            if (LockOnCam != null) LockOnCam.Priority = 0;
            if (FreeLookCam != null) FreeLookCam.Priority = 20;
        }
        else
        {
            // 🔹 락온 설정
            targets[1].target = target;
            targets[1].weight = 1f;

            if (LockOnCam != null) LockOnCam.Priority = 20;
            if (FreeLookCam != null) FreeLookCam.Priority = 0;
        }

        TargetGroup.m_Targets = targets;
    }

    // ===================== 카메라 흔들기 =========================
    public void Shake(float intensity, float time)
    {
        if (Noise == null) return;

        Noise.m_AmplitudeGain = intensity;
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

    // =================== Visual Postprocess =================
    // Color Grading 켜기/끄기
    public void SetColorGradingEnabled(bool enabled)
    {
        if (colorAdjustments == null) return;

        colorAdjustments.active = enabled;
    }
}