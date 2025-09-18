/// <summary>
/// 모든 상태가 상속하는 추상 베이스.
/// - Enter / Update / FixedUpdate / Exit 수명주기 제공
/// - Companion(컨텍스트)에 안전하게 접근할 수 있도록 보관
/// </summary>
public abstract class PetState
{
    protected readonly Companion comp;

    protected PetState(Companion comp) => this.comp = comp;

    public virtual void Enter() { }
    public virtual void Update(float dt) { }
    public virtual void FixedUpdate(float dt) { }
    public virtual void Exit() { }
}
