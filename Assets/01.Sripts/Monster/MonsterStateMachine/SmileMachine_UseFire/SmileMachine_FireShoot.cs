using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileMachine_FireShoot : MonsterBaseState
{
    private MonsterSkillSO skillData;
    public SmileMachine_FireShoot(MonsterStateMachine ms, MonsterSkillSO fireShootSkill) : base(ms)
    {
        skillData = fireShootSkill;
    }
}
