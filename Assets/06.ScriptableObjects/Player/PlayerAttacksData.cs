using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AttackType
{
    LightAttack,
    HeavyAttack,
    Skill,
    Dodge
}

public enum EffectType
{
    None,
    knockback,
    groggy,
}

public class PlayerAttacksData : ScriptableObject
{
    public int id;
    public string name;
    public AttackType attackType;
    public EffectType effectType;
    public float effectValue;
    public float duration;
    //public List<castEffectName> = new List<castEffectName>();
    public float range;
    //public List<areaEffectPrefab> = new List<areaEffectPrefab>();
    public float knockbackDistance;
    public int comboSkillId;
    public float cooldown;
    public int mpCost;
    public int hitCount;
    public float skillUseRange;
    public float preCastTime;
}
