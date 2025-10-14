using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerSkillState : PlayerBaseState
{
    private BattleModule module;

    private Transform attackTarget;


    public PlayerSkillState(PlayerStateMachine sm) : base(sm) { }

    public override bool AllowMovement => false; // 스킬 중 이동 제한
    public override bool AllowRotation => false;


    public override void Enter()
    {
        base.Enter();
        var anim = stateMachine.Player.AnimationData;
        StartAnimation(anim.SkillBoolHash);

        stateMachine.IsSkill = true;

        attackTarget = FindNearestMonster(stateMachine.Player.InfoData.AttackData.AttackRange, true);
        stateMachine.Player.Attack.SetAttackTarget(attackTarget);
        if (attackTarget != null) stateMachine.Player.camera.ToggleLockOnTarget(attackTarget);

        module = stateMachine.CurrentBattleModule;
        if (module != null) module.OnSkillEnd += HandleSkillEnd;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.SkillBoolHash);

        stateMachine.IsSkill = false;

        if (module != null) module.OnSkillEnd -= HandleSkillEnd;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    private void HandleSkillEnd()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }
}