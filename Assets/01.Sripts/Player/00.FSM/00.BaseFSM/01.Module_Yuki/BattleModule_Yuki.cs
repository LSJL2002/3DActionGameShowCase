using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;


public class BattleModule_Yuki : BattleModule
{
    private ComboSubModule_Yuki comboSub;
    private SkillSubModule_Yuki skillSub;
    private AwakenSubModule_Yuki awakenSub;

    public bool IsAwakened => awakenSub.IsAwakened;

    public BattleModule_Yuki(PlayerStateMachine sm) : base(sm)
    {
        comboSub = new ComboSubModule_Yuki(sm);
        skillSub = new SkillSubModule_Yuki(sm);
        awakenSub = new AwakenSubModule_Yuki(sm);

        // 초기 콤보 핸들러 연결
        comboHandler = comboSub.ComboHandler;

        // 하위 모듈의 이벤트를 받아서 자신 이벤트로 중계
        skillSub.OnSkillEnd += RaiseSkillEnd;
        comboSub.OnComboEnd += RaiseAttackEnd;
        // 각성 종료 시 콤보 초기화 + AttackEnd 이벤트 전달
        awakenSub.OnAwakenEnd += () =>
        {
            comboSub.ResetCombo();   // 콤보 상태 초기화
            RaiseAttackEnd();        // FSM에 공격 종료 알림
        };
    }

    // ====================== UI 접근용 프로퍼티 ======================

    /// <summary>
    /// 현재 각성 상태 여부
    /// </summary>
    public bool IsAwakened => awakenSub.IsAwakened;

    /// <summary>
    /// 현재 각성 게이지 (0~MaxAwakenGauge)
    /// </summary>
    public float AwakenGauge => sm.Player.Stats.AwakenGauge;

    /// <summary>
    /// 최대 각성 게이지
    /// </summary>
    public float MaxAwakenGauge => sm.Player.Stats.MaxAwakenGauge;

    // ================== 기본 공격 입력 =================
    public override void OnAttack()
    {
        comboSub.SetAwakened(awakenSub.IsAwakened);
        comboSub.OnAttack();
        awakenSub.CheckAwakenHoldStart();
    }

    public override void OnAttackCanceled()
    {
        comboSub.OnAttackCanceled();
        awakenSub.OnAttackCanceled();
    }

    // === 스킬 ===
    public override void OnSkill() => skillSub.OnSkill();
    public override void OnSkillCanceled() => skillSub.OnSkillCanceled();


    // === 업데이트 ===
    public override void OnUpdate()
    {
        comboSub.OnUpdate();
        awakenSub.OnUpdate();
    }
    public override void OnSkillUpdate() => skillSub.OnSkillUpdate();


    // === 기타 ===
    public override void OnEnemyHit(IDamageable target)
    {
        comboSub.OnEnemyHit(target);
        awakenSub.OnEnemyHit(target);
    }

    public override void ResetCombo() => comboSub.ResetCombo();
}