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

    [Header("UI Buttons")]
    [SerializeField] private Button btnCharacter;
    [SerializeField] private Button btnStat;
    [SerializeField] private Button btnCore;
    [SerializeField] private Button btnSkill;
    [SerializeField] private Button btnInventory;

    private void Awake()
    {
        // 버튼 리스너 등록
        btnCharacter?.onClick.AddListener(() => OpenPreview("Char"));
        btnStat?.onClick.AddListener(() => OpenPreview("Stat"));
        btnCore?.onClick.AddListener(() => OpenPreview("Core"));
        btnSkill?.onClick.AddListener(() => OpenPreview("Skill"));
        btnInventory?.onClick.AddListener(() => OpenPreview("Inven"));
    }

    private void OnEnable()
    {
        LoadPreviewScene();
    }

    private void OnDisable()
    {
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
    private void OpenPreview(string target)
    {
        if (previewAnimator == null) return;

        // 먼저 모든 Pose bool 초기화
        previewAnimator.SetBool("Base/Switch_Char", false);
        previewAnimator.SetBool("Base/Switch_Stat", false);
        previewAnimator.SetBool("Base/Switch_Core", false);
        previewAnimator.SetBool("Base/Switch_Skill", false);
        previewAnimator.SetBool("Base/Switch_Inven", false);

        // 선택한 Pose만 true
        switch (target)
        {
            case "Char": previewAnimator.SetBool("Base/Switch_Char", true);
                break;
            case "Stat": previewAnimator.SetBool("Base/Switch_Stat", true);
                UIManager.Instance.Show<CharacterInfomationUI>();
                break;
            case "Core": previewAnimator.SetBool("Base/Switch_Core", true);
                UIManager.Instance.Show<CharacterCoreUI>();
                break;
            case "Skill": previewAnimator.SetBool("Base/Switch_Skill", true);
                UIManager.Instance.Show<CharacterSkillUI>();
                break;
            case "Inven": previewAnimator.SetBool("Base/Switch_Inven", true);
                UIManager.Instance.Show<SoundSettingUI>();
                break;
        }
    }
}