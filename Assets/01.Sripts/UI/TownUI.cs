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
            case "CharacterInfo":
                // await UIManager.Instance.Show<CharacterInfoUI>();
                break;

            // 인벤토리UI 활성화
            case "Inventory":
                // await UIManager.Instance.Show<InventoryUI>();
                break;

            // 상점UI 활성화
            case "Shop":
                // await UIManager.Instance.Show<ShopUI>();
                break;

            // 퀘스트UI 활성화
            case "Quest":
                // await UIManager.Instance.Show<QuestUI>();
                break;

            // 설정UI 활성화
            case "OptionUI":
                await UIManager.Instance.Show<OptionUI>();
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
