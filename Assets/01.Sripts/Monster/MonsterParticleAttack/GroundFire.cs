using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundFire : MonoBehaviour
{
    public float lifeTime = 5f;

    private ParticleSystem ps;
    private bool isActive = true;
    private HashSet<Collider> effected = new HashSet<Collider>();

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

    private void OnTriggerStay(Collider other)
    {
        if (!isActive) return;
        if (other.CompareTag("Player") && !effected.Contains(other))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Vector3 sourcePos = transform.position;
                float knockbackDistance = 0f;
                float duration = 5f;
                damageable.ApplyEffect(MonsterEffectType.Burn, sourcePos, knockbackDistance, duration);

                effected.Add(other);
            }
        }
    }
}