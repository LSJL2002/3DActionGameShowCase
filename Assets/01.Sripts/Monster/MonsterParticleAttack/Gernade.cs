using UnityEngine;

public class Gernade : MonoBehaviour
{
    private float lifetime = 5f;
    private int damage;
    private Transform owner;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float rotateSpeed = 10f; // how quickly it turns toward velocity

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Initialize(int dmg, Transform shooter)
    {
        damage = dmg;
        owner = shooter;
    }

    private void Update()
    {
        if (rb != null && rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rb.linearVelocity.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
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
