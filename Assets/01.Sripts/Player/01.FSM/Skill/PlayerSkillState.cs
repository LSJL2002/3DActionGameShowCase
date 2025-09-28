using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerSkillState : PlayerBaseState
{
    private Transform attackTarget;

    [Header("Dash Settings")]
    private float stopDistance = 2f;     // 타겟 근접 거리
    private float dashPower = 12f;       // 돌진 속도(세게 줘야 확 보임)
    private float returnPower = 8f;      // 후퇴 속도
    private float dashDuration = 0.15f;  // 돌진 유지 시간
    private float returnDuration = 0.1f;// 후퇴 유지 시간
    private float waitTime = 0.8f;       // 도착 후 대기

    private Vector3 dashDir;
    private Vector3 returnDir;
    private float phaseTimer = 0f;

    private enum Phase { Dash, Wait, Return }
    private Phase phase;

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

        // Force 초기화
        stateMachine.Player.ForceReceiver.Reset();

        if (attackTarget != null)
        {
            dashDir = (attackTarget.position - stateMachine.Player.transform.position).normalized;
            dashDir.y = 0f;

            if (dashDir.sqrMagnitude > 0.01f)
                stateMachine.Player.transform.rotation = Quaternion.LookRotation(dashDir);

            returnDir = -dashDir;
            phase = Phase.Dash;   // 타겟 있으면 돌진부터
        }
        else
        {
            dashDir = Vector3.zero; // 돌진 없음
            returnDir = -stateMachine.Player.transform.forward;
            returnDir.y = 0f;
            phase = Phase.Wait;    // 타겟 없으면 바로 대기
        }

        phaseTimer = 0f;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.SkillBoolHash);

        stateMachine.Player.vFX.StopDash();

        // ForceReceiver 리셋
        stateMachine.Player.ForceReceiver.Reset();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        phaseTimer += Time.deltaTime;

        switch (phase)
        {
            case Phase.Dash:
                if (attackTarget != null)
                {
                    Vector3 toTarget = attackTarget.position - stateMachine.Player.transform.position;
                    toTarget.y = 0f;
                    float distance = toTarget.magnitude;

                    if (distance <= stopDistance)
                    {
                        // 타겟에 도달 → 즉시 대기
                        phase = Phase.Wait;
                        phaseTimer = 0f;
                        stateMachine.Player.ForceReceiver.Reset();
                        break;
                    }

                    // 이동량을 남은 거리까지만 적용
                    float moveDistance = dashPower * Time.deltaTime;
                    if (moveDistance > distance - stopDistance)
                        moveDistance = distance - stopDistance;

                    Vector3 dashStep = toTarget.normalized * moveDistance;
                    stateMachine.Player.ForceReceiver.AddForce(dashStep / Time.deltaTime);
                }
                else
                {
                    // 타겟 없음 → 바로 대기
                    phase = Phase.Wait;
                    phaseTimer = 0f;
                    stateMachine.Player.ForceReceiver.Reset();
                }

                // 시간 기반 안전 종료
                if (phaseTimer >= dashDuration)
                {
                    phase = Phase.Wait;
                    phaseTimer = 0f;
                    stateMachine.Player.ForceReceiver.Reset();
                }
                break;

            case Phase.Wait:
                // 🔹 대기시간 동안 완전 정지
                stateMachine.Player.ForceReceiver.Reset();
                if (phaseTimer >= waitTime)
                {
                    phase = Phase.Return;
                    phaseTimer = 0f;
                }
                break;

            case Phase.Return:
                // 🔹 타겟 유무와 관계없이 후퇴
                stateMachine.Player.ForceReceiver.AddForce(returnDir * returnPower);
                if (phaseTimer >= returnDuration)
                {
                    stateMachine.Player.ForceReceiver.Reset();
                }
                break;
        }


        // ForceReceiver → Controller.Move
        ForceMove();

        // 4️⃣ 애니메이션 종료 시 Idle로 전환
        if (GetNormalizeTime(stateMachine.Player.Animator, "Skill") >= 0.99f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
