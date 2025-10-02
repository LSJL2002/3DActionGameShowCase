// ICompanionState.cs
public interface ICompanionState
{
    void Enter();
    void Exit();
    void HandleInput();
    void Update();
    void PhysicsUpdate();
}

// CompanionStateMachine.cs
public class CompanionStateMachine
{
    public ICompanionState Current { get; private set; }
    public CompanionController Ctx { get; }

    public CompanionStateMachine(CompanionController ctx) => Ctx = ctx;

    public void ChangeState(ICompanionState next) { Current?.Exit(); Current = next; Current?.Enter(); }
    public void HandleInput() => Current?.HandleInput();
    public void Update() => Current?.Update();
    public void PhysicsUpdate() => Current?.PhysicsUpdate();
}

