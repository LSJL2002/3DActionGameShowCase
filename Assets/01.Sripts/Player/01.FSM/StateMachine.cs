

using System;

public interface Istate
{
    public void Enter();
    public void Exit();
    public void HandleInput(); //키입력확인
    public void LogicUpdate(); //체력회복 등 논리
    public void PhysicsUpdate(); //물리계산
}



public abstract class StateMachine
{
    protected Istate currentState;


    public void ChangeState(Istate state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    public void HandleInput() => currentState?.HandleInput();
    public void LogicUpdate() => currentState?.LogicUpdate();
    public void Physicsupdate() => currentState?.PhysicsUpdate();
}