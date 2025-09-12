using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletMonster : BaseMonster
{
    public void OnSwingEnd()
    {
        Debug.Log("Testing if animation event works");
        stateMachine.isAttacking = false;

        //stateMachine.Monster.Stats.MonsterSkills.
    }
}
