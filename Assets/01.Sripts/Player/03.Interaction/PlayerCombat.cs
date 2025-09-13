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

    // 공격 입력 시 호출 에니메이션 이벤트로 조작
    public void OnAttack(string skillName)
    {
        // 애니메이션 이벤트에서 호출
        var skillObj = SkillManagers.Instance.SpawnSkill(skillName, effectSpawnPoint);

        var skillHitbox = skillObj.GetComponentInChildren<Hitbox>();
        if (skillHitbox != null)
            skillHitbox.OnHit += HandleHit;

        var ps = skillObj.GetComponent<ParticleSystem>();
        float duration = ps != null ? ps.main.duration : 1.0f;
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
