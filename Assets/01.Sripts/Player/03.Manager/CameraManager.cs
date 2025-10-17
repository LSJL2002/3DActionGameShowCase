using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    [field: SerializeField] public Transform MainCamera { get; private set; }
    [field: SerializeField] public Volume Volume_Main { get; private set; }
    [field: SerializeField] public CinemachineCamera FreeLookCam { get; private set; }
    private CinemachineInputAxisController inputAxisController;
    [field: SerializeField] public CinemachineTargetGroup TargetGroup { get; private set; }
    [field: SerializeField] public CinemachineCamera LockOnCam { get; private set; }
    public CinemachineBasicMultiChannelPerlin Noise { get; private set; }
    private float shakeTimer;

    public Volume Volume_Blur { get; private set; }

    private ColorAdjustments colorAdjustments;



    private Transform player; // 기본 바닦임
    private Transform playerFace;
    private Transform lockOnTarget;

    public Transform GetLockOnTarget() => lockOnTarget;


    private void Awake()
    {
        MainCamera = Camera.main?.transform;
        Volume_Blur = MainCamera.gameObject.GetComponent<Volume>();

        Noise = LockOnCam.GetComponent<CinemachineBasicMultiChannelPerlin>();

        if (Volume_Main != null && Volume_Main.profile != null)
            Volume_Main.profile.TryGet(out colorAdjustments);
    }


    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
                Noise.AmplitudeGain = 0f;
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
        // === LockOn 카메라 타깃 설정 ===
        if (LockOnCam != null)
        {
            LockOnCam.Follow = body;  // 이동 기준 → Body
        }
        // TargetGroup의 플레이어 타겟 갱신
        if (TargetGroup != null)
        {
            var targets = TargetGroup.Targets;
            // targets가 비어있으면 2 슬롯 확보
            if (targets == null || targets.Count == 0)
            {
                targets = new List<CinemachineTargetGroup.Target>
        {
            new CinemachineTargetGroup.Target { Object = face, Weight = 1f, Radius = 1f }, // 플레이어
            new CinemachineTargetGroup.Target { Object = null, Weight = 0f, Radius = 0f }  // 락온 슬롯
        };
            }
            else
            {
                // 0번 슬롯을 플레이어 얼굴로 갱신
                var t0 = targets[0];
                t0.Object = face;
                t0.Weight = 1f;
                t0.Radius = 1f;
                targets[0] = t0;
            }

            TargetGroup.Targets = targets; // List 재할당        
        }
    }

    // ======================== 카메라 Lock-On ========================
    public void ToggleLockOnTarget(Transform target)
    {
        if (TargetGroup == null) return;

        lockOnTarget = target;
        var targets = TargetGroup.Targets;

        if (targets == null || targets.Count < 2)
        {
            targets = new List<CinemachineTargetGroup.Target>
            {
                new CinemachineTargetGroup.Target { Object = playerFace, Weight = 1f, Radius = 1f },
                new CinemachineTargetGroup.Target { Object = null, Weight = 0f, Radius = 0f }
            };
        }

        // 1번 슬롯 락온 대상 갱신
        var t1 = targets[1];
        t1.Object = target;
        t1.Weight = target ? 1f : 0f;
        t1.Radius = target ? 1f : 0f;
        targets[1] = t1;

        TargetGroup.Targets = targets;

        // 카메라 우선순위 대신 enabled로 전환
        if (target == null)
        {
            if (LockOnCam != null) LockOnCam.enabled = false;
            if (FreeLookCam != null) FreeLookCam.enabled = true;
        }
        else
        {
            if (LockOnCam != null) LockOnCam.enabled = true;
            if (FreeLookCam != null) FreeLookCam.enabled = false;
        }
    }

    // ===================== 카메라 흔들기 =========================
    public void Shake(float intensity, float time)
    {
        if (Noise == null) return;

        Noise.AmplitudeGain = intensity;
        shakeTimer = time;
    }


    // ===================== 카메라 보정 회전 =====================
    public void RotateTowardsTarget()
    {
    }


    // ===================== 카메라 입력 잠금 =====================
    public void SetCameraInputEnabled(bool enabled)
    {
        if (inputAxisController != null)
            inputAxisController.enabled = enabled;
    }

    // =================== Visual Postprocess =================
    // Color Grading 켜기/끄기
    public void SetColorGradingEnabled(bool enabled)
    {
        if (colorAdjustments == null) return;

        colorAdjustments.active = enabled;
    }
}