using System.Collections;
using UnityEngine;

public class AreaEffectController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform filler;
    [SerializeField] private Transform border;
    [SerializeField] private SphereCollider damageCollider;

    public event System.Action OnTelegraphFinished;

    private float castTime;
    private int damage;

    public void Initialize(float castDuration, float radius, int damageAmount)
    {
        castTime = castDuration;
        damage = damageAmount;

        // Scale prefab size to radius
        transform.localScale = new Vector3(radius, 1, radius);

        damageCollider.enabled = false; // disabled until attack
        StartCoroutine(FillRoutine());
    }

    private IEnumerator FillRoutine()
    {
        filler.localScale = new Vector3(0, 0, 1);
        float timer = 0f;

        while (timer < castTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / castTime);

            // Works for any shape: prefab defines target scale
            filler.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);

            yield return null;
        }

        OnTelegraphFinished?.Invoke();
    }

    public void EnableDamage()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamage()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!damageCollider.enabled) return;

        if (other.CompareTag("Player"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            damageable?.OnTakeDamage(damage);
        }
    }
}
