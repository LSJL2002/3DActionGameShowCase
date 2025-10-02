using UnityEngine;

public class BattleState : ICompanionState
{
    readonly CompanionStateMachine sm;
    CompanionController Ctx => sm.Ctx;

    public BattleState(CompanionStateMachine sm) { this.sm = sm; }

    public void Enter()
    {
        Ctx.anim?.SetBool("isDance", true);
        Ctx.anim?.SetBool("isMove", false);
        if (Ctx.moveFx) Ctx.moveFx.SetActive(false);
    }

    public void Exit()
    {
        Ctx.anim?.SetBool("isDance", false);
        Ctx.anim?.SetBool("isMove", true);
    }

    public void HandleInput()
    {
        // 전투 해제되면 Follow로 복귀
        if (!Ctx.isAttack) sm.ChangeState(new CompanionFollowState(sm));
    }

    public void Update() { }
    public void PhysicsUpdate() { }
}
