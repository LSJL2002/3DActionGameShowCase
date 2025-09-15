using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 역활: 충돌감지, 대상필터링(Layer기준)
// 행위 실행은 안함: 실제 데미지 처리, 피격 애니메이션, 넉백 등은 Hitbox가 직접 하지 않음
// 장점: Hitbox는 범용으로 여러 공격 모션에 재사용 가능
public class Hitbox : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    private bool active = false;

    // 이미 맞은 대상 관리 (멀티히트 방지)
    private HashSet<IDamageable> alreadyHit = new HashSet<IDamageable>();

    // Hitbox가 맞으면 호출되는 이벤트
    public event Action<IDamageable> OnHit;

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
            if (alreadyHit.Contains(dmgable)) return;

            alreadyHit.Add(dmgable);
            OnHit?.Invoke(dmgable);
        }
    }
}