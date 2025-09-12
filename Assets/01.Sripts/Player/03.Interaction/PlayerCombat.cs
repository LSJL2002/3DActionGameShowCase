using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//전투관련 로직만
public class PlayerCombat : MonoBehaviour, IDamageable
{
    private PlayerManager player;
    public Transform effectSpawnPoint;


    private void Awake()
    {
        player ??= GetComponent<PlayerManager>();

        if (effectSpawnPoint == null)
            effectSpawnPoint = transform;
    }

    // 공격 입력 시 호출 예시
    // 공격 입력 시 호출
    public void PerformAttack(string skillName)
    {
        // 풀에서 스킬 꺼내기 (소환 위치 = effectSpawnPoint)
        var skillObj = SkillManagers.Instance.SpawnSkill(skillName, effectSpawnPoint);

        // Hitbox 이벤트 연결
        var skillHitbox = skillObj.GetComponent<Hitbox>();
        if (skillHitbox != null)
            skillHitbox.OnHit += HandleHit;

        // 파티클 지속시간 가져오기
        var ps = skillObj.GetComponent<ParticleSystem>();
        float duration = ps != null ? ps.main.duration : 1.0f;

        // 일정 시간 후 반환
        StartCoroutine(ReturnAfterTime(skillName, skillObj, duration));
    }


    private IEnumerator ReturnAfterTime(string skillName, GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        var skillHitbox = obj.GetComponent<Hitbox>();
        if (skillHitbox != null)
            skillHitbox.OnHit -= HandleHit; // 구독 해제 (중복 방지)
    }

    private void HandleHit(IDamageable target)
    {
        Debug.Log(target);

        int damage = player?.Stats.Attack ?? 0;
        target.OnTakeDamage(damage);
    }

    // Hitbox 충돌 이벤트 처리


    // IDamageable 구현 예시 (플레이어가 맞았을 때)
    public void OnTakeDamage(int amount)
    {
        player?.Stats.TakeDamage(amount); // HP 변경은 Stats에서만
        // 피격 애니메이션, 넉백 등도 여기서 처리 가능
    }
}
