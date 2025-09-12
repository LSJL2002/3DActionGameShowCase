using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO_New", menuName = "Skill/New Skill")]
public class SkillSO : ScriptableObject
{
    // === 스킬 전용 enum (중첩 정의) ===
    public enum AttackType
    {
        Melee,      // 근접
        Ranged,     // 원거리
        Magic       // 마법
    }

    public enum EffectType
    {
        Damage,     // 피해
        Buff,       // 버프
        Debuff      // 디버프
    }

    [Header("기본 식별 정보")]
    public int id;                   // 고유 식별 번호
    public string skillName;         // 스킬 이름
    public Sprite skillIcon;         // 스킬 이미지(UI 아이콘)

    [Header("전투/효과 정보")]
    public AttackType attackType;    // 공격 타입
    public EffectType effectType;    // 효과 종류
    public float duration = 0f;      // 효과 지속 시간
    public float effectRadius = 1.5f;// OverlapSphere 반경
    public float cooldown = 2f;      // 쿨타임(초)
    public float mpCost = 10f;       // 소모 MP
    public float useRange = 2f;      // 사용 가능 거리

    [Header("전투 수치")]
    public float basePower = 25f;    // 기본 공격력
}
