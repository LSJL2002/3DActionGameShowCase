using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerBaseState
{
    private ComboHandler comboHandler;

    private Transform attackTarget;

    private PlayerAttackController attackCtrl;



    public PlayerAttackState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.AttackBoolHash);

        attackTarget = FindNearestMonster(stateMachine.Player.InfoData.AttackData.AttackRange, true);
        stateMachine.Player.Attack.SetAttackTarget(attackTarget);
        if (attackTarget != null) stateMachine.Player.camera.ToggleLockOnTarget(attackTarget);

        attackCtrl = stateMachine.Player.Attack;

        // PlayerManager에서 공격 정보 가져오기
        var attackInfos = stateMachine.Player.InfoData.AttackData.AttackInfoDatas;
        // ComboHandler 초기화
        comboHandler = new ComboHandler(attackInfos, stateMachine.Player.Animator, attackCtrl);

        comboHandler.RegisterInput();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackBoolHash);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        var currentAttack = comboHandler.Update(); // 현재 공격 단계 + 발동
        if (currentAttack == null && !comboHandler.IsActive)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        base.OnAttackStarted(context);
        comboHandler.RegisterInput();
    }

    protected override void OnAttackCanceled(InputAction.CallbackContext context)
    {
        base.OnAttackCanceled(context);
    }

    protected override void OnDodgeStarted(InputAction.CallbackContext context)
    {
        base.OnDodgeStarted(context);
        float normalizedTime = GetNormalizeTime(stateMachine.Player.Animator, "Attack");
        if (normalizedTime >= 0.1f)
        {
            stateMachine.ChangeState(stateMachine.DodgeState);
        }
    }
}