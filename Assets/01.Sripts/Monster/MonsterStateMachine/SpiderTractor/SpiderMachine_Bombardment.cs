using UnityEngine;

public class SpiderMachine_Bombardment : MonsterBaseState
{
    private MonsterSkillSO skillData;
    public SpiderMachine_Bombardment(MonsterStateMachine ms, MonsterSkillSO bombardmentSkill) : base(ms)
    {
        skillData = bombardmentSkill;
    }
}
