using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum CharacterType
{
    Yuki,  // 1차 캐릭터: 각성 모드 + 근접평타 변환
    Aoi,   // 2차 캐릭터: 기본 원거리 + 게이지 기반 스킬
    Mika   // 3차 캐릭터: 콤보마다 스킬 발동
}

// =============== 인터페이스 ===============
public interface IBattleModule
{
    void OnAttack();                     // 공격 입력
    void OnSkill();                      // 스킬 입력
    void OnUpdate();                     // 매 프레임 업데이트
    void OnAttackCanceled();             // 공격 취소
    void ResetCombo();                   // 콤보 초기화
    void OnEnemyHit(IDamageable target); // 타격 성공 시 호출

    ComboHandler ComboHandler { get; }   // 외부 참조용
}

    // =============== 추상 기본 모듈 ===============
    public abstract class BattleModule : IBattleModule
{
    protected PlayerStateMachine sm;
    protected ComboHandler comboHandler;
    public ComboHandler ComboHandler => comboHandler; // 외부 참조용

    public BattleModule(PlayerStateMachine sm) => this.sm = sm;

    public virtual void OnAttack()
    {
        comboHandler?.RegisterInput();
    }
    public abstract void OnSkill();
    public virtual void OnUpdate()
    {
        comboHandler?.Update();
    }
    public virtual void OnAttackCanceled()
    {
        // 기본 취소 시 행동 없음
    }

    public virtual void ResetCombo()
    {
        comboHandler = null;
    }

    public virtual void OnEnemyHit(IDamageable target)
    {
        // 기본 동작: 히트하면 아무것도 안함
    }
}