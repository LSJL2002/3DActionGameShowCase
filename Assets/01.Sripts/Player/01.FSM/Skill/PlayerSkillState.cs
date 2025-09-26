using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerSkillState : PlayerBaseState
{
    private Transform attackTarget;

    private float forwardDashPower = 3f;   // 앞으로 힘
    private float returnDashPower = 3f;    // 뒤로 힘
    private float dashDuration = 0.2f;     // 힘 적용 시간
    private float waitTime = 0.5f;         // 뒤로 돌아오기까지 대기

    private float dashTimer = 0f;
    private float waitTimer = 0f;
    private bool isReturning = false;
    private Vector3 dashDirection;

    public PlayerSkillState(PlayerStateMachine sm) : base(sm) { }

    public override bool AllowMovement => false; // 스킬 중 이동 제한
    public override bool AllowRotation => false;


    public override void Enter()
    {
        base.Enter();

        // 가장 가까운 몬스터 탐색
        attackTarget = FindNearestMonster(stateMachine.Player.InfoData.AttackData.AttackRange, true);
        stateMachine.Player.Combat.SetAttackTarget(attackTarget);
        // 공격 진입 시 Lock-On 강제 적용
        if (attackTarget != null) stateMachine.Player.camera.ToggleLockOnTarget(attackTarget);

        var anim = stateMachine.Player.AnimationData;
        StartAnimation(anim.SkillBoolHash);
        stateMachine.Player.Animator.SetTrigger(anim.SkillTriggerHash);

        // 파티클 (VFXManager는 파티클만 재생)
        stateMachine.Player.vFX.StartDash();

        dashTimer = 0f;
        waitTimer = 0f;
        isReturning = false;

        // 이동 방향 설정
        dashDirection = attackTarget != null
            ? (attackTarget.position - stateMachine.Player.transform.position).normalized
            : stateMachine.Player.transform.forward;
        dashDirection.y = 0f;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.SkillBoolHash);

        stateMachine.Player.vFX.StopDash();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (attackTarget != null)
        {
            // 부드럽게 타겟 바라보기
            Vector3 lookDir = (attackTarget.position - stateMachine.Player.transform.position).normalized;
            lookDir.y = 0;
            if (lookDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                stateMachine.Player.transform.rotation =
                    Quaternion.Slerp(stateMachine.Player.transform.rotation, targetRot, Time.deltaTime * 10f);
            }
        }

        // 앞으로 / 뒤로 힘 적용
        if (!isReturning)
        {
            if (attackTarget != null)
            {
                Vector3 toTarget = attackTarget.position - stateMachine.Player.transform.position;
                toTarget.y = 0;
                float distance = toTarget.magnitude;

                if (distance > 0.1f)
                {
                    float moveAmount = Mathf.Min(forwardDashPower * Time.deltaTime, distance);
                    stateMachine.Player.ForceReceiver.AddForce(dashDirection * moveAmount, true);
                }
                else
                {
                    // 타겟 도달 → 대기 시작
                    waitTimer += Time.deltaTime;
                    if (waitTimer >= waitTime)
                    {
                        isReturning = true;
                        dashTimer = 0f;
                    }
                }
            }
            else
            {
                // 타겟 없으면 그냥 forward 방향으로 dashDuration 동안 이동
                if (dashTimer < dashDuration)
                {
                    dashTimer += Time.deltaTime;
                    stateMachine.Player.ForceReceiver.AddForce(dashDirection * forwardDashPower, true);
                }
                else
                {
                    waitTimer += Time.deltaTime;
                    if (waitTimer >= waitTime)
                    {
                        isReturning = true;
                        dashTimer = 0f;
                    }
                }
            }
        }
        else
        {
            // 뒤로 돌아오기
            if (dashTimer < dashDuration)
            {
                dashTimer += Time.deltaTime;
                stateMachine.Player.ForceReceiver.AddForce(-dashDirection * returnDashPower, true);
            }
        }


        // ForceReceiver 적용
        ForceMove();


        if (GetNormalizeTime(stateMachine.Player.Animator, "Skill") >= 0.9f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
