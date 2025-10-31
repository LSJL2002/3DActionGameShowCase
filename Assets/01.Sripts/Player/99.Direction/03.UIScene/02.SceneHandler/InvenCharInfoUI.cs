using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InvenCharInfoUI : MonoBehaviour
{
    [SerializeField] private UISceneManager manager;

    [SerializeField] private Button robyBtn;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button goldBtn;
    [SerializeField] private Button energyBtn;
    [SerializeField] private Button cashBtn;

    [SerializeField] private List<Button> buttons;
    // 문자열 키와 실제 UI 열기 함수 매핑
    private Dictionary<string, Func<Task>> uiActions;

    private void Awake()
    {
        // Dictionary 초기화
        uiActions = new Dictionary<string, Func<Task>>()
        {
            { "Info", async () => { await UIManager.Instance.Show<CharacterInfomationUI>(); } },
            { "Core", async () => { await UIManager.Instance.Show<CharacterCoreUI>(); } },
            { "Skill", async () => { await UIManager.Instance.Show<CharacterSkillUI>(); } },
            { "Option", async () => { await UIManager.Instance.Show<SoundSettingUI>(); } },
            { "Quit", async () =>
                {
                    EventsManager.Instance.TriggerEvent(GameEvent.OnMenu);
                    UIManager.Instance.currentUI.Hide();
                    FindAnyObjectByType<DirectionManager>()?.UnloadScene();
                }
            }
        };
        EventsManager.Instance.TriggerEvent(GameEvent.OnMenu);
    }

    private void Start()
    {
        if (robyBtn != null) robyBtn.onClick.AddListener(OnLobbyBtnClicked);
        if (backBtn != null) backBtn.onClick.AddListener(OnBackBtnClicked);
        if (goldBtn != null) goldBtn.onClick.AddListener(OnGoldBtnClicked);
        if (energyBtn != null) energyBtn.onClick.AddListener(OnEnergyBtnClicked);
        if (cashBtn != null) cashBtn.onClick.AddListener(OnCashBtnClicked);

        // 버튼 리스트 반복 처리
        string[] uiKeys = { "Info", "Core", "Skill", "Option", "Quit" };
        for (int i = 0; i < buttons.Count && i < uiKeys.Length; i++)
        {
            int index = i; // 캡처 문제 방지
            buttons[i].onClick.AddListener(() => OnOpenUI(uiKeys[index]));
        }
    }

    // ---------------- 버튼별 함수 ----------------

    // 로비로 가는 버튼
    private async void OnLobbyBtnClicked()
    {
        Time.timeScale = 1f;
        PlayerManager.Instance.EnableInput(false);
        SceneLoadManager.Instance.ChangeScene(1, null, LoadSceneMode.Single);
        await Task.CompletedTask;
    }

    // 뒤로 가는 버튼
    private void OnBackBtnClicked()
    {
        if (manager != null && manager.seqCam1 != null)
            manager.seqCam1.enabled = true;

        UIManager.Instance.currentUI.Hide();
    }

    private void OnGoldBtnClicked() { /* TODO: 구현 */ }
    private void OnEnergyBtnClicked() { /* TODO: 구현 */ }
    private void OnCashBtnClicked() { /* TODO: 구현 */ }

    private async void OnOpenUI(string key)
    {
        if (UIManager.Instance.currentUI != null)
            UIManager.Instance.Hide(UIManager.Instance.currentUI.gameObject.name);

        if (uiActions.TryGetValue(key, out var action))
            await action();
        else
            Debug.LogWarning($"UI key '{key}' not found in dictionary.");
    }
}