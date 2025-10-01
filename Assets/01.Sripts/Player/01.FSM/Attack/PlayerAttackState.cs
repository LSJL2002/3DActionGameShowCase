using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerBaseState
{
    private ComboHandler comboHandler;

    private Transform attackTarget;



    public PlayerAttackState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.AttackBoolHash);
        // 가장 가까운 몬스터 탐색
        attackTarget = FindNearestMonster(stateMachine.Player.InfoData.AttackData.AttackRange, true);
        stateMachine.Player.Combat.SetAttackTarget(attackTarget);
        // 공격 진입 시 Lock-On 강제 적용
        if (attackTarget != null) stateMachine.Player.camera.ToggleLockOnTarget(attackTarget);


        // PlayerManager에서 공격 정보 가져오기
        var attackInfos = PlayerManager.Instance.InfoData.AttackData.AttackInfoDatas;
        // ComboHandler 초기화
        comboHandler = new ComboHandler(attackInfos, stateMachine.Player.Animator);

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

        // ComboHandler 업데이트 → 현재 공격 단계 반환
        var currentAttack = comboHandler.Update();

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
        // 공격 버튼 떼었을 때는 특별한 동작 없음
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