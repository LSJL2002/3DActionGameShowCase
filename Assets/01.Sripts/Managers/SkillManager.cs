using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("두번배기 스킬 데이터 (ScriptableObject 참조)")]
    public SkillSO doubleSlashSkill;

    [Header("타격 대상 레이어")]
    public LayerMask enemyLayerMask;

    [Header("애니메이션 (선택)")]
    public Animator animator;
    public string doubleSlashTriggerName = "DoubleSlash";

    [Header("OverlapSphere 시각화")]
    public bool drawGizmos = true;
    public Color gizmoColor = new Color(1f, 0.2f, 0.2f, 0.25f);

    private readonly Dictionary<int, float> _cooldownEndTimes = new Dictionary<int, float>();
    private PlayerStatsSkill _stats;
    private Transform _transform;

    private void Awake()
    {
        _stats = GetComponent<PlayerStatsSkill>();
        if (_stats == null)
            Debug.LogError("[SkillManager] PlayerStats가 필요합니다.");

        _transform = transform;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TryUseDoubleSlash();
        }
    }

    private void TryUseDoubleSlash()
    {
        if (doubleSlashSkill == null || _stats == null) return;

        // 쿨타임 체크
        if (IsOnCooldown(doubleSlashSkill))
        {
            Debug.Log($"[SkillManager] {doubleSlashSkill.skillName} 쿨타임 진행 중...");
            return;
        }

        // MP 차감
        if (!_stats.TryConsumeMP(doubleSlashSkill.mpCost))
        {
            Debug.Log("[SkillManager] MP 부족");
            return;
        }

        // 실행
        StartCoroutine(DoubleSlashRoutine(doubleSlashSkill));
    }

    private IEnumerator DoubleSlashRoutine(SkillSO skill)
    {
        StartCooldown(skill);

        if (animator && !string.IsNullOrEmpty(doubleSlashTriggerName))
            animator.SetTrigger(doubleSlashTriggerName);

        DoHitScan(skill, 1);
        yield return new WaitForSeconds(0.2f);
        DoHitScan(skill, 2);

        if (skill.duration > 0f && skill.effectType != SkillSO.EffectType.Damage)
            yield return StartCoroutine(ApplyTimedEffect(skill));
    }

    private void DoHitScan(SkillSO skill, int slashIndex)
    {
        Vector3 center = GetHitCenter(skill);
        float radius = skill.effectRadius;

        // 1) 레이어마스크가 비어 있으면(=Nothing) 모든 레이어로 임시 확장해서 디버그
        LayerMask maskToUse = enemyLayerMask.value == 0 ? Physics.AllLayers : enemyLayerMask;

        // 2) Trigger 콜라이더도 맞도록 Collide로 명시
        Collider[] hits = Physics.OverlapSphere(center, radius, maskToUse, QueryTriggerInteraction.Collide);

        // 디버깅 로그: 중심/반경/탐지수
        Debug.Log($"[HitScan] center={center} radius={radius} hits={hits.Length} mask={maskToUse.value}");

        if (hits.Length == 0)
        {
            Debug.Log($"[SkillManager] {slashIndex}차 베기: 타격 대상 없음");
            return;
        }

        float damage = skill.basePower + _stats.attackPower;

        foreach (var col in hits)
        {
            // 3) 자식 콜라이더 ↔ 부모 스크립트 구조 대응
            IDamageableSkill dmg =
                col.GetComponent<IDamageableSkill>() ??
                col.GetComponentInParent<IDamageableSkill>() ??
                col.GetComponentInChildren<IDamageableSkill>();

            // 추가 디버그: 무슨 오브젝트를 때렸는지, 레이어/트리거 정보
            Debug.Log($"[HitScan] hit={col.name} layer={LayerMask.LayerToName(col.gameObject.layer)} isTrigger={col.isTrigger}");

            if (dmg != null)
            {
                Vector3 hitPoint = col.bounds.center;
                dmg.TakeDamage(damage, hitPoint, _transform);
                Debug.Log($"{slashIndex}차 베기: {col.name}에게 {damage} 피해");
            }
            else
            {
                Debug.LogWarning($"[HitScan] {col.name}에서 IDamageable을 찾지 못했습니다. (부모/자식 확인)");
            }
        }
    }

    private Vector3 GetHitCenter(SkillSO skill)
    {
        Vector3 forward = _transform.forward;
        Vector3 origin = _transform.position + forward * skill.useRange;
        origin.y = _transform.position.y + 1f;
        return origin;
    }

    private IEnumerator ApplyTimedEffect(SkillSO skill)
    {
        Debug.Log($"'{skill.skillName}' {skill.effectType} 효과 {skill.duration}s 시작");
        yield return new WaitForSeconds(skill.duration);
        Debug.Log($"'{skill.skillName}' 효과 종료");
    }

    private void StartCooldown(SkillSO skill)
    {
        _cooldownEndTimes[skill.id] = Time.time + skill.cooldown;
    }

    private bool IsOnCooldown(SkillSO skill)
    {
        if (_cooldownEndTimes.TryGetValue(skill.id, out float endTime))
            return Time.time < endTime;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos || doubleSlashSkill == null) return;
        Vector3 center = transform.position + transform.forward * doubleSlashSkill.useRange;
        center.y = transform.position.y + 1f;
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(center, doubleSlashSkill.effectRadius);
    }
}
