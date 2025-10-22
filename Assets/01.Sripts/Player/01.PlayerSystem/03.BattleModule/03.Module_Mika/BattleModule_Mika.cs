using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleModule_Mika : BattleModule
{
    private int comboIndex = 0;
    private float comboTimer = 0f;

    public BattleModule_Mika(PlayerStateMachine sm) : base(sm) { }

    public override void OnAttackStart()
    {
        comboIndex = (comboIndex + 1) % 3;
        sm.Player.Attack.OnAttack($"Combo{comboIndex + 1}", 1, 0.1f, 1f);
        comboTimer = 1.5f; // 콤보 유지 시간
    }

    public override void OnSkillStart()
    {
        // 현재 콤보 단계에 따라 다른 스킬 실행
        string skillName = comboIndex switch
        {
            0 => "Skill_After1",
            1 => "Skill_After2",
            2 => "Skill_After3",
            _ => "Skill_Default"
        };
        sm.Player.skill.SpawnSkill(skillName, sm.Player.Body.position);
    }

    public override void OnUpdate()
    {
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0) comboIndex = 0;
        }
    }
}