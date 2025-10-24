using UnityEngine;


public abstract class PlayerBaseState : Istate
{
    protected PlayerStateMachine sm;
    public PlayerBaseState(PlayerStateMachine sm) => this.sm = sm;

    public virtual void Enter() { }
    public virtual void Exit() { }
    protected void StartAnimation(int animatorHash) => sm.Player.Animator.SetBool(animatorHash, true);
    protected void StopAnimation(int animatorHash) => sm.Player.Animator.SetBool(animatorHash, false);

    public virtual void HandleInput() { }
    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() { }

    // ====================== 에니메이션 특정 태그 =============================
    protected float GetNormalizeTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        // 애니메이션 전환(Transition) 중이고, 다음 상태가 지정한 tag면 → 다음 애니메이션의 진행도 반환
        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
            return nextInfo.normalizedTime;
        // 전환 중이 아니고, 현재 상태가 지정한 tag면 → 현재 애니메이션의 진행도 반환
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
            return currentInfo.normalizedTime;
        // 태그랑 맞는 애니메이션이 없다면 0 반환
        else
            return 0f;
    }
}