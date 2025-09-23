using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

// GameUI의 Player Part
public partial class GameUI : UIBase
{
    public TextMeshProUGUI playerHPText;   // UI : 플레이어 체력 텍스트
    public Slider playerHPSlider;          // UI : 플레이어 체력 슬라이더바
    public TextMeshProUGUI playerMPText;   // UI : 플레이어 마력 텍스트
    public Slider playerMPSlider;          // UI : 플레이어 마력 슬라이더바

    private float playerMaxHP;             // 플레이어 최대 체력
    private float playerMaxMP;             // 플레이어 최대 마력

    PlayerStats playerStats;               // 플레이어의 stats에 접근가능한 변수

    public void OnEnablePlayer()
    {
        playerStats = PlayerManager.Instance.Stats;

        // 플레이어 변수 초기화
        playerMaxHP = playerStats.MaxHealth.Value;
        playerMaxMP = playerStats.MaxEnergy.Value;

        // 플레이어 슬라이더를 초기화
        playerHPSlider.maxValue = 1f;
        playerMPSlider.maxValue = 1f;

        SelectAbilityUI.OnStatChange += UpdateStat;
    }

    public void UpdatePlayer()
    {
        // 플레이어 현재체력
        float playerCurrentHP = playerStats.CurrentHealth;

        // 플레이어 현재 체력텍스트 업데이트 (백분율, 소수점이하 버림, 형변환)
        playerHPText.text = Mathf.FloorToInt(playerCurrentHP / playerMaxHP * 100).ToString() + "%";

        // 플레이어 체력 슬라이더 업데이트
        playerHPSlider.value = playerCurrentHP / playerMaxHP;

        // 플레이어 현재마력
        float playerCurrentMP = playerStats.CurrentEnergy;

        // 플레이어 현재 마력텍스트 업데이트 (백분율, 소수점이하 버림, 형변환)
        playerMPText.text = Mathf.FloorToInt(playerCurrentMP / playerMaxMP * 100).ToString() + "%";

        // 플레이어 마력 슬라이더 업데이트
        playerMPSlider.value = playerCurrentMP / playerMaxMP;
    }

    // 플레이어 스탯이 변화했을때 호출 할 함수
    public void UpdateStat()
    {
        // 플레이어 맥스체력,마력을 업데이트
        playerMaxHP = playerStats.MaxHealth.Value;
        playerMaxMP = playerStats.MaxEnergy.Value;
    }
}
