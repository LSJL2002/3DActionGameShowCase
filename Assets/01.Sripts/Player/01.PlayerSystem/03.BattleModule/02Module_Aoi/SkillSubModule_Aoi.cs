using System;
using UnityEngine;

public class SkillSubModule_Aoi
{
    public event Action OnSkillEnd;

    private PlayerStateMachine sm;
    private bool isFiring;

    public SkillSubModule_Aoi(PlayerStateMachine sm)
    {
        this.sm = sm;
        isFiring = false;
    }

    // 스킬 입력 시작 (버튼 꾹 누름)
    public void OnSkillStart()
    {
        isFiring = true;
    }

    // 스킬 입력 종료 (버튼 뗌)
    public void OnSkillCanceled()
    {
        isFiring = false;
        OnSkillEnd?.Invoke();
    }

    // 매 프레임 호출
    public void OnUpdate()
    {
        if (isFiring)
        {
            FireLaser();
        }
    }

    public void OnSkillUpdate() { }

    public void OnEnemyHit(IDamageable target)
    {
        // 필요 시 적 피격 처리
    }

    private void FireLaser()
    {
        // 레이저 생성 로직
        // 예: sm.Player.skill.SpawnSkill("Laser", sm.Player.Body.position, sm.Player.Body.rotation);
    }
}
