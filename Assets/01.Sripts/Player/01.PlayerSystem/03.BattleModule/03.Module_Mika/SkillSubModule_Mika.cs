using System;
using UnityEngine;

public class SkillSubModule_Mika
{
    public event Action OnSkillEnd;

    private PlayerStateMachine sm;

    public SkillSubModule_Mika(PlayerStateMachine sm) => this.sm = sm;

    // 스킬 입력 시작
    public void OnSkillStart()
    {
        var player = sm.Player;
        player.Animator.CrossFade("Skill1", 0.1f);
    }

    // 스킬 입력 종료
    public void OnSkillCanceled()
    {
        OnSkillEnd?.Invoke();
    }

    // 매 프레임 호출
    public void OnSkillUpdate()
    {
        var player = sm.Player;
        var anim = player.Animator;

        // 0번 레이어에서 현재 스킬 애니메이션 상태 확인
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        // 애니메이션 이름과 재생 완료 체크
        if (state.IsName("Skill1"))
        {
        }
    }

    public void OnEnemyHit(IDamageable target)
    {
        // 필요 시 적 피격 처리
    }
}