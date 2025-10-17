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
        var anim = sm.Player.AnimationData;
        StartAnimation(anim.SkillBoolHash);

        sm.IsSkill = true;

        attackTarget = FindNearestMonster(sm.Player.InfoData.AttackData.AttackRange, true);
        if (attackTarget != null) sm.Player._camera.ToggleLockOnTarget(attackTarget);

        module = sm.CurrentBattleModule;
        if (module != null) module.OnSkillEnd += HandleSkillEnd;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(sm.Player.AnimationData.SkillBoolHash);

        sm.IsSkill = false;

        if (module != null) module.OnSkillEnd -= HandleSkillEnd;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    private void HandleSkillEnd()
    {
        sm.ChangeState(sm.IdleState);
    }
}