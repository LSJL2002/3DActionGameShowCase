using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundFire : MonoBehaviour
{
    public float lifeTime = 5f;          // how long the fire stays alive
    public float damageInterval = 1f;    // how often to deal damage
    public float damageAmount = 10f;     // damage dealt each tick

    private ParticleSystem ps;
    private bool isActive = true;
    private HashSet<Collider> activeTargets = new HashSet<Collider>();

    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(lifeTime);
        isActive = false;
        if (ps != null)
        {
            ps.Stop();
        }
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        if (other.CompareTag("Player") && !activeTargets.Contains(other))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                activeTargets.Add(other);
                StartCoroutine(DamageOverTime(other, damageable));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (activeTargets.Contains(other))
        {
            activeTargets.Remove(other);
        }
    }

    private IEnumerator DamageOverTime(Collider target, IDamageable damageable)
    {
        while (isActive && activeTargets.Contains(target))
        {
            damageable.OnTakeDamage((int)damageAmount); 
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
