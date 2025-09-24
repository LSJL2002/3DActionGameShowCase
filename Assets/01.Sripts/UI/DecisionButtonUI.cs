using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GuideState { UseItem, SelectAbility, }

public class DecisionButtonUI : UIBase
{
    public GuideState currentGuideState;

    [SerializeField] private ItemData itemData;
    [SerializeField] private TextMeshProUGUI guideText;

    public event Action<bool> OnDecisionMade; // (구독 : 인벤토리뷰모델)

    // State를 변경할 때 호출할 함수
    public void ChangeState(GuideState state)
    {
        currentGuideState = state;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        SetGuideText();

        Canvas canvas = GetComponentInParent<Canvas>();
        canvas.sortingOrder = 101;
    }

    public void SetGuideText()
    {
        switch(currentGuideState)
        {
            case GuideState.UseItem:
                guideText.text = "아이템을<br>사용하시겠습니까?";
                break;

            case GuideState.SelectAbility:
                guideText.text = "이 능력을<br>선택하시겠습니까??";
                break;
        }
    }

    public void OnClickButton(string str)
    {
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
