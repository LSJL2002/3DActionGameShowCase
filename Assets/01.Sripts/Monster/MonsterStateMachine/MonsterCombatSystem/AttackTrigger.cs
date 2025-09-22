using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public int damage;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>()?.OnTakeDamage(damage);
            Debug.Log($"{name} hit {other.name} for {damage} damage!");
        }
    }
}
