using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ProjectileHitbox : MonoBehaviour
{
    [Header("투사체 기본 설정")]
    [SerializeField] private LayerMask targetLayer;       // 타겟 레이어
    [SerializeField] private float speed = 10f;           // 이동 속도
    [SerializeField] private float hitRadius = 1f;        // 충돌 판정 반경
    [SerializeField] private int maxHits = 1;             // 최대 타격 횟수
    [SerializeField] private float hitInterval = 0.03f;   // 동일 대상 타격 간격

    private Vector3 moveDir;
    private int hitCount = 0;

    // 이미 맞은 대상 + 마지막 타격 시간 기록
    private readonly HashSet<IDamageable> alreadyHit = new();
    private readonly Dictionary<IDamageable, float> lastHitTime = new();
    public event Action<IDamageable, Vector3> OnHit; // 맞은 대상, 위치 콜백

    public void Launch(Vector3 startPos, Vector3 direction)
    {
        transform.position = startPos;
        moveDir = direction.normalized;
        alreadyHit.Clear(); // 새로 발사할 때 초기화
        lastHitTime.Clear();
        hitCount = 0;
    }

    private void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;

        // 충돌 체크
        var hits = Physics.OverlapSphere(transform.position, hitRadius, targetLayer);
        foreach (var col in hits)
        {
            if (col.TryGetComponent<IDamageable>(out var dmg))
            {
                float timeSinceLastHit = Time.time - (lastHitTime.ContainsKey(dmg) ? lastHitTime[dmg] : 0f);
                if (!alreadyHit.Contains(dmg) || timeSinceLastHit >= hitInterval)
                {
                    alreadyHit.Add(dmg);
                    lastHitTime[dmg] = Time.time;

                    hitCount++;
                    OnHit?.Invoke(dmg, col.ClosestPoint(transform.position));

                    // 최대 타격 횟수 도달 시 투사체 비활성화
                    if (hitCount >= maxHits)
                    {
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        // 오브젝트가 비활성화(풀에 복귀)될 때 구독자 정리
        OnHit = null;            // 클래스 내부라 가능
        alreadyHit.Clear();
        lastHitTime.Clear();
        moveDir = Vector3.zero;
        hitCount = 0;
    }

    private void OnDrawGizmos()
    {
        // Scene 뷰에서 현재 충돌 범위 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}