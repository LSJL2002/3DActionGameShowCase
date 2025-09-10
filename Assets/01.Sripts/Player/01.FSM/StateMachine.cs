

public interface Istate
{
    public void Enter();
    public void Exit();
    public void HandleInput();
    public void LogicUpdate();
    public void PhysicsUpdate();
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

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()
    {

        currentState?.LogicUpdate();
    }

    public void Physicsupdate()
    {
        currentState?.PhysicsUpdate();
    }
}