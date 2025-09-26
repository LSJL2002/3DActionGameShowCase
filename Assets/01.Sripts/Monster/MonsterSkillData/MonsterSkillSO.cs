using System;
using UnityEngine;

public enum MonsterAttackType
{
    None,
    기술,
    물리,
    마법
}

public enum MonsterEffectType
{
    None,
    Knockback,
    Groggy,
    Burn,
    Freeze,
    Slow,
    Poison
}

[CreateAssetMenu(fileName = "NewSkill", menuName = "Game Data/Skill")]
public class MonsterSkillSO : ScriptableObject
{
    [Header("Basic Info")]
    public int id;
    public string skillName;
    public string imageId; 
    public AttackType monsterAttackType; // enum instead of string

    [Header("Effect Info")]
    public MonsterEffectType monsterEffectType; // enum instead of string
    public float effectValue;   // 효과 수치
    public float duration;      // 지속 시간

    [Header("Casting & Visuals")]
    public string castEffectName;
    public float range;
    public GameObject areaEffectPrefab; // AoE 프리팹
    public float knockbackDistance;

    [Header("Combat Flow")]
    public int comboSkillId; 
    public float cooldown;
    public int mpCost;
    public int hitCount;
    public float skillUseRange;
    public float preCastTime;
}
