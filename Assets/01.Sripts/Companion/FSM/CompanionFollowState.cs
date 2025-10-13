using UnityEngine;

public class CompanionFollowState : ICompanionState
{
    readonly CompanionStateMachine sm;
    CompanionController Ctx => sm.Ctx;

    public CompanionFollowState(CompanionStateMachine sm) { this.sm = sm; }

    public void Enter()
    {
        Ctx.anim?.SetBool("isMove", true);
        if (Ctx.moveFx) Ctx.moveFx.SetActive(true);
    }

    public void Exit()
    {
        if (Ctx.moveFx) Ctx.moveFx.SetActive(false);
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            sm.ChangeState(new CompanionTalkState(sm));
            return;
        }
        if (Ctx.isAttack)
        {
            sm.ChangeState(new BattleState(sm));
            return;
        }
    }

    public void Update()
    {
        // 이동 중 애니 판정
        bool isMoving = Ctx.rb && Ctx.rb.linearVelocity.sqrMagnitude > Ctx.moveSpeedThreshold * Ctx.moveSpeedThreshold;
        Ctx.anim?.SetBool("isMove", isMoving);
        
        if (!Ctx.isTalkMode && !Ctx.isAttack && Ctx.targetObject && Ctx.rb)
        {
            Vector3 toTarget = Ctx.targetObject.position - Ctx.rb.position;
            if (toTarget.sqrMagnitude <= 0.25f) // 숫자는 감으로 조절
            {
                sm.ChangeState(new CompanionIdleState(sm));
            }
        }
    }

    public void PhysicsUpdate()
    {
        if (Ctx.targetObject == null || Ctx.rb == null || PlayerManager.Instance.stateMachine.IsAttacking)
            return;

        // 위치 이동
        Vector3 nextMove = Vector3.MoveTowards(Ctx.rb.position, Ctx.targetObject.position, Ctx.moveSpeed * Time.deltaTime);
        Ctx.rb.MovePosition(nextMove);

        // 회전
        Vector3 dir = (Ctx.lookObject.position - Ctx.rb.position).normalized;
        dir = Vector3.ProjectOnPlane(dir, Vector3.up);
        Quaternion look = Quaternion.LookRotation(dir, Vector3.up);
        Quaternion nextRot = Quaternion.RotateTowards(Ctx.rb.rotation, look, Ctx.rotationSpeed * Time.deltaTime);
        Ctx.rb.MoveRotation(nextRot);
    }
}
