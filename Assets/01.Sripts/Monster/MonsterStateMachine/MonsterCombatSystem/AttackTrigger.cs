using UnityEngine;
using System;
public class AttackTrigger : MonoBehaviour
{
    public int damage;
    public Action onHit;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>()?.OnTakeDamage(damage);
            onHit?.Invoke();
            Debug.Log($"{name} hit {other.name} for {damage} damage!");
        }
    }
}
