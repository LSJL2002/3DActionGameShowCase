using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUI : UIBase
{
    public async void OnClickButton(string str)
    {
        switch (str)
        {
            // 캐릭터 정보UI 활성화
            case "CharacterStat":
                await UIManager.Instance.Show<CharacterStatUI>();
                break;

            // 스킬UI 활성화
            case "Skill":
                await UIManager.Instance.Show<CharacterSkillUI>();
                break;

            // 코어UI 활성화
            case "Core":
                //await UIManager.Instance.Show<CharacterCoreUI>();
                break;

            // 인벤토리UI 활성화
            case "Inventory":
                await UIManager.Instance.Show<CharacterInventoryUI>();
                break;

            // 설정UI 활성화
            case "Option":
                await UIManager.Instance.Show<OptionUI>();
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
