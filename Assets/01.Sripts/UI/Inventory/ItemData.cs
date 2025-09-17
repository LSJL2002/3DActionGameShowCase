using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Consumable, Skill, Core, }

    public enum EffectType { None, Heal, }

    public int itemID;
    public string itemName;
    public Sprite itemIcon;
    [TextArea]
    public string itemDescription;
    public int maxEa;
    public ItemType itemType;
    public EffectType effectType;
    public float effectValue;
    public float duration; // 효과 지속시간
}