using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemAbility", menuName = "Item Abilities/ATKUP")]
public class ATKUPAbility : ItemAbility
{
    // DOTween Sequence 참조를 저장
    private Sequence ATKUPSequence;

    public override void Use(ItemData itemData)
    {
        PlayerAttribute stats = PlayerManager.Instance.Attr;
        float atkupPercentage = itemData.effectValue; // 추가 스탯
        float duration = itemData.duration;
        float totalATKUPAmount = stats.Attack.Value * atkupPercentage / 100; // 총 증가량 계산: 공격력의 n%

        // 이전 Sequence가 있다면 제거 (중복 사용 방지)
        ATKUPSequence?.Kill();

        // 새로운 Sequence 생성
        ATKUPSequence = DOTween.Sequence();

        ATKUPSequence.AppendCallback(() => stats.AddModifier(StatType.Attack, totalATKUPAmount));
        ATKUPSequence.AppendInterval(duration);
        ATKUPSequence.AppendCallback(() => stats.RemoveModifier(StatType.Attack, totalATKUPAmount));

        // Sequence 재생 및 Auto파괴옵션세팅 호출
        ATKUPSequence.SetAutoKill(true) // 기본값(true)을 명시적으로 설정
                    .OnKill(() => ATKUPSequence = null) // Sequence가 Kill 될 때 참조 해제
                    .Play(); // Sequence 시작

        Debug.Log($"물약 사용 시작: 총 {duration}초 동안 공격력 {atkupPercentage}% 증가(+{totalATKUPAmount})");
    }
}