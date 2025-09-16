using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo_SkillUI : UIBase
{
    public void OnClickButton(string str)
    {
        switch (str)
        {
            // StatUI로 이동
            case "Stat":
                // 스탯 UI 켜기
                // UIManager.Instance.Show<CharacterInfo_StatUI>();
                break;

            // CoreUI로 이동
            case "Core":
                // 코어 UI 켜기
                // UIManager.Instance.Show<CharacterInfo_CoreUI>();
                break;

            // 스킬1 장착/해제
            case "Skill_1":
                // 스킬 장착 함수 호출
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
