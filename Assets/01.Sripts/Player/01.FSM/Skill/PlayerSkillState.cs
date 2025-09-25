using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : PlayerBaseState
{
    private Transform attackTarget;


    public PlayerSkillState(PlayerStateMachine sm) : base(sm) { }

    public override bool AllowMovement => false; // 스킬 중 이동 제한
    public override bool AllowRotation => false;


    public override void Enter()
    {
        base.Enter();

        // 가장 가까운 몬스터 탐색
        attackTarget = FindNearestMonster(stateMachine.Player.InfoData.AttackData.AttackRange);
        stateMachine.Player.Combat.SetAttackTarget(attackTarget);
        // 공격 진입 시 Lock-On 강제 적용
        if (attackTarget != null) stateMachine.Player.camera.ToggleLockOnTarget(attackTarget);


        var anim = stateMachine.Player.AnimationData;
        StartAnimation(anim.SkillBoolHash);
        stateMachine.Player.Animator.SetTrigger(anim.SkillTriggerHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.SkillBoolHash);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // 애니메이션이 Skill 태그를 가진 스테이트를 90% 이상 재생했으면 Idle로 전환
        float normalizedTime = GetNormalizeTime(
            stateMachine.Player.Animator,
            "Skill" // Animator에서 Skill 태그를 설정해둔 상태
        );

        if (normalizedTime >= 0.9f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
