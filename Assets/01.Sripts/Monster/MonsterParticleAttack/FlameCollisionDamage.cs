using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameCollisionDamage : MonoBehaviour
{
    private SmileMachine_UseFire ownerMonster;

    public void Init(SmileMachine_UseFire monster)
    {
        ownerMonster = monster;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                target.OnTakeDamage(ownerMonster.Stats.AttackPower);
            }
        }
    }
}