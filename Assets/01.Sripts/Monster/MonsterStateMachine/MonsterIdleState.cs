using UnityEngine;

public class MonsterIdleState : MonsterBaseState
{
    public MonsterIdleState(MonsterStateMachine ms) : base(ms) { }

    public override void Enter()
    {
        StopMoving();
        stateMachine.Monster.Agent.updateRotation = false;
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
    }

    public override void PhysicsUpdate()
    {

        base.PhysicsUpdate();

        Transform player = stateMachine.Monster.PlayerTarget;
        if (player != null)
        {
            Vector3 direction = player.position - stateMachine.Monster.transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction);
                stateMachine.Monster.transform.rotation = Quaternion.Slerp(
                    stateMachine.Monster.transform.rotation,
                    targetRot,
                    Time.deltaTime * 5f
                );
            }
        }
    }

    public override void Exit()
    {
        stateMachine.Monster.Agent.updateRotation = true;
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
    }
}
