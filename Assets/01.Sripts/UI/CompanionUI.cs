using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionUI : UIBase
{
    [HideInInspector] public CompanionController controller;

    void OnEnable()
    {
        // 이미 세팅돼 있지 않다면 씬에서 찾아서 캐싱
        if (controller == null)
            controller = FindObjectOfType<CompanionController>();
    }

    public void OnClickClose()
    {
        // 안전 가드: null이면 로그만 남기고 리턴
        if (controller == null)
        {
            Debug.LogError("[CompanionUI] controller 참조가 없습니다. 씬에 ObjectFollow가 있는지 확인하세요.");
            return;
        }

        // 플레이어 입력 재허용(싱글톤이 없다면 이 줄은 주석 처리해도 됨)
        if (PlayerManager.Instance != null)
            PlayerManager.Instance.EnableInput(true);

        // 위치/커서/챗UI 복원은 기존 ObjectFollow가 담당
        controller.ExitTalkMode();
        controller.Sm?.ChangeState(new CompanionFollowState(controller.Sm));
    }

    public async void OnClickButton(string str)
    {
        switch(str)
        {
            case "Stat":
                await UIManager.Instance.Show<CharacterStatUI>();
                break;

            case "Inventory":
                await UIManager.Instance.Show<CharacterInventoryUI>();
                break;

            case "Talk":
                await UIManager.Instance.Show<TutorialUI>();
                UIManager.Instance.Get<TutorialUI>().PlayDialogue(SceneType.Tutorial);
                break;
        }

        Hide();
    }    
}