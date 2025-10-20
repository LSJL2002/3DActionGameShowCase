using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    private int damage;
    private Transform owner;
    private float lifeTime = 2f;
    private float activeTime = 0.3f;

    private Collider triggerCollider;

    public void Initialize(int dmg, Transform shooter)
    {
        damage = dmg;
        owner = shooter;

        triggerCollider = GetComponent<Collider>();
        if (triggerCollider == null)
        {
            Debug.LogWarning("ExplosionDamage: No Collider found!");
        }
        Invoke(nameof(DisableCollider), activeTime);

        Destroy(gameObject, lifeTime);
    }

    private void DisableCollider()
    {
        if (triggerCollider != null)
            triggerCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner != null && other.transform == owner)
            return;

        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<IDamageable>(out var dmgTarget))
            {
                dmgTarget.OnTakeDamage(damage);
            }
        }
    }
}
