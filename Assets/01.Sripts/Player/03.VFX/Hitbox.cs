using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 역할: 충돌 감지 & 대상 필터링(Layer 기준)
// 실제 데미지, 이펙트 등은 외부(구독자)가 처리
public class Hitbox : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    private bool active = false;

    // 이미 맞은 대상 관리 (멀티히트 방지)
    private readonly HashSet<IDamageable> alreadyHit = new();

    // Hitbox가 맞으면 호출되는 이벤트
    public event Action<IDamageable, Vector3> OnHit;

    public void OnEnable()
    {
        active = true;
        alreadyHit.Clear();
    }

    public void OnDisable()
    {
        active = false;
    }

    // Hitbox는 충돌 감지만
    // OnHit 이벤트로 데미지 처리 위임

    private void OnTriggerEnter(Collider other)
    {
        if (!active) return;

        if ((targetLayer.value & (1 << other.gameObject.layer)) == 0) return;

        if (other.TryGetComponent<IDamageable>(out var dmgable))
        {
            // 이미 맞은 대상이면 무시
            if (alreadyHit.Contains(dmgable)) return;

            // 아직 맞지 않았다면 → 히트 처리
            alreadyHit.Add(dmgable);
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            OnHit?.Invoke(dmgable, hitPoint);
        }
    }
}