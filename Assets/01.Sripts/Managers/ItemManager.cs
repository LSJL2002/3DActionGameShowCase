using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public void UseItem(ItemData itemData)
    {
        Debug.Log($"{itemData.duration}초 동안 총{itemData.effectValue}만큼 효과");

        StartCoroutine(UseItem(itemData.effectValue, itemData.duration));
    }

    private IEnumerator UseItem(float totalAmount, float duration)
    {
        float amountPerDuration = totalAmount / duration;
        
        for (int i = 0; i <= duration; i++)
            {
                PlayerManager.Instance.Stats.Heal(amountPerDuration);
            }

        yield return null;
    }
}
