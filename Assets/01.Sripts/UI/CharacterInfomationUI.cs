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

        InventoryAwake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InventoryOnEnable();

        SetPlayerStat();
    }

    public async void OnClickButton(string str)
    {
        AudioManager.Instance.PlaySFX("ButtonSoundEffect");

        switch (str)
        {
            // 게임UI로 돌아가기
            case "Return":
                await UIManager.Instance.Show<CompanionUI>();
                Hide();
                break;

            // CoreUI로 이동
            case "Left":
                await UIManager.Instance.Show<CharacterCoreUI>();
                Hide();
                break;

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
