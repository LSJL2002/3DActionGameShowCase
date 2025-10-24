using System;
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

        PlayerManager.Instance.EnableInput(false); // 플레이어 입력 제한
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SetGuideText()
    {
        switch(UIManager.Instance.currentDecisionState)
        {
            case DecisionState.UseItem:
                guideText.text = "아이템을<br>사용하시겠습니까?";
                break;

            case DecisionState.SelectAbility:
                guideText.text = "이 능력을<br>선택하시겠습니까??";
                break;

            case DecisionState.EnterToZone:
                guideText.text = "진입 후 전투가 시작됩니다.<br>진입하시겠습니까?";
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
        PlayerManager.Instance.EnableInput(true); // 플레이어 입력 제한 해제
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Hide(); // 현재 팝업창 닫기
    }
}
