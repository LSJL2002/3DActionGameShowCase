using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlameTrigger : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float tickRate = 0.2f;

    private bool isPlayerInside = false;
    private Coroutine damageRoutine;
    private IDamageable targetDamageable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;

            if (other.TryGetComponent(out targetDamageable))
            {
                damageRoutine = StartCoroutine(ApplyDamageOverTime());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;

            if (damageRoutine != null)
            {
                StopCoroutine(damageRoutine);
                damageRoutine = null;
            }
        }
    }

    private IEnumerator ApplyDamageOverTime()
    {
        while (isPlayerInside && targetDamageable != null)
        {
            targetDamageable.OnTakeDamage(damage);
            Vector3 sourcePos = transform.position;
            float knockbackDistance = 0f;
            float duration = 5f;
            targetDamageable.ApplyEffect(MonsterEffectType.Burn, sourcePos, knockbackDistance, duration);
            yield return new WaitForSeconds(tickRate);
        }
    }
}
