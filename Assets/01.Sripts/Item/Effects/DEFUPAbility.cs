using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemAbility", menuName = "Item Abilities/DEFUP")]
public class DEFUPAbility : ItemAbility
{
    // DOTween Sequence 참조를 저장
    private Sequence DEFUPSequence;

    public override void Use(ItemData itemData)
    {
        PlayerAttribute stats = PlayerManager.Instance.Attr;
        float defupPercentage = itemData.effectValue; // 추가 스탯
        float duration = itemData.duration;
        float totalDEFUPAmount = stats.Defense.Value * defupPercentage / 100; // 총 증가량 계산: 공격력의 n%

        // 이전 Sequence가 있다면 제거 (중복 사용 방지)
        DEFUPSequence?.Kill();

        // 새로운 Sequence 생성
        DEFUPSequence = DOTween.Sequence();

        DEFUPSequence.AppendCallback(() => stats.AddModifier(StatType.Defense, totalDEFUPAmount));
        DEFUPSequence.AppendInterval(duration);
        DEFUPSequence.AppendCallback(() => stats.RemoveModifier(StatType.Defense, totalDEFUPAmount));

        // Sequence 재생 및 Auto파괴옵션세팅 호출
        DEFUPSequence.SetAutoKill(true) // 기본값(true)을 명시적으로 설정
                    .OnKill(() => DEFUPSequence = null) // Sequence가 Kill 될 때 참조 해제
                    .Play(); // Sequence 시작

        Debug.Log($"물약 사용 시작: 총 {duration}초 동안 공격력 {defupPercentage}% 증가(+{totalDEFUPAmount})");
    }
}