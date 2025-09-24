using UnityEngine;

public class CompanionIdleState : ICompanionState
{
    readonly CompanionStateMachine sm;
    CompanionController Ctx => sm.Ctx;

    public CompanionIdleState(CompanionStateMachine sm) { this.sm = sm; }

    public void Enter()
    {
        Ctx.anim?.SetBool("isMove", false);
        if (Ctx.moveFx) Ctx.moveFx.SetActive(false);
    }

    public void Exit() { }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.G)) sm.ChangeState(new CompanionTalkState(sm));

        // 거리/플레이어 움직임 감지해 Follow로 전환 원하면 여기 조건 추가
        // if ( ... ) sm.ChangeState(new CompanionFollowState(sm));
    }

    public void Update()
    {
        // 대화/전투 중엔 전환 금지
        if (Ctx.isTalkMode || Ctx.isAttack) return;

        // 참조 체크
        if (Ctx.targetObject == null || Ctx.rb == null) return;

        // Idle → Follow 전환 조건:
        Vector3 toTarget = Ctx.targetObject.position - Ctx.rb.position;

        if (toTarget.sqrMagnitude > 0.36f)
        {
            sm.ChangeState(new CompanionFollowState(sm));
        }
    }
    public void PhysicsUpdate() { }
}
