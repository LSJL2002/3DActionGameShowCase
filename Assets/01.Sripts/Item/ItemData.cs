using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Consumable, Skill, Core, }

    public enum EffectType { None, Heal, }

    public string itemDescription;
    public int id;
    public string inGameName;
    public Sprite itemIcon;
    public int maxEa;
    public ItemType itemType;
    public EffectType effectType;
    public float effectValue;
    public float duration; // 효과 지속시간

    // 능력 배열
    public List<ItemAbility> abilities;
}