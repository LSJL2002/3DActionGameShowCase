using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ProjectileHitbox : MonoBehaviour
{
    [Header("투사체 기본 설정")]
    [SerializeField] private LayerMask targetLayer;            // 타겟 레이어
    [SerializeField] private float speed = 10f;                // 이동 속도
    [SerializeField] private float hitRadius = 1f;           // 충돌 판정 반경

    private Vector3 moveDir;

    private readonly HashSet<IDamageable> alreadyHit = new();
    public event Action<IDamageable, Vector3> OnHit; // 맞은 대상, 위치 콜백

    public void Launch(Vector3 startPos, Vector3 direction)
    {
        transform.position = startPos;
        moveDir = direction.normalized;
        alreadyHit.Clear(); // 새로 발사할 때 초기화
    }

    private void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;

        var hits = Physics.OverlapSphere(transform.position, hitRadius, targetLayer);
        foreach (var col in hits)
        {
            if (col.TryGetComponent<IDamageable>(out var dmg) && !alreadyHit.Contains(dmg))
            {
                alreadyHit.Add(dmg);
                OnHit?.Invoke(dmg, col.ClosestPoint(transform.position));
            }
        }
    }

    private void OnDisable()
    {
        // 오브젝트가 비활성화(풀에 복귀)될 때 구독자 정리
        OnHit = null;            // 클래스 내부라 가능
        alreadyHit.Clear();
        moveDir = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        // Scene 뷰에서 현재 충돌 범위 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}