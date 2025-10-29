using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreviewScene : MonoBehaviour
{
    [Header("Preview Scene")]
    [SerializeField] private string previewSceneName = "PlayerUIScene";
    private bool isLoaded = false;
    private Animator previewAnimator;
    [SerializeField] private Canvas canvas;

    [Header("UI Buttons")]
    [SerializeField] private Button btnInfo;
    [SerializeField] private Button btnCore;
    [SerializeField] private Button btnSkill;
    [SerializeField] private Button btnOption;
    [SerializeField] private Button btnQuit;

    private void Awake()
    {
        // 버튼 리스너 등록
        btnInfo?.onClick.AddListener(() => OpenPreview("Char"));
        btnCore?.onClick.AddListener(() => OpenPreview("Core"));
        btnSkill?.onClick.AddListener(() => OpenPreview("Skill"));
        btnOption?.onClick.AddListener(() => OpenPreview("Option"));
        btnQuit?.onClick.AddListener(() => OpenPreview("Quit"));
    }

    private void OnEnable()
    {
        LoadPreviewScene();

        if (canvas.sortingOrder == 0)
        {
            canvas.sortingOrder = UIManager.Instance.GetNewSortingOrder();
        }
    }

    private void OnDisable()
    {
        if (UIManager.Instance != null)
        {
            var ui = UIManager.Instance.currentUI;
            if (ui != null && ui.gameObject != null)
            {
                string uiName = ui.gameObject.name;
                UIManager.Instance.Hide(uiName);
                Debug.Log($"[OnDisable] UI '{uiName}' 숨김 처리 완료");
            }
        }

        UnloadPreviewScene();
    }

    private void LoadPreviewScene()
    {
        if (isLoaded) return;
        SceneManager.LoadScene(previewSceneName, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnPreviewSceneLoaded;
        isLoaded = true;
    }

    private void UnloadPreviewScene()
    {
        if (!isLoaded) return;
        SceneManager.UnloadSceneAsync(previewSceneName);
        isLoaded = false;
    }

    private void OnPreviewSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != previewSceneName) return;

        previewAnimator = GameObject.Find("PreviewCharacter")?.GetComponent<Animator>();
        SceneManager.sceneLoaded -= OnPreviewSceneLoaded;

        // 인벤토리 활성화 시 기본 캐릭터 포즈 재생
        if (gameObject.activeSelf)
        {
            OpenPreview("Char");
        }
    }

    /// <summary>
    /// 버튼 클릭 또는 InputAction 호출용
    /// </summary>
    /// <param name="target">Char / Stat / Core / Skill / Inven</param>
    private async void OpenPreview(string target)
    {
        if (previewAnimator == null) return;

        if (UIManager.Instance.currentUI != null)
        {
            string uiName = UIManager.Instance.currentUI.gameObject.name;
            UIManager.Instance.Hide(uiName);
        }

        // 먼저 모든 Pose bool 초기화
        previewAnimator.SetBool("Base/Switch_Char", false);
        previewAnimator.SetBool("Base/Switch_Stat", false);
        previewAnimator.SetBool("Base/Switch_Core", false);
        previewAnimator.SetBool("Base/Switch_Skill", false);
        previewAnimator.SetBool("Base/Switch_Inven", false);

        // 선택한 Pose만 true
        switch (target)
        {
            case "Char":
                previewAnimator.SetBool("Base/Switch_Char", true);
                await UIManager.Instance.Show<CharacterInfomationUI>();
                break;
            case "Core":
                previewAnimator.SetBool("Base/Switch_Stat", true);
                await UIManager.Instance.Show<CharacterCoreUI>();
                break;
            case "Skill":
                previewAnimator.SetBool("Base/Switch_Core", true);
                await UIManager.Instance.Show<CharacterSkillUI>();
                break;
            case "Option":
                previewAnimator.SetBool("Base/Switch_Skill", true);
                await UIManager.Instance.Show<SoundSettingUI>();
                break;
            case "Quit":
                previewAnimator.SetBool("Base/Switch_Inven", true);
                Time.timeScale = 1f;
                PlayerManager.Instance.EnableInput(false);
                SceneLoadManager.Instance.ChangeScene(1, null, LoadSceneMode.Single); // Home씬으로 이동
                break;
        }
    }
}