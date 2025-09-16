using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public partial class CharacterInfoUI : UIBase
{
    public List<GameObject> infoUIList;

    private int currentIndex = 0;

    public async void OnClickButton(string str)
    {
        switch (str)
        {
            // 게임UI로 돌아가기
            case "Return":
                await UIManager.Instance.Show<GameUI>();
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }

    protected override void OnEnable()
    {
        // 초기 UI 설정 (첫 번째 UI만 활성화)
        SetUI();

        SetPlayerStat(); // 플레이어 스탯 UI 초기화
    }

    // 왼쪽/오른쪽 버튼 클릭 시 호출될 함수
    public void ChangeUI(int direction)
    {
        // 현재 인덱스를 다음 또는 이전으로 이동
        currentIndex += direction;

        // 인덱스를 배열 범위 내로 유지 (순환 구조)
        // C#에서는 음수 % 연산이 원하는 대로 작동하지 않을 수 있으므로, 별도로 처리
        if (currentIndex < 0)
        {
            currentIndex = infoUIList.Count - 1;
        }
        else if (currentIndex >= infoUIList.Count)
        {
            currentIndex = 0;
        }

        SetUI();
    }

    private void SetUI()
    {
        // 모든 UI를 비활성화
        for (int i = 0; i < infoUIList.Count; i++)
        {
            infoUIList[i].SetActive(false);
        }

        // 현재 인덱스에 해당하는 UI만 활성화
        if (infoUIList.Count > 0)
        {
            infoUIList[currentIndex].SetActive(true);
        }
    }
}
