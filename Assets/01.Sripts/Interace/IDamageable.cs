using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void OnTakeDamage(int amount);

    void ApplyEffect(MonsterEffectType effectType, Vector3 sourcePosition, float effectValue = 0f, float duration = 0f);
}
