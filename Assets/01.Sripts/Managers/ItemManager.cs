using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public void UseItem(ItemData itemData)
    {
        // 아이템 타입에 따라 다른 함수 호출
        switch (itemData.effectType1)
        {
            // Heal
            case ItemData.EffectType1.Heal:
                StartCoroutine(UseHealItem(itemData));
                break;

            // StatUP
            case ItemData.EffectType1.StatUP:
                StartCoroutine(UseStatUPItem(itemData));
                break;

            // SkillUP

            // DebuffUP

            // CooldownUP
        }
    }

    // Heal 코루틴 (HP, MP)
    private IEnumerator UseHealItem(ItemData itemData)
    {
        ItemData.EffectType2 effectType2 = itemData.effectType2; // 효과대상
        float healpercentage = itemData.effectValue; // 효과 값 (%)
        float duration = itemData.duration; // 효과 시간 (s)
        float totalHealAmount = 0;
        float amountPerDuration = totalHealAmount / duration; // 회복 총량을 지속시간으로 나누기

        switch (effectType2)
        {
            case ItemData.EffectType2.HP:

                Debug.Log($"{effectType2}를 {duration}초 동안 총 HP{healpercentage}만큼 회복효과");

                // 퍼센트만큼을 회복 총량으로 지정
                totalHealAmount = PlayerManager.Instance.Stats.MaxHealth.Value * (healpercentage / 100);
                // 지속시간동안 나눈 값만큼 회복
                for (int i = 0; i <= duration; i++)
                {
                    //PlayerManager.Instance.Stats.HPHeal(amountPerDuration);
                }
                break;

            case ItemData.EffectType2.MP:

                Debug.Log($"{effectType2}를 {duration}초 동안 총 HP{healpercentage}만큼 회복효과");

                // 퍼센트만큼을 회복 총량으로 지정
                totalHealAmount = PlayerManager.Instance.Stats.MaxEnergy.Value * (healpercentage / 100);
                // 지속시간동안 나눈 값만큼 회복
                for (int i = 0; i <= duration; i++)
                {
                    //PlayerManager.Instance.Stats.MPHeal(amountPerDuration);
                }
                break;
        }

        yield return null;
    }

    // StatUP 코루틴 (각 스탯)
    private IEnumerator UseStatUPItem(ItemData itemData)
    {
        // enum 타입을 명시적 변환 (enum 이름이 정확히 같을때)
        StatType statToModify = (StatType)itemData.effectType2;

        PlayerStats playerStats = PlayerManager.Instance.Stats;

        // 스탯을 추가하는 함수 호출
        playerStats.AddModifier(statToModify, itemData.effectValue);

        // 지속시간동안 대기
        yield return new WaitForSeconds(itemData.duration);

        // 스탯을 감소하는 함수 호출
        playerStats.RemoveModifier(statToModify, itemData.effectValue);
        
        yield return null;
    }
}
