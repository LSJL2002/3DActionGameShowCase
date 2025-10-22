using UnityEngine;

public class Missile : MonoBehaviour
{
    private float lifetime = 5f;
    private int damage;
    private Transform owner;
    [SerializeField] private GameObject explosionPrefab;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Initialize(int dmg, Transform shooter)
    {
        damage = dmg;
        owner = shooter;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (owner != null && collision.transform == owner)
            return;

        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            if (explosion.TryGetComponent<ExplosionDamage>(out var explosionScript))
            {
                explosionScript.Initialize(damage, owner);
            }
        }
        Destroy(gameObject);
    }
}
