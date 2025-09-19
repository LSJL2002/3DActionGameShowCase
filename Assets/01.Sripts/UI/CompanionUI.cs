using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionUI : UIBase
{
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
                //await UIManager.Instance.Show<CompanionTalkUI>();
                break;
        }

        Hide();
    }    
}