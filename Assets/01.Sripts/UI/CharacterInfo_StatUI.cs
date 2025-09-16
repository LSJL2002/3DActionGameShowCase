using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo_StatUI : UIBase
{
    public void OnClickButton(string str)
    {
        switch (str)
        {
            // CoreUI로 이동
            case "Core":
                // 코어 UI 켜기
                // UIManager.Instance.Show<CharacterInfo_CoreUI>();
                break;

            // SkillUI로 이동
            case "Skill":
                // 스킬 UI 켜기
                // UIManager.Instance.Show<CharacterInfo_SkillUI>();
                break;

        }

        // 현재 팝업창 닫기
        Hide();
    }
}
