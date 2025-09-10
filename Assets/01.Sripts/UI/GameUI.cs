using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIBase
{
    public TextMeshProUGUI playerHPText; // 플레이어 체력 텍스트
    public Slider playerHPSlider; // 플레이어 체력 슬라이더바

    public TextMeshProUGUI playerMPText; // 플레이어 마력 텍스트
    public Slider playerMPSlider; // 플레이어 마력 슬라이더바

    public TextMeshProUGUI enemyHPText; // 적 체력 텍스트
    public Slider enemyHPSlider; // 적 체력 슬라이더바

    public async void OnClickButton(string str)
    {
        switch (str)
        {
            case "Pause":
                // 게임매니저의 게임 일시정지 메서드를 호출
                GameManager.Instance.PauseGame(true);
                // 일시정지 UI 팝업
                await UIManager.Instance.Show<PauseUI>();
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }

    protected override void Awake()
    {
        base.Awake();

        // 플레이어 최대체력을 가져와서 체력 변수 초기화
        // playerHPText = 

        // 플레이어 체력 슬라이더를 초기화
        playerHPSlider.maxValue = 1f;

        // 플레이어 최대마력을 가져와서 체력 변수 초기화
        // playerMPText = 

        // 플레이어 마력 슬라이더를 초기화
        playerMPSlider.maxValue = 1f;

        // 적 최대체력을 가져와서 체력 변수 초기화
        // enemyHPText = 

        // 적 체력 슬라이더를 초기화
        enemyHPSlider.maxValue = 1f;
    }

    protected override void Update()
    {
        base.Update();
       
        // 플레이어 최대체력
        int playerMaxHP = 0;

        // 플레이어 현재체력
        int playerCurrentHP = 0;

        // 플레이어 현재 체력텍스트 업데이트 (소수점이하 버림, 형변환)
        playerHPText.text = Mathf.FloorToInt(playerCurrentHP).ToString();

        // 플레이어 체력 슬라이더 업데이트
        playerHPSlider.value = playerCurrentHP / playerMaxHP;

        // 플레이어 최대마력
        int playerMaxMP = 0;

        // 플레이어 현재마력
        int playerCurrentMP = 0;

        // 플레이어 현재 마력텍스트 업데이트 (소수점이하 버림, 형변환)
        playerMPText.text = Mathf.FloorToInt(playerCurrentMP).ToString();

        // 플레이어 마력 슬라이더 업데이트
        playerMPSlider.value = playerCurrentMP / playerMaxMP;



        // 적 체력 가져올 구조 생각필요 (이번 적이 누구인지 어디서 정보를 받을지)

        // 적 최대체력
        int enemyMaxHP = 0;

        // 적 현재체력
        int enemyCurrentHP = 0;

        // 적 현재 체력텍스트 업데이트 (소수점이하 버림, 형변환)
        enemyHPText.text = Mathf.FloorToInt(enemyCurrentHP).ToString();

        // 적 체력 슬라이더 업데이트
        enemyHPSlider.value = enemyCurrentHP / enemyMaxHP;
    }
}
