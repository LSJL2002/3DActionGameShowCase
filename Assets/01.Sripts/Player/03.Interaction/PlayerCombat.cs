using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//전투관련 로직만
public class PlayerCombat : MonoBehaviour, IDamageable
{
    private PlayerManager player;
    private SkillManagers skillManager;

    [Header("Debug / Gizmos")]
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private int attackIndex = 0; // 인스펙터에서 공격 선택

    [SerializeField] private Transform currentAttackTarget; // 인스펙터용
    public Transform CurrentAttackTarget
    {
        get => currentAttackTarget;
        private set => currentAttackTarget = value;
    }

    private void Awake()
    {
        player ??= GetComponent<PlayerManager>();
        skillManager = GetComponentInChildren<SkillManagers>();
        playerInfo = player.InfoData;
    }

    /// 공격 입력 시 호출 에니메이션 이벤트로 조작
    public void OnAttack(string skillName)
    {
        var skillObj = skillManager.SpawnSkill(skillName);

        // Hitbox 연결
        var skillHitbox = skillObj.GetComponentInChildren<Hitbox>();
        if (skillHitbox != null)
            skillHitbox.OnHit += HandleHit;

        // ParticleSystem 재생
        var ps = skillObj.GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();

        // 일정 시간 후 반환 처리
        float duration = ps != null ? ps.main.duration : 1f;
        StartCoroutine(ReturnAfterTime(skillName, skillObj, duration));
    }

    private IEnumerator ReturnAfterTime(string skillName, GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        var skillHitbox = obj.GetComponent<Hitbox>();
        if (skillHitbox != null)
            skillHitbox.OnHit -= HandleHit;
    }

    public void SetAttackTarget(Transform target)
    {
        CurrentAttackTarget = target;
    }


    private void HandleHit(IDamageable target)
    {
        int damage = Mathf.RoundToInt(player.Stats.Attack.Value);
        target.OnTakeDamage(damage);
    }

    // Hitbox 충돌 이벤트 처리


    // IDamageable 구현 예시 (플레이어가 맞았을 때)
    public void OnTakeDamage(int amount)
    {
        player?.Stats.TakeDamage(amount); // HP 변경은 Stats에서만
        // 피격 애니메이션, 넉백 등도 여기서 처리 가능
    }

    // -------------------------------
    // 기즈모 시각화 (씬뷰에서 공격 범위/Force 확인용)
    private void OnDrawGizmosSelected()
    {
        if (playerInfo == null || playerInfo.AttackData == null)
            return;

        var attackData = playerInfo.AttackData;

        // 공격 범위 (공용)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackData.AttackRange);

        // 돌진 멈춤 거리 (공용)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackData.StopDistance);

        // Force 방향 표시 (공용 기본값만 사용)
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.5f);

        // 타겟 연결선
        Transform target = currentAttackTarget; // 편집 모드에서 연결한 필드 사용
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position);
            Gizmos.DrawWireSphere(target.position, 0.5f);
        }
    }
}