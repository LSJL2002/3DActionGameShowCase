using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//전투관련 로직만
public class PlayerCombat : MonoBehaviour, IDamageable
{
    private PlayerManager player;

    [Header("Hitbox & Effects")]
    public Hitbox hitbox;
    public GameObject hitEffectPrefab;
    public Transform effectSpawnPoint; // 이펙트 생성 위치 지정

    private void Awake()
    {
        player ??= GetComponent<PlayerManager>();
        if (player == null)
        {
            Debug.LogError("PlayerManager가 없습니다!");
            return;
        }
        if (effectSpawnPoint == null)
        {
            Debug.LogWarning("EffectSpawnPoint가 지정되지 않았습니다. 기본값은 Player 위치 사용.");
            effectSpawnPoint = transform; // 안전장치
        }
    }

    public void OnTakeDamage(int damage)
    {
        player.Stats.TakeDamage(damage);

        Debug.Log($"플레이어 체력: {player.Stats.CurrentHealth}");
        if (hitEffectPrefab)
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
    }

    // 공격 입력 시 호출 예시
    public void PerformAttack(int damage)
    {
        // Hitbox 활성화
        hitbox.Enable(damage);

        // 공격 시 이펙트 생성
        if (hitEffectPrefab != null)
            Instantiate(hitEffectPrefab, effectSpawnPoint.position, Quaternion.identity);

        // 공격 모션 끝나면 Disable 호출
        // 애니메이션 이벤트에서 weaponHitbox.Disable() 호출 가능
    }

    // 공격 종료 시 호출
    public void EndAttack()
    {
        hitbox.Disable();
    }
}
