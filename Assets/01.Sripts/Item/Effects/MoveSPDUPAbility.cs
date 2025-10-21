using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemAbility", menuName = "Item Abilities/MoveSPDUP")]
public class MoveSPDUPAbility : ItemAbility
{
    // DOTween Sequence 참조를 저장
    private Sequence movespdupSequence;

    public override void Use(ItemData itemData)
    {
        PlayerAttribute stats = PlayerManager.Instance.Attr;
        float movespdupPercentage = itemData.effectValue; // 추가 스탯
        float duration = itemData.duration;
        float totalMoveSPDUPAmount = stats.MoveSpeed.Value * movespdupPercentage / 100; // 총 증가량 계산: 공격력의 n%

        // 이전 Sequence가 있다면 제거 (중복 사용 방지)
        movespdupSequence?.Kill();

        // 새로운 Sequence 생성
        movespdupSequence = DOTween.Sequence();

        movespdupSequence.AppendCallback(() =>
        {
            stats.AddModifier(StatType.MoveSpeed, totalMoveSPDUPAmount);
            EventsManager.Instance.TriggerEvent(GameEvent.OnStatChanged);
            Debug.Log($"물약 사용 시작: 총 {duration}초 동안 이동속도 {movespdupPercentage}% 증가(+{totalMoveSPDUPAmount})");
        }); 

        movespdupSequence.AppendInterval(duration);

        movespdupSequence.AppendCallback(() =>
        {
            stats.RemoveModifier(StatType.MoveSpeed, totalMoveSPDUPAmount);
            EventsManager.Instance.TriggerEvent(GameEvent.OnStatChanged);
            Debug.Log($"능력치 복구");
        });

        // Sequence 재생 및 Auto파괴옵션세팅 호출
        movespdupSequence.SetAutoKill(true) // 기본값(true)을 명시적으로 설정
                    .OnKill(() => movespdupSequence = null) // Sequence가 Kill 될 때 참조 해제
                    .Play(); // Sequence 시작

        Debug.Log($"물약 사용 시작: 총 {duration}초 동안 공격력 {movespdupPercentage}% 증가(+{totalMoveSPDUPAmount})");
    }
}