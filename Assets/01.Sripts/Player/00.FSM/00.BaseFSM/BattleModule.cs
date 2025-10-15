using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum CharacterType
{
    Yuki,  // 1차 캐릭터: 근접 + 각성 모드
    Aoi,   // 2차 캐릭터: 원거리 + 게이지 스킬
    Mika   // 3차 캐릭터: 콤보 기반 스킬
}

// =============== 인터페이스 ===============
public interface IBattleModule
{
    event Action OnAttackEnd;
    event Action OnSkillEnd;

    void OnAttack();                        // 기본 공격
    void OnAttackCanceled();                
    void OnUpdate();                  
    void OnSkill();                         // 스킬 입력
    void OnSkillCanceled();
    void OnSkillUpdate();                   
    void ResetCombo();                      // 콤보 초기화
    void OnEnemyHit(IDamageable target);    // 타격 성공 시 호출

    ComboHandler ComboHandler { get; }      // 외부 참조용
}

    // =============== 추상 기본 모듈 ===============
public abstract class BattleModule : IBattleModule
{
    protected PlayerStateMachine sm;
    protected ComboHandler comboHandler;
    public ComboHandler ComboHandler => comboHandler; // 외부 참조용

    public event Action OnAttackEnd;
    public event Action OnSkillEnd;

    public BattleModule(PlayerStateMachine sm) => this.sm = sm;

    protected void RaiseAttackEnd() => OnAttackEnd?.Invoke();
    protected void RaiseSkillEnd() => OnSkillEnd?.Invoke();
    // === 공격 ===
    public virtual void OnAttack() => comboHandler?.RegisterInput();
    public virtual void OnAttackCanceled() { }
    public virtual void OnUpdate() => comboHandler?.Update();

    // === 스킬 ===
    public abstract void OnSkill();
    public virtual void OnSkillCanceled() { }
    public virtual void OnSkillUpdate() { }

    // === 기타 ===
    public virtual void OnEnemyHit(IDamageable target) { }
    public virtual void ResetCombo() => comboHandler = null;
}