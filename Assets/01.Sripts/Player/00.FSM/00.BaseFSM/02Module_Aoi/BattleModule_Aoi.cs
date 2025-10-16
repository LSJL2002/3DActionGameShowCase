using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleModule_Aoi : BattleModule
{
    private ComboSubModule_Aoi comboSub;
    private SkillSubModule_Aoi skillSub;

    public BattleModule_Aoi(PlayerStateMachine sm) : base(sm)
    {
        comboSub = new ComboSubModule_Aoi(sm);
        skillSub = new SkillSubModule_Aoi(sm);

        // 콤보 핸들러 연결
        comboHandler = comboSub.ComboHandler;
        comboSub.OnComboEnd += RaiseAttackEnd;
        skillSub.OnSkillEnd += RaiseSkillEnd;
    }

    // ================== 기본 공격 =================
    public override void OnAttack() => comboSub.OnAttack();
    public override void OnAttackCanceled() => comboSub.OnAttackCanceled();

    // ================== 스킬 =================
    public override void OnSkill() => skillSub.OnSkillStart();
    public override void OnSkillCanceled() => skillSub.OnSkillCanceled();

    // ================== 업데이트 =================
    public override void OnUpdate()
    {
        comboSub.OnUpdate();
        skillSub.OnUpdate();
    }

    public override void OnSkillUpdate() => skillSub.OnSkillUpdate();

    // ================== 기타 =================
    public override void OnEnemyHit(IDamageable target)
    {
        comboSub.OnEnemyHit(target);
        skillSub.OnEnemyHit(target);
    }
    public override void ResetCombo() => comboSub.ResetCombo();
}