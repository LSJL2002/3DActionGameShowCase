using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//전투관련 로직만
public class PlayerCombat : MonoBehaviour, IDamageable
{
    private IStats stats;

    private void Awake()
    {
        // PlayerSO는 ScriptableObject 데이터
        stats = new PlayerStats(Resources.Load<PlayerSO>("PlayerData"));
        stats.OnDie += HandleDie;
    }

    public void OnTakeDamage(int damage)
    {
        stats.TakeDamage(damage); //순수계산
        // 피격 애니메이션, 넉백 등 Unity 관련 처리 가능
        Debug.Log($"플레이어 체력: {stats.CurrentHealth}");
    }

    private void HandleDie()
    {
        // 유니티에서 처리할 죽음 이벤트
        Debug.Log("플레이어 사망!");
        // ex: 애니메이션, 게임오버 UI, 상태머신 전환
    }

    private void OnDestroy()
    {
        stats.OnDie -= HandleDie;
    }
}
