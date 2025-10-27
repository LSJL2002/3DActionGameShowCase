using UnityEngine;

public class SpiderMachine_FangFall : MonsterBaseState
{
    private MonsterSkillSO skillData;
    public SpiderMachine_FangFall(MonsterStateMachine stateMachine, MonsterSkillSO fangFallSkill) : base(stateMachine)
    {
        skillData = fangFallSkill;
    }

    public override void Enter()
    {
        
    }
}
