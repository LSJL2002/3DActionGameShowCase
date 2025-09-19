using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAbilityUI : UIBase
{
    // 배틀매니저가 가지고 있는 아이템을 저장할 변수
    private int upgradeStat;
    private string skillItemAdress;
    private string coreItemAdress;

    protected override void OnEnable()
    {
        base.OnEnable();

        // 변수 초기화
        //upgradeStat =
        //skillItemAdress = 
        //coreItemAdress =
    }

    public void OnClickButton(string str)
    {
        InventoryManager.Instance.LoadData_Addressables("Healing_Potion");
        Debug.Log("회복약 획득");

        switch (str)
        {
            case "Stat":
                // 플레이어 스탯 증가 함수 호출
                Debug.Log($"플레이어 {upgradeStat} 스탯증가");
                break;

            case "Skill":
                // 아이템을 인벤토리에 추가
                InventoryManager.Instance.LoadData_Addressables(skillItemAdress);
                Debug.Log($"{skillItemAdress} 획득");
                break;

            case "Core":
                // 아이템을 인벤토리에 추가
                InventoryManager.Instance.LoadData_Addressables(coreItemAdress);
                Debug.Log($"{coreItemAdress} 획득");
                break;
        }

        Hide();
    }
}
