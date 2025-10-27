using DG.Tweening;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EventManager : MonoBehaviour
{
    private PlayerCharacter player;
    private ForceReceiver force;
    private Transform playerpos;

    [Header("타겟 뒤 이동 설정")]
    public float sideOffset = 2f;
    public float behindOffset = 4f;
    public float moveDuration = 1f;  //진행속도

    [Header("Debug Path")]
    public bool drawPathGizmos = true;
    public int debugResolution = 20;

    // Bezier 컨트롤 포인트
    private Vector3 bezierP0, bezierP1, bezierP2, bezierP3;


    public void Initialize(PlayerCharacter player, Transform body, ForceReceiver forceReceiver)
    {
        this.player = player;
        playerpos = body;
        force = forceReceiver;
    }

    // ======================== Yuki ==========================
    // ==================== 타겟 뒤 이동 =====================
    public void MoveBehindWithPush(Transform target)
    {
        if (target == null || force == null) return;

        // ✅ PushBack 먼저 실행
        float pushDuration = 0.4f;
        float pushPower = 1f;
        float pushElapsed = 0f;

        DOTween.To(() => pushElapsed, x => pushElapsed = x, 1f, pushDuration)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                // 뒤로 밀기
                force.AddForce(-player.transform.forward * pushPower);
            })
            .OnComplete(() =>
            {
                force.ReleaseForce();
                StartBezierMove(target);
            });
    }

    private void StartBezierMove(Transform target)
    {
        Vector3 startPos = playerpos.position;
        bezierP0 = startPos;

        Vector3 toTarget = target.position - startPos;
        toTarget.y = 0;
        Vector3 dir = toTarget.normalized;

        // 목표 위치 (타겟 뒤)
        Vector3 targetPos = target.position + dir * behindOffset;
        targetPos.y = startPos.y;
        bezierP3 = targetPos;

        // 곡선 포인트 계산
        Vector3 right = Vector3.Cross(Vector3.up, dir).normalized;
        bezierP1 = startPos + right * sideOffset;
        bezierP2 = targetPos + right * sideOffset;

        float elapsed = 0f;
        DOTween.To(() => elapsed, x => elapsed = x, 1f, moveDuration)
            .SetEase(Ease.InQuad)
            .OnUpdate(() =>
            {
                Vector3 newPos = GizmoUtility.BezierPoint(bezierP0, bezierP1, bezierP2, bezierP3, elapsed);
                Vector3 delta = newPos - playerpos.position;
                delta.y = 0;
                player.Controller.Move(delta);

                // 회전
                Vector3 lookDir = target.position - playerpos.position;
                lookDir.y = 0f;
                if (lookDir != Vector3.zero)
                {
                    var rot = Quaternion.LookRotation(lookDir);
                    playerpos.rotation = Quaternion.Slerp(playerpos.rotation, rot, 0.2f);
                }
            })
            .OnComplete(() =>
            {
                force.ReleaseForce();
            });
    }

    // ================ 각성 공격 막타 이동 =================
    public void OnAwakenAttackStepMove()
    {
        Vector2 input = player.StateMachine.MovementInput;
        Vector3 moveDir =
            input == Vector2.zero
            ? -playerpos.transform.forward
            : (playerpos.transform.forward * input.y + playerpos.transform.right * input.x).normalized;
        force.AddForce(moveDir * 20f);
    }


    // ================== 디버그 Gizmo ==================
    private void OnDrawGizmos()
    {
        if (!drawPathGizmos) return;
        if (bezierP0 == Vector3.zero && bezierP3 == Vector3.zero) return;
        Gizmos.color = Color.green;
        GizmoUtility.DrawBezierCurve(bezierP0, bezierP1, bezierP2, bezierP3, debugResolution, true);
    }
}