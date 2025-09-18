/// <summary>
/// 상태 보관 + 전환 + 업데이트를 전담하는 작은 상태머신 헬퍼.
/// - 단일 책임(SRP)으로 Companion을 깔끔하게 유지
/// </summary>
public class PetStateMachine
{
    private PetState _current;

    public void Initialize(PetState start)
    {
        _current = start;
        _current.Enter();
    }

    public void Change(PetState next)
    {
        if (_current == next) return;
        _current?.Exit();
        _current = next;
        _current?.Enter();
    }

    public void TickUpdate(float dt) => _current?.Update(dt);
    public void TickFixedUpdate(float dt) => _current?.FixedUpdate(dt);
}
