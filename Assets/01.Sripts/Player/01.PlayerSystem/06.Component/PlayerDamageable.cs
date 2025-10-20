using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


// 전투 대상 / 피해 처리 전용
public class PlayerDamageable : MonoBehaviour, IDamageable
{
    private PlayerCharacter player;


    private void Awake()
    {
        player ??= GetComponent<PlayerCharacter>();
    }


    // IDamageable 구현 예시 (플레이어가 맞았을 때)
    public void OnTakeDamage(int amount)
    {
        player?.Attr.Resource.TakeDamage(amount);
    }

    public void ApplyEffect(MonsterEffectType type, Vector3 sourcePos, float value = 0, float duration = 0)
    {
        var ability = player.Ability;
        Vector3 dir = (transform.position - sourcePos).normalized;

        switch (type)
        {
            case MonsterEffectType.Knockback:
                ability.ApplyKnockback(dir, value, duration);
                break;

            case MonsterEffectType.Groggy:
                ability.ApplyStun(duration);
                break;
        }
    }
}