using System;

public interface Istate
{
    void Enter();
    void Exit();
    void HandleInput(); //키입력확인
    void LogicUpdate(); //체력회복 등 논리
    void PhysicsUpdate(); //물리계산
}



public abstract class StateMachine
{
    protected Istate currentState;

    public void ChangeState(Istate state)
    {
        //if (currentState == state) return;
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    public void HandleInput() => currentState?.HandleInput();
    public void LogicUpdate() => currentState?.LogicUpdate();
    public void Physicsupdate() => currentState?.PhysicsUpdate();
}