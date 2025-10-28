using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HitboxType { OverlapSphere, OverlapBox, RayCast, RayAll, CapsuleCast }
public enum AttackType { Melee, Projectile, InstantRange }

public class HitboxOverlap : MonoBehaviour
{
    private PlayerCharacter player;
    private Transform body;

    [Header("히트박스 기본 설정")]
    [field: SerializeField] public AttackType attackType { get; private set; } = AttackType.Melee;
    [SerializeField] private HitboxType type = HitboxType.OverlapSphere;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float radius = 1f;
    [SerializeField] private float distance = 5f;
    [SerializeField] private Vector3 hitboxOffset = new Vector3(0, 0, 1f);
    [SerializeField] private Vector3 boxSize = Vector3.one;
    [SerializeField] private Vector3 capsuleStart = Vector3.zero;
    [SerializeField] private Vector3 capsuleEnd = Vector3.forward * 2f;
    [SerializeField] private float capsuleSpeed = 10f;    // 초당 이동 속도

    private readonly HashSet<IDamageable> alreadyHit = new();
    public event Action<IDamageable, Vector3, float> OnHit;

    private float currentDamageMultiplier = 1f;
    public void SetDamageMultiplier(float multiplier) => currentDamageMultiplier = multiplier;

    public void Inject(PlayerCharacter player)
    {
        this.player = player;
        body = player.Body;
    }

    // 딱 한프레임만 실행, Clear로 중복도방지함
    public void FireSkill()
    {
        alreadyHit.Clear();

        switch (type)
        {
            case HitboxType.OverlapSphere: FireSphere(); break;
            case HitboxType.OverlapBox: FireBox(); break;
            case HitboxType.CapsuleCast: FireCapsule(); break;
            case HitboxType.RayCast: FireRay(); break;
            case HitboxType.RayAll: break;
        }
    }

    private void FireSphere()
    {
        Vector3 center = body.position + body.rotation * hitboxOffset; // 오프셋 반영
        var hits = Physics.OverlapSphere(center, radius, targetLayer);
        foreach (var col in hits) ProcessHit(col);
    }

    private void FireBox()
    {
        Vector3 center = body.position + body.rotation * hitboxOffset; // 오프셋 반영
        var hits = Physics.OverlapBox(center, boxSize / 2f, body.rotation, targetLayer);
        foreach (var col in hits) ProcessHit(col);
    }

    private void FireRay()
    {
        Vector3 origin = body.position + body.rotation * hitboxOffset; // 오프셋 반영
        Vector3 dir = body.forward;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, distance, targetLayer))
            ProcessHit(hit.collider);
    }

    private void FireCapsule()
    {
        StartCoroutine(CapsuleCastRoutine());
    }

    private IEnumerator CapsuleCastRoutine()
    {
        Vector3 startPos = body.position + body.rotation * (capsuleStart + hitboxOffset);
        Vector3 endPos = body.position + body.rotation * (capsuleEnd + hitboxOffset);
        Vector3 direction = (endPos - startPos).normalized;
        float traveled = 0f;

        while (traveled < distance)
        {
            float step = capsuleSpeed * Time.deltaTime;
            traveled += step;

            if (Physics.CapsuleCast(
                startPos, endPos, radius,
                direction, out RaycastHit hit,
                step, targetLayer))
            {
                ProcessHit(hit.collider);
                yield break; // 맞으면 멈춤
            }

            startPos += direction * step;
            endPos += direction * step;
            yield return null;
        }
    }

    private void ProcessHit(Collider col)
    {
        if (!col.TryGetComponent<IDamageable>(out var dmgable)) return;
        if (alreadyHit.Contains(dmgable)) return;

        alreadyHit.Add(dmgable);
        Vector3 hitPoint = col.ClosestPoint(body.position);

        OnHit?.Invoke(dmgable, hitPoint, currentDamageMultiplier);
    }

    private void OnDrawGizmosSelected()
    {
        Transform bodyToUse = body;

        if (bodyToUse == null)
        {
            var player = GetComponent<PlayerCharacter>();
            if (player != null && player.Body != null)
                bodyToUse = player.Body;
            else
                bodyToUse = transform;
        }

        Vector3 offsetPos = bodyToUse.position + bodyToUse.rotation * hitboxOffset; // 기즈모에서도 오프셋 반영

        switch (type)
        {
            case HitboxType.OverlapSphere:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(offsetPos, radius);
                break;
            case HitboxType.OverlapBox:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(offsetPos, boxSize);
                break;
            case HitboxType.RayCast:
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(offsetPos, offsetPos + bodyToUse.forward * distance);
                break;
            case HitboxType.CapsuleCast:
                Gizmos.color = Color.blue;
                Vector3 p1 = bodyToUse.position + bodyToUse.rotation * (capsuleStart + hitboxOffset);
                Vector3 p2 = bodyToUse.position + bodyToUse.rotation * (capsuleEnd + hitboxOffset);
                Vector3 dir = (p2 - p1).normalized;
                Gizmos.DrawWireSphere(p1, radius);
                Gizmos.DrawWireSphere(p2, radius);
                Gizmos.DrawLine(p1, p2);
                Gizmos.DrawLine(p1, p1 + dir * distance);
                break;
        }
    }
}