using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkillUI : UIBase
{
    public async void OnClickButton(string str)
    {
        switch (str)
        {
            // 게임UI로 돌아가기
            case "Return":
                await UIManager.Instance.Show<TownUI>();
                break;

            // StatUI로 이동
            case "left":
                await UIManager.Instance.Show<CharacterStatUI>();
                break;

            // CoreUI로 이동
            case "Right":
                // UIManager.Instance.Show<CharacterCoreUI>();
                break;
        }
        
        // 현재 팝업창 닫기
        Hide();
    }

    // 스킬장착 함수 (스킬카드 버튼에서 호출)
    // 이미 장착상태일시 해제되도록 로직 구성
    public void SetSkill(string str)
    {
        
    }
}
