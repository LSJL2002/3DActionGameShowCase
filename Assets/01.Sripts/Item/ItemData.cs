using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Consumable, SkillCard, Core, }

    public enum EffectType { None, Heal, }

    public int id;
    public string inGameName;
    public int spriteID;
    public Sprite sprite;
    public int scriptID;
    public string scriptText;
    public int maxEa;
    public ItemType itemType;
    public EffectType effectType;
    public float effectValue;
    public float duration; // 효과 지속시간

    // 능력 배열
    public List<ItemAbility> abilities;
}