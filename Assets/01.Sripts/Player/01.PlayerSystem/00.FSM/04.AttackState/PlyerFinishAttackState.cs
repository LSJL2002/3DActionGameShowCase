using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlyerFinishAttackState : PlayerBaseState
{
    private float enterTime; // 상태 진입 시각 기록
    private float finishDelay = 1f; // 2초 대기

    public PlyerFinishAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }


    public override bool AllowRotation => false;


    public override void Enter()
    {
        base.Enter();

        enterTime = Time.time; // 상태 진입 시간 기록

        var anim = sm.Player.Animator;
        anim.SetTrigger(sm.Player.AnimationData.FinishAttackHash);
    }

    public override void Exit()
    {
        base.Exit();
        sm.ComboIndex = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // 이동 입력 들어오면 바로 이동 상태
        if (sm.MovementInput.sqrMagnitude > 0.01f)
        {
            sm.ChangeState(sm.IdleState);
            return;
        }

        // 2초 뒤 Idle로 전환
        if (Time.time - enterTime >= finishDelay)
        {
            sm.ChangeState(sm.IdleState);
        }
    }
}
