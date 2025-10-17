using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerBaseState
{
    private BattleModule module;

    private Transform attackTarget;


    public PlayerAttackState(PlayerStateMachine sm) : base(sm) { }

    public override bool AllowMovement => false; // 스킬 중 이동 제한
    public override bool AllowRotation => false;


    public override void Enter()
    {
        base.Enter();
        StartAnimation(sm.Player.AnimationData.AttackBoolHash);

        sm.IsAttacking = true;

        attackTarget = FindNearestMonster(sm.Player.InfoData.AttackData.AttackRange, true);
        if (attackTarget != null) sm.Player._camera.ToggleLockOnTarget(attackTarget);

        module = sm.CurrentBattleModule;
        if (module != null) module.OnAttackEnd += HandleAttackEnd;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(sm.Player.AnimationData.AttackBoolHash);

        sm.IsAttacking = false;

        if (module != null) module.OnAttackEnd -= HandleAttackEnd;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        module?.OnUpdate();
    }

    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        base.OnAttackStarted(context);

        module?.OnAttack();

        // 공격 시작 시점에만 타겟 갱신
        attackTarget = FindNearestMonster(sm.Player.InfoData.AttackData.AttackRange, true);
        if (attackTarget != null) sm.Player._camera.ToggleLockOnTarget(attackTarget);
    }

    protected override void OnAttackCanceled(InputAction.CallbackContext context)
    {
        base.OnAttackCanceled(context);

        if (sm.CurrentBattleModule is BattleModule_Yuki yuki)
            yuki.OnAttackCanceled();
    }

    protected override void OnDodgeStarted(InputAction.CallbackContext context)
    {
        base.OnDodgeStarted(context);
        float normalizedTime = GetNormalizeTime(sm.Player.Animator, "Attack");
        if (normalizedTime >= 0.1f)
        {
            sm.ChangeState(sm.DodgeState);
        }
    }

    private void HandleAttackEnd()
    {
        sm.ChangeState(sm.IdleState);
    }
}