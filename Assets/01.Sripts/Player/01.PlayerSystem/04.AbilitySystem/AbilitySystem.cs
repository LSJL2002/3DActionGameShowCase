using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class AbilitySystem : MonoBehaviour
{
    // 다른 오브젝트나 테스트 환경에서도 단독 실행 가능
    // 유지보수 시 PlayerCharacter가 너무 커지는 것을 방지
    private PlayerStateMachine sm;
    public PlayerAttribute Attr { get; private set; }
    public InputSystem Input { get; private set; }

    // ========== Actions ================
    public event Action OnDeath;

    // ========== 상태 플래그 ==========
    public bool IsWalking { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsDodging { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsUsingSkill { get; private set; }
    public bool IsKnockback { get; private set; }
    public bool IsStun { get; private set; }
    public bool IsDeath { get; private set; }
    public bool IsSwapping { get; private set; }
    public bool IsInvincible { get; private set; }

    public void Initialize(PlayerAttribute stats, PlayerStateMachine sm, InputSystem inputSystem)
    {
        Attr = stats;
        Attr.Resource.OnDie += ApplyDeath;
        this.sm = sm;
        Input = inputSystem;

        // === 입력 이벤트 연결 ===
        Input.OnMove += TryMove;
        Input.OnAttackStarted += TryAttack;
        Input.OnAttackCanceled += CancelAttack;
        Input.OnDodge += TryDodge;
        Input.OnJump += TryJump;
        Input.OnSkillStarted += TrySkill;
        Input.OnSkillCanceled += CancelSkill;
        Input.OnSwapNext += () => TrySwap(true);  // E
        Input.OnSwapPrev += () => TrySwap(false); // Q

        sm.CurrentBattleModule?.Initialize(inputSystem);
    }

    private void OnDestroy()
    {
        if (Input == null) return;
        Input.OnMove -= TryMove;
        Input.OnAttackStarted -= TryAttack;
        Input.OnAttackCanceled -= CancelAttack;
        Input.OnDodge -= TryDodge;
        Input.OnJump -= TryJump;
        Input.OnSkillStarted -= TrySkill;
        Input.OnSkillCanceled -= CancelSkill;

        sm.CurrentBattleModule?.Dispose();
    }

    // =============== 상태 전환 로직 ===============
    private void TryMove(Vector2 moveInput)
    {
        if (BlockInput()) return;
        if (IsUsingSkill) return;
        if (IsAttacking) return;
        sm.MovementInput = moveInput;

        if (moveInput.sqrMagnitude > 0.01f && sm.CurrentState != sm.WalkState)
            sm.ChangeState(sm.WalkState);
        else if (moveInput.sqrMagnitude <= 0.01f && sm.CurrentState != sm.IdleState)
            sm.ChangeState(sm.IdleState);
    }

    private void TryJump()
    {
        if (IsStun || IsKnockback || IsDeath) return;  // 스턴, 넉백, 죽음 등
        sm.ChangeState(sm.JumpState);
    }

    private void TryDodge()
    {
        // 에니메이션 정규화 해야될지도? < 0.1f
        if (BlockInput()) return;
        if (!Attr.EvadeBuffer.Use()) return;

        sm.ChangeState(sm.DodgeState);
    }

    private void TryAttack()
    {
        if (BlockInput()) return;
        if (IsUsingSkill) return;

        // 이미 공격 상태라면 FSM 바꾸지 않고, 모듈로 입력만 전달
        if (IsAttacking)
        {
            sm.CurrentBattleModule?.OnAttackStart();
            sm.UpdateAttackTarget();
            return;
        }
        // 처음 공격 진입일 때
        sm.ChangeState(sm.AttackState);
        sm.CurrentBattleModule?.OnAttackStart();
        sm.UpdateAttackTarget();
    }
    private void CancelAttack() { }

    private void TrySkill()
    {
        if (BlockInput()) return;
        if (IsUsingSkill) return;
        if (IsAttacking) return;
        if (!Attr.SkillBuffer.Use()) return; // 리소스 체크 등

        sm.ChangeState(sm.SkillState);
        sm.CurrentBattleModule?.OnSkillStart();
        sm.UpdateAttackTarget();
    }

    private void CancelSkill() { }

    public void TrySwap(bool next)
    {
        if (BlockInput()) return;
        if (IsAttacking) return;
        if (IsUsingSkill) return;

        // 살아있는 캐릭터 수 체크
        if (sm.Player.PlayerManager.AliveCount <= 1) return;

        IsSwapping = true;

        if (next)
            sm.Player.PlayerManager.SwapNext();
        else
            sm.Player.PlayerManager.SwapPrev();
        sm.ChangeState(sm.SwapOutState);
    }

    public void FinishSwap()
    {
        IsSwapping = false;
        sm.ChangeState(sm.IdleState); // 스왑 완료 후 바로 Idle
    }

    public void StartSwapIn()
    {
        IsSwapping = true;
        sm.ChangeState(sm.SwapInState);
    }


    // ===================== 상태 제어 메서드 =====================
    public void ApplyKnockback(Vector3 dir, float force, float time)
    {
        if (IsInvincible) return;

        IsKnockback = true;
        sm.KnockbackState.Setup(dir, force, time);
        sm.ChangeState(sm.KnockbackState);
    }
    public void EndKnockback() => IsKnockback = false;

    public void ApplyStun(float duration)
    {
        if (IsInvincible) return;

        IsStun = true;
        sm.StunState.Setup(duration);
        sm.ChangeState(sm.StunState);
    }
    public void EndStun() => IsStun = false;
    public void ApplyDeath()
    {
        if (IsDeath) return;

        IsDeath = true;
        sm.ChangeState(sm.DeathState);
    }
    public void EndDeath()
    {
        OnDeath?.Invoke();
    }

    // 상태 초기화용
    public void ResetStateFlags()
    {
        IsDodging = false;
        IsAttacking = false;
        IsUsingSkill = false;
        IsKnockback = false;
        IsStun = false;
    }

    // =================== 조건 로직 ===================
    private bool BlockInput()
    {
        // 입력을 막는 모든 조건
        return IsStun || IsKnockback || IsDeath || IsJumping || IsDodging || IsSwapping;
    }

    public void StartDodge() { IsDodging = true; IsInvincible = true; }
    public void EndDodge() { IsDodging = false; IsInvincible = false; }
    public void StartJump() => IsJumping = true;
    public void EndJump() => IsJumping = false;
    public void StartAttack() => IsAttacking = true;
    public void EndAttack() => IsAttacking = false;
    public void StartSkill() => IsUsingSkill = true;
    public void EndSkill() => IsUsingSkill = false;
    public void Revive() => IsDeath = false;
}