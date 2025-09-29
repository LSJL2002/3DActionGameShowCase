using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AreaEffectController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform filler;
    [SerializeField] private Transform border;
    [SerializeField] private Collider damageCollider;
    private MonsterSkillSO skillData;

    public event System.Action OnTelegraphStarted;
    public event System.Action OnTelegraphFinished;

    private float castTime;
    private int damage;

    public void CircleInitialize(float castDuration, float radius, int damageAmount, MonsterSkillSO skill)
    {
        skillData = skill;
        castTime = castDuration;
        damage = damageAmount;

        // Scale prefab size to radius
        transform.localScale = new Vector3(radius, 1, radius);

        damageCollider.enabled = false; // disabled until attack
        StartCoroutine(FillRoutine());
    }

    public void DashInitialize(float castDuration, float length, int damageAmount, Transform monsterTransform)
    {
        castTime = castDuration;
        damage = damageAmount;

        Vector3 originalScale = transform.localScale;
        transform.localScale = new Vector3(originalScale.x, originalScale.y, length);

        // Position it in front of the monster
        Vector3 forwardOffset = monsterTransform.forward * (length / 2 + 0.5f); // 0.5f can adjust for monster pivot
        transform.position = monsterTransform.position + forwardOffset;

        // Align rotation to the monster's forward
        transform.rotation = Quaternion.LookRotation(monsterTransform.forward);

        StartCoroutine(FillRoutine());
    }


    private IEnumerator FillRoutine()
    {
        OnTelegraphStarted?.Invoke();
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
    // ------------------- Half-Circle Method -------------------
    public void HalfCircleInitialize(float castDuration, float radius, int damageAmount, Transform origin, MonsterSkillSO skill, float angle = 180f)
    {
        skillData = skill; // store reference
        castTime = castDuration;
        damage = damageAmount;

        transform.localScale = new Vector3(radius, 1, radius);
        transform.position = origin.position;
        transform.rotation = Quaternion.LookRotation(origin.forward);

        StartCoroutine(HalfCircleRoutine(origin, radius, angle));
    }


    private IEnumerator HalfCircleRoutine(Transform origin, float radius, float angle)
    {
        OnTelegraphStarted?.Invoke();

        filler.localScale = Vector3.zero;
        float timer = 0f;

        while (timer < castTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / castTime);
            filler.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }

        // Apply damage after telegraph
        ApplyHalfCircleDamage(origin, radius, angle);

        OnTelegraphFinished?.Invoke();
    }

    private void ApplyHalfCircleDamage(Transform origin, float radius, float angle)
    {
        Collider[] hits = Physics.OverlapSphere(origin.position, radius);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Player")) continue;

            Vector3 dirToTarget = (hit.transform.position - origin.position).normalized;
            float angleToTarget = Vector3.Angle(origin.forward, dirToTarget);

            if (angleToTarget <= angle / 2f)
            {
                IDamageable damageable = hit.GetComponent<IDamageable>();
                damageable?.OnTakeDamage(damage * skillData.effectValue);

                if (skillData != null)
                {
                    damageable?.ApplyEffect(
                        skillData.monsterEffectType,
                        origin.position,
                        skillData.effectValue,
                        skillData.duration
                    );
                }
            }
        }
    }


    public void EnableDamage(Transform monsterTransform)
    {
        damageCollider.enabled = true;

        // Get all colliders overlapping this trigger collider
        Collider[] hits = Physics.OverlapBox(
            damageCollider.bounds.center,
            damageCollider.bounds.extents,
            damageCollider.transform.rotation
        );

        foreach (Collider col in hits)
        {
            if (col.CompareTag("Player"))
            {
                IDamageable damageable = col.GetComponent<IDamageable>();
                damageable?.OnTakeDamage(damage * skillData.effectValue);

                if (skillData != null)
                {
                    damageable?.ApplyEffect(
                        skillData.monsterEffectType,
                        monsterTransform.position,
                        skillData.effectValue,
                        skillData.duration
                    );
                }
            }
        }
    }

    public void DisableDamage()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Testing Hit");
        if (!damageCollider.enabled) return;

        if (other.CompareTag("Player"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            damageable?.OnTakeDamage(damage * skillData.effectValue);
        }
    }
}
