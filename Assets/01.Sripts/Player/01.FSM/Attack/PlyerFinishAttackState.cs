using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlyerFinishAttackState : PlayerBaseState
{
    public PlyerFinishAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override PlayerStateID StateID => throw new System.NotImplementedException();


    public override void Enter()
    {
        base.Enter();
        var anim = stateMachine.Player.Animator;
        anim.SetTrigger(stateMachine.Player.AnimationData.FinishAttackHash);
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.ComboIndex = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // 이동 입력 들어오면 바로 이동 상태
        if (stateMachine.MovementInput.sqrMagnitude > 0.01f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        // 애니 끝나면 Idle로
        float normalizedTime = GetNormalizeTime(stateMachine.Player.Animator, "FinishAttack");
        if (normalizedTime >= 1f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    // 공격 입력 처리
    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        // 무기 집어넣기 중이더라도 공격 입력 → 바로 Attack 상태 진입
        stateMachine.ComboIndex = 0;
        stateMachine.ChangeState(stateMachine.AttackState);
    }
}
