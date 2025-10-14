using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        stateMachine.IsSkill = true;

        // 가장 가까운 몬스터 탐색
        attackTarget = FindNearestMonster(stateMachine.Player.InfoData.AttackData.AttackRange, true);
        stateMachine.Player.Attack.SetAttackTarget(attackTarget);
        // 공격 진입 시 Lock-On 강제 적용
        if (attackTarget != null) stateMachine.Player.camera.ToggleLockOnTarget(attackTarget);

        var anim = stateMachine.Player.AnimationData;
        StartAnimation(anim.SkillBoolHash);
        stateMachine.Player.Animator.SetTrigger(anim.SkillTriggerHash);
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.IsSkill = false;

        StopAnimation(stateMachine.Player.AnimationData.SkillBoolHash);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        // 애니메이션 종료 시 Idle로 전환
        if (GetNormalizeTime(stateMachine.Player.Animator, "Skill") >= 0.99f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}