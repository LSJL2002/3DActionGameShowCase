using System;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvenCharInfoUI : MonoBehaviour
{
    [SerializeField] private UISceneManager manager;

    [SerializeField] private Button robyBtn;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button goldBtn;
    [SerializeField] private Button energyBtn;
    [SerializeField] private Button cashBtn;

    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private Button button4;
    [SerializeField] private Button button5;


    private void Start()
    {
        if (robyBtn != null) robyBtn.onClick.AddListener(OnRobyBtnClicked);
        if (backBtn != null) backBtn.onClick.AddListener(OnBackBtnClicked);
        if (goldBtn != null) goldBtn.onClick.AddListener(OnGoldBtnClicked);
        if (energyBtn != null) energyBtn.onClick.AddListener(OnEnergyBtnClicked);
        if (cashBtn != null) cashBtn.onClick.AddListener(OnCashBtnClicked);

        if (button1 != null) button1.onClick.AddListener(OnBtn1Clicked);
        if (button2 != null) button2.onClick.AddListener(OnBtn2Clicked);
        if (button3 != null) button3.onClick.AddListener(OnBtn3Clicked);
        if (button4 != null) button4.onClick.AddListener(OnBtn4Clicked);
        if (button5 != null) button5.onClick.AddListener(OnBtn5Clicked);
    }

    // ---------------- 버튼별 함수 ----------------

    // 로비로 가는 버튼
    private void OnRobyBtnClicked()
    {
        // 실제 로비 이동 로직 구현
        // 예: SceneManager.LoadScene("Lobby");
    }

    // 뒤로 가는 버튼
    private void OnBackBtnClicked()
    {
        if (manager != null && manager.seqCam != null)
            manager.seqCam.enabled = true;
    }

    // 나머지 버튼 함수 이름만 정의
    private void OnGoldBtnClicked() { /* TODO: 구현 */ }
    private void OnEnergyBtnClicked() { /* TODO: 구현 */ }
    private void OnCashBtnClicked() { /* TODO: 구현 */ }

    private void OnBtn1Clicked() { /* TODO: 구현 */ }
    private void OnBtn2Clicked() { /* TODO: 구현 */ }
    private void OnBtn3Clicked() { /* TODO: 구현 */ }
    private void OnBtn4Clicked() { /* TODO: 구현 */ }
    private void OnBtn5Clicked() { /* TODO: 구현 */ }
}