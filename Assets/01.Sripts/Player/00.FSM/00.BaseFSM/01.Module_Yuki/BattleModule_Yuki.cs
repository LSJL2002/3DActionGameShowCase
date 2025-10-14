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

    public BattleModule_Yuki(PlayerStateMachine sm) : base(sm)
    {
        comboSub = new ComboSubModule_Yuki(sm);
        skillSub = new SkillSubModule_Yuki(sm);
        awakenSub = new AwakenSubModule_Yuki(sm);

        // 초기 콤보 핸들러 연결
        comboHandler = comboSub.ComboHandler;
    }

    // ================== 기본 공격 입력 =================
    public override void OnAttack()
    {
        if (awakenSub.IsAwakened)
            comboSub.SetAwakened(true);
        else
            comboSub.SetAwakened(false);

        comboSub.OnAttack();
        awakenSub.CheckAwakenHoldStart();
    }

    // ================== 공격 취소 ==================
    public override void OnAttackCanceled()
    {
        comboSub.OnAttackCanceled();
        awakenSub.OnAttackCanceled();
    }

    // ================== 스킬 입력 ==================
    public override void OnSkill()
    {
        skillSub.OnSkill();
    }

    // ============== 일반 업데이트 (항상 호출) =================
    public override void OnUpdate()
    {
        comboSub.OnUpdate();
        awakenSub.OnUpdate();
    }

    // ============== 스킬 중 업데이트 (스킬 상태일 때만) ==========
    public override void OnSkillUpdate()
    {
        skillSub.OnSkillUpdate();
    }

    // =========== 적 타격 시 (콤보/각성게이지 반영 등) ============
    public override void OnEnemyHit(IDamageable target)
    {
        comboSub.OnEnemyHit(target);
        awakenSub.OnEnemyHit(target);
    }

    // ===================== 콤보 리셋 ======================
    public override void ResetCombo()
    {
        comboSub.ResetCombo();
    }
}