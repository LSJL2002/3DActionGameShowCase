using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 역활: 충돌감지, 대상필터링(Layer기준)
// 행위 실행은 안함: 실제 데미지 처리, 피격 애니메이션, 넉백 등은 Hitbox가 직접 하지 않음
// 장점: Hitbox는 범용으로 여러 공격 모션에 재사용 가능
public class Hitbox : MonoBehaviour
{
    private int damage;
    private bool active = false;
    [SerializeField] private LayerMask targetLayer; // 몬스터 레이어만 맞도록

    public void Enable(int dmg)
    {
        damage = dmg;
        active = true;
    }

    public void Disable()
    {
        active = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!active) return;

        if ((targetLayer.value & (1 << other.gameObject.layer)) == 0) return;

        if (other.TryGetComponent<IDamageable>(out var dmgable))
        {
            dmgable.OnTakeDamage(damage);
            Disable(); // 한 번 맞으면 비활성화
        }
    }
}