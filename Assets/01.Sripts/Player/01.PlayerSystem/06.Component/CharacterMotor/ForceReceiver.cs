using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 넉백 / 외부 힘 계산
public class ForceReceiver : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float drag = 0.3f;
    private Vector3 impact;             // 누적 힘
    private Vector3 dampingVelocity;    // SmoothDamp용

    // 외부에서 강제로 설정할 힘
    private Vector3 forcedForce = Vector3.zero;
    private bool useForcedForce = false;

    public Vector3 HorizontalVelocity => useForcedForce ? forcedForce : impact;

    // 매 프레임 감속 처리
    public void UpdateForce()
    {
        if (!useForcedForce)
        {
            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
        }
        else
        {
            // forcedForce는 그대로 유지
            impact = Vector3.zero;
        }
    }

    // 누적 힘, 감속 적용
    public void AddForce(Vector3 force)
    {
        force.y = 0;
        impact += force;
    }

    // 강제 힘, 감속 없음
    public void SetForce(Vector3 force)
    {
        forcedForce = force;
        useForcedForce = true;
    }

    // SetForce 해제, 다시 누적 힘 모드로
    public void ReleaseForce()
    {
        forcedForce = Vector3.zero;
        useForcedForce = false;
    }
}