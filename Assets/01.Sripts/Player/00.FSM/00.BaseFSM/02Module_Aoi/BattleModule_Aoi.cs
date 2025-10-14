using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleModule_Aoi : BattleModule
{
    private float charge = 0f;
    private float maxCharge = 100f;

    public BattleModule_Aoi(PlayerStateMachine sm) : base(sm) { }

    public override void OnAttack()
    {
        // 원거리 기본 공격
        sm.Player.skill.SpawnSkill("BasicShot", sm.Player.Body.position);
        charge += 10f;
    }

    public override void OnSkill()
    {
        if (charge < maxCharge) return;
        sm.Player.skill.SpawnSkill("LaserBeam", sm.Player.Body.position);
        charge = 0f;
    }

    public override void OnUpdate()
    {
        // charge 게이지 UI 업데이트 등
    }
}