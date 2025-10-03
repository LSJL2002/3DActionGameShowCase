using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// CharacterInfomationUI의 Base Part
public partial class CharacterInfomationUI : UIBase
{
    protected override void Awake()
    {
        base.Awake();

        AwakeInventory();
        AwakeStatus();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        OnEnableInventory();
        uiType = UIType.Screen;
    }

    public void OnClickButton(string str)
    {
        switch (str)
        {
            case "20000000":
                InventoryManager.Instance.LoadData_Addressables(str, 1);
                break;

            case "20010008":
                InventoryManager.Instance.LoadData_Addressables(str);
                break;

            case "20010005":
                InventoryManager.Instance.LoadData_Addressables(str);
                break;
        }

        // 현재 팝업창 닫기
        //Hide();
    }
}
