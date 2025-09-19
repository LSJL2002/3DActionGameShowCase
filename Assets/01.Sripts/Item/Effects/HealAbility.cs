using UnityEngine;

[CreateAssetMenu(fileName = "NewHealAbility", menuName = "Item Abilities/Heal")]
public class HealAbility : ItemAbility
{
    public override void Use(ItemData itemData)
    {
        // 플레이어 체력 회복
        //PlayerManager.Instance.Stats.Heal(itemData.effectValue, itemData.duration);
        Debug.Log($"플레이어 체력 {itemData.duration}초 동안 {itemData.effectValue} 회복");
    }
}