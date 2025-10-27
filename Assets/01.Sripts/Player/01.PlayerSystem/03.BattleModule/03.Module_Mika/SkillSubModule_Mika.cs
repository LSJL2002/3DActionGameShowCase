using System;
using UnityEngine;

public class SkillSubModule_Mika
{
    public event Action OnSkillEnd;

    private PlayerStateMachine sm;

    private ComboSubModule_Mika comboSub;

    public SkillSubModule_Mika(PlayerStateMachine sm, ComboSubModule_Mika comboSub)
    {
        this.sm = sm;
        this.comboSub = comboSub;
    }

    // 스킬 입력 시작
    public void OnSkillStart()
    {
        var comboIndex = comboSub.CurrentComboIndex;

        string skillAnim = GetSkillAnimByCombo(comboIndex);
        sm.Player.Animator.CrossFade(skillAnim, 0.1f);

        var skillData = sm.Player.InfoData.SkillData.GetSkillInfoData(0);
        sm.Player.Attack.OnAttack(skillData.SkillName, skillData.HitCount, skillData.Interval, skillData.DamageMultiplier);
    }

    // 스킬 입력 종료
    public void OnSkillCanceled()
    {
        OnSkillEnd?.Invoke();
    }

    // 매 프레임 호출
    public void OnSkillUpdate()
    {
        var anim = sm.Player.Animator;
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Skill1") || state.IsName("Skill2") || state.IsName("Skill3") || state.IsName("Skill4"))
        {
            if (state.normalizedTime >= 1f)
            {
                OnSkillEnd?.Invoke();
            }
        }
    }

    public void OnEnemyHit(IDamageable target)
    {
        // 필요 시 적 피격 처리
    }

    private string GetSkillAnimByCombo(int comboIndex)
    {
        // 예시: 2타->Skill1, 3타->Skill2, 4타->Skill3, 6타->Skill4
        return comboIndex switch
        {
            2 => "Skill1",
            3 => "Skill2",
            4 => "Skill3",
            6 => "Skill4",
            _ => "Skill1"
        };
    }
}