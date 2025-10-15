using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EventManager : MonoBehaviour
{
    private PlayerCharacter player;
    private ForceReceiver force;
    private Transform playerTransform;

    [Header("타겟 뒤 이동 설정")]
    public float sideOffset = 1.5f;
    public float behindOffset = 3f;
    public float moveDuration = 0.25f;
    public int pathPoints = 5;


    public void Initialize(PlayerCharacter player, Transform body, ForceReceiver forceReceiver)
    {
        this.player = player;
        playerTransform = body;
        force = forceReceiver;
    }

    // ==================== 타겟 뒤 이동 =====================
    public void MoveBehindTarget(Transform target)
    {
        if (target == null || force == null) return;

        Vector3 startPos = playerTransform.position;

        // 타겟 뒤 방향 계산
        Vector3 rawDir = target.position - startPos;
        rawDir.y = 0f;
        Vector3 playerToTarget = rawDir.normalized;

        Vector3 targetBehind = target.position + playerToTarget * behindOffset;
        targetBehind.y = startPos.y;

        Vector3 right = Vector3.Cross(Vector3.up, playerToTarget).normalized;

        Vector3[] path = new Vector3[pathPoints + 2];
        path[0] = startPos;
        for (int i = 1; i <= pathPoints; i++)
        {
            float t = (float)i / (pathPoints + 1);
            Vector3 point = Vector3.Lerp(startPos, targetBehind, t);
            point += right * Mathf.Sin(t * Mathf.PI) * sideOffset;
            point.y = startPos.y;
            path[i] = point;
        }
        path[path.Length - 1] = targetBehind;

        float elapsed = 0f;
        DOTween.To(() => elapsed, x => elapsed = x, 1f, moveDuration)
            .SetEase(Ease.OutQuad)
            .OnUpdate(() =>
            {
                // 경로 계산
                float t = elapsed;
                Vector3 newPos = CatmullRomPath(path, t);
                // 이동 delta 계산
                Vector3 delta = newPos - playerTransform.position;
                // ForceReceiver에 힘 추가
                force.AddForce(delta, horizontalOnly: true);

                // 타겟 바라보기
                Vector3 lookDir = target.position - playerTransform.position;
                lookDir.y = 0f;
                if (lookDir != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDir, Vector3.up);
                    playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, 0.2f);
                }
            })
            .OnComplete(() =>
            {
                // 최종 위치에서 타겟 바라보기
                Vector3 lookDir = target.position - playerTransform.position;
                lookDir.y = 0f;
                if (lookDir != Vector3.zero)
                    playerTransform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
            });
    }

    private Vector3 CatmullRomPath(Vector3[] points, float t)
    {
        int numSections = points.Length - 3;
        int currPt = Mathf.Min(Mathf.FloorToInt(t * numSections), numSections - 1);
        float u = t * numSections - currPt;

        Vector3 a = points[currPt];
        Vector3 b = points[currPt + 1];
        Vector3 c = points[currPt + 2];
        Vector3 d = points[currPt + 3];

        float u2 = u * u;
        float u3 = u2 * u;

        return 0.5f * ((2f * b) +
                       (-a + c) * u +
                       (2f * a - 5f * b + 4f * c - d) * u2 +
                       (-a + 3f * b - 3f * c + d) * u3);
    }

    // ================ 각성 공격 막타 이동 =================
    public void OnAwakenAttackStepMove()
    {
        Vector2 input = player.StateMachine.MovementInput; // 현재 방향키 입력
        Vector3 moveDir;
        if (input == Vector2.zero)
        {
            // 입력 없으면 뒤로 이동
            moveDir = -player.transform.forward;
        }
        else
        {
            moveDir = (player.transform.forward * input.y +
                       player.transform.right * input.x).normalized;
        }

        player.ForceReceiver?.AddForce(moveDir * 25f, horizontalOnly: true);
    }
}
