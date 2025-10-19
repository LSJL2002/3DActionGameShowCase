using UnityEngine;

public class PlayerDeathState : Istate
{
    private readonly PlayerStateMachine stateMachine;

    public PlayerDeathState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void HandleInput()
    {
    }

    public void LogicUpdate()
    {
    }

    public void PhysicsUpdate()
    {
    }
}
