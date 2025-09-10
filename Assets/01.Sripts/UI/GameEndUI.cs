using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : UIBase
{
    // 플레이어 레벨

    // 플레이어 경험치

    // 소모시간

    public async void OnClickButton(string str)
    {
        switch (str)
        {
            case "Exit":
                // 게임엔드UI 닫고 게임UI 팝업
                await UIManager.Instance.Show<GameUI>();
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }

    protected override void Awake()
    {
        // 플레이어 레벨 초기화

        // 플레이어 경험치 초기화

        // 소모시간 초기화
    }
}
