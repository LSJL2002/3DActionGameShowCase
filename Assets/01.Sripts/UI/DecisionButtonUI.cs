using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionButtonUI : UIBase
{
    public ItemData itemData;

    public event Action<bool> OnDecisionMade; // (구독 : 인벤토리뷰모델)

    protected override void OnEnable()
    {
        base.OnEnable();

        Canvas canvas = GetComponentInParent<Canvas>();
        canvas.sortingOrder = 101;
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
