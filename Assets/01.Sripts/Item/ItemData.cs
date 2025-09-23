using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Consumable, SkillCard, Core, }

    public enum EffectType1 { None, Heal, StatUP, SkillATKUP, DebuffUP, CooldownUP}

    public enum EffectType2 { None, HP, MP, Attack, Defense, MoveSpeed, AttackSpeed, }

    public int id;
    public string inGameName;
    public int spriteID;
    public Sprite sprite;
    public int scriptID;
    public string scriptText;
    public int maxEa;
    public ItemType itemType;
    public EffectType1 effectType1;
    public EffectType2 effectType2;
    public float effectValue;
    public float duration; // 효과 지속시간
    public List<int> colors;

    // 능력 배열
    public List<ItemAbility> abilities;
}