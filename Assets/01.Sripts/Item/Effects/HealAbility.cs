using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemAbility", menuName = "Item Abilities/Heal")]
public class HealAbility : ItemAbility
{
    // DOTween Sequence 참조를 저장
    private Sequence healSequence;

    public override void Use(ItemData itemData)
    {
        PlayerStats stats = PlayerManager.Instance.Stats;
        float healPercentage = itemData.effectValue; // 회복량 (n%)
        float duration = itemData.duration; // 지속시간 (n초)
        float totalHealAmount = stats.MaxHealth.Value * healPercentage / 100; // 총 회복량 계산: 최대 체력의 n%
        float healPerTick = totalHealAmount / duration; // 1회당 회복량 계산: 총 회복량 / 호출 횟수
        float interval = itemData.duration / duration; // 회복 호출 간격 계산: 총 지속 시간 / 호출 횟수

        // 이전 Sequence가 있다면 제거 (중복 사용 방지)
        healSequence?.Kill();

        // 새로운 Sequence 생성
        healSequence = DOTween.Sequence();

        // 루프를 돌면서 Sequence에 AppendCallback과 AppendInterval을 추가
        for (int i = 0; i < duration; i++)
        {
            // 플레이어의 AddHeal 함수 호출 (1회당 회복량 전달)
            healSequence.AppendCallback(() => stats.RecoverHealth(healPerTick));

            // 다음 회복까지 대기 시간 추가
            if (i < duration - 1)
            {
                // interval초씩 대기 (회복은 바로 돼서 0초걸리고, 1초 쉬고, 다시 회복 0초 이런식)
                healSequence.AppendInterval(interval);
            }
        }

        // Sequence 재생 및 Auto파괴옵션세팅 호출
        healSequence.SetAutoKill(true) // 기본값(true)을 명시적으로 설정
                    .OnKill(() => healSequence = null) // Sequence가 Kill 될 때 참조 해제
                    .Play(); // Sequence 시작

        Debug.Log($"물약 사용 시작: 총 {duration}초 동안 {duration}회 (매 {interval}초마다 {healPerTick} 회복)");
    }
}