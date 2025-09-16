using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterInfoUI : UIBase
{
    public TextMeshProUGUI nameText;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI currentHealthText;

    public TextMeshProUGUI energyText;
    public TextMeshProUGUI currentEnergyText;

    public TextMeshProUGUI attackText;
    public TextMeshProUGUI extraAttackText; // 추가 공격력

    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI extraDefenseText; // 추가 방어력

    public TextMeshProUGUI moveSpeedText;
    public TextMeshProUGUI extraMoveSpeedText; // 추가 이동속도

    public void OnClickButton(string str)
    {
        switch (str)
        {
            // 이전 UI로 돌아가기
            case "Return":
                // UI매니저의 '이전UI' 변수를 찾아 활성화
                UIManager.Instance.previousUI.canvas.gameObject.SetActive(true);
                // UI매니저의 '현재UI' 변수에 이전 변수를 저장
                UIManager.Instance.currentUI = UIManager.Instance.previousUI;
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }

    protected override void OnEnable()
    {
        
    }

    // 플레이어 정보 초기화 함수
}
