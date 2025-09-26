using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemAbility", menuName = "Item Abilities/Heal")]
public class HealAbility : ItemAbility
{
    public override void Use(ItemData itemData)
    {
        //float healpercentage = itemData.effectValue; // 효과 값 (%)
        //float duration = itemData.duration; // 효과 시간 (s)
        //float totalHealAmount = 0;
        //float amountPerDuration = totalHealAmount / duration; // 회복 총량을 지속시간으로 나누기
        //float healamount = PlayerManager.Instance.Stats.MaxHealth.Value * itemData.effectValue/100; // 단위시간당 회복량

        //// DOTween.To를 사용하여 currentHealth 변수를 targetHealth까지 10초 동안 변화시킵니다.
        //DOTween.To(() => currentHealth, // Getter: 트윈 시작 시 현재 체력
        //           x => currentHealth = x, // Setter: 트윈이 진행되는 동안 체력 값 업데이트
        //           targetHealth, // End Value: 최종 목표 체력 (ex: 800)
        //           duration) // Duration: 트윈 지속 시간 (10초)
        //       .SetEase(Ease.Linear) // 부드럽고 일정한 회복을 위해 Linear(선형) 이징을 사용합니다.
        //       .OnUpdate(() =>
        //       {
        //           // 트윈이 진행될 때마다 호출됩니다 (선택 사항).
        //           // UI 업데이트 로직 (예: 체력 바 슬라이더)을 여기에 넣을 수 있습니다.
        //           Debug.Log($"회복 중... 현재 체력: {currentHealth:F2}");
        //       })
        //       .OnComplete(() =>
        //       {
        //           // 트윈이 10초 후에 완료되면 호출됩니다.
        //           Debug.Log($"물약 회복 완료! 최종 체력: {currentHealth:F2}");
        //       });
    }
}