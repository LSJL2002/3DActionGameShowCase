using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingHit : MonoBehaviour, IDamageable
{
    public virtual void OnTakeDamage(int amount)
    {
        //Debug.Log("object is hit");
    }
    public void ApplyEffect(MonsterEffectType effectType, Vector3 sourcePosition, float effectValue = 0f, float duration = 0f)
    {
        // 플레이어게만 적용
    }
}
