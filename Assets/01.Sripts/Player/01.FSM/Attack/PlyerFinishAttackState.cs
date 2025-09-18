using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlyerFinishAttackState : PlayerBaseState
{
    private float enterTime; // 상태 진입 시각 기록
    private float finishDelay = 2f; // 2초 대기

    public PlyerFinishAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override PlayerStateID StateID => throw new System.NotImplementedException();

    public override bool AllowRotation => false;


    public override void Enter()
    {
        base.Enter();

        enterTime = Time.time; // 상태 진입 시간 기록

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

        // 2초 뒤 Idle로 전환
        if (Time.time - enterTime >= finishDelay)
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
