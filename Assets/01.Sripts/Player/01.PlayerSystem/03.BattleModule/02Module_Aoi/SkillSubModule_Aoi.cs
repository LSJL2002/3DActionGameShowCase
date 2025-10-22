using System;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillSubModule_Aoi
{
    public event Action OnSkillEnd;

    private PlayerStateMachine sm;

    public SkillSubModule_Aoi(PlayerStateMachine sm) => this.sm = sm;

    private bool isFiring = false;

    // 스킬 입력 시작
    public void OnSkillStart()
    {
        var player = sm.Player;
        player.Animator.CrossFade("Skill1", 0.1f);

        isFiring = false; // 초기화    

        // 2️⃣ 바로 공중으로 띄우기
        float heightOffset = 3f;  // 원하는 높이
        float duration = player.Animator.GetCurrentAnimatorStateInfo(0).length; // 전체 애니 길이로 설정
        player.ForceReceiver.BeginVerticalHoldImmediate(heightOffset, duration);
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
            // 애니메이션 40% 진행 시 공격 시작
            if (state.normalizedTime >= 0.4f && !isFiring)
            {
                isFiring = true;
                FireSkillAtLockOnTargetSeparated(player);
            }

            // 애니메이션 끝나면 스킬 종료
            if (state.normalizedTime >= 1f)
            {
                // 애니 끝나면 공중 유지 종료
                player.ForceReceiver.EndVerticalHold();

                OnSkillEnd?.Invoke();
            }
        }
    }

    public void OnEnemyHit(IDamageable target)
    {
        // 필요 시 적 피격 처리
    }

    // 1초 간격으로 5번 타격
    private void FireSkillAtLockOnTargetSeparated(PlayerCharacter player)
    {
        // FX 반복: 0.5초 간격, 총 5번
        FireSkillFX(player, 5, 0.5f).Forget();

        // 데미지 반복: 0.5초 간격, 총 20번
        FireSkillDamage(player, 15, 0.3f, 1f).Forget();
    }

    // FX 반복
    private async UniTaskVoid FireSkillFX(PlayerCharacter player, int fxCount, float interval)
    {
        for (int i = 0; i < fxCount; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-15f, 15f),
                0f,
                Random.Range(-15f, 15f)
            );
            Vector3 fxPos = player.transform.position + randomOffset;

            player.skill.SpawnSkill("Skill11", fxPos);

            await UniTask.Delay(TimeSpan.FromSeconds(interval));
        }
    }

    // 데미지 반복
    private async UniTaskVoid FireSkillDamage(PlayerCharacter player, int hitCount, float interval, float damageMultiplier)
    {
        for (int i = 0; i < hitCount; i++)
        {
            var targetObj = player._camera.GetLockOnTarget();
            if (targetObj != null && targetObj.TryGetComponent<IDamageable>(out var dmg))
            {
                // 몬스터 최신 위치 기준 랜덤 오프셋
                Vector3 randomOffset = new Vector3(
                    Random.Range(-1f, 1f), // 몬스터 주변 범위 설정
                    0f,
                    Random.Range(-1f, 1f)
                );
                Vector3 hitPos = targetObj.position + randomOffset;

                // 데미지 처리
                player.Attack.HandleHit(dmg, hitPos, damageMultiplier);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(interval));
        }
    }
}