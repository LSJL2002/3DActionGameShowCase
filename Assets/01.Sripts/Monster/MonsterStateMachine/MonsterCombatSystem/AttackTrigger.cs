using UnityEngine;
using System;

public class AttackTrigger : MonoBehaviour
{
    private int damage;
    public Action onHit;

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>()?.OnTakeDamage(damage);
            onHit?.Invoke();
        }
    }
}
