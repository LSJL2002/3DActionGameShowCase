using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DecisionButtonUI : UIBase
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private TextMeshProUGUI guideText;

    public event Action<bool> OnDecisionMade; // (구독 : 인벤토리뷰모델)

    protected override void OnEnable()
    {
        base.OnEnable();

        SetGuideText();

        Canvas canvas = GetComponentInParent<Canvas>();
        canvas.sortingOrder = 101;
    }

    public void SetGuideText()
    {
        switch(InventoryManager.Instance.currentDecisionState)
        {
            case DecisionState.UseItem:
                guideText.text = "아이템을<br>사용하시겠습니까?";
                break;

            case DecisionState.SelectAbility:
                guideText.text = "이 능력을<br>선택하시겠습니까??";
                break;
        }
    }

    public void OnClickButton(string str)
    {
        DOVirtual.DelayedCall(0.2f, () => { }); // 아무것도 없이 n초간 대기

        switch (str)
        {
            case "Yes":
                OnDecisionMade?.Invoke(true);
                break;

            case "No":
                OnDecisionMade?.Invoke(false);
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
