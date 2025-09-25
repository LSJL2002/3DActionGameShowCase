using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileMachineShootState : MonsterBaseState
{
    private MonsterSkillSO skillData;
    public SmileMachineShootState(MonsterStateMachine ms, MonsterSkillSO shootSkill) : base(ms)
    {
        skillData = shootSkill;
    }

    public override void Enter()
    {
        
    }
}
