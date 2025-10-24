using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeUI : UIBase
{
    [SerializeField] private CanvasGroup newGameButton;
    [SerializeField] private CanvasGroup loadGameButton;
    [SerializeField] private CanvasGroup optionButton;
    [SerializeField] private CanvasGroup quitButton;

    // 로드게임 활성화 관련 오브젝트들
    [SerializeField] private ButtonHoverEffects buttonHoverEffects;
    [SerializeField] private Button loadGameButtonComponent;
    [SerializeField] private CanvasGroup loadGameButtonCanvasGroup;

    protected override void OnEnable()
    {
        base.OnEnable();

        // SaveData가 없다면,
        if (!File.Exists(SaveManager.Instance.path))
        {
            loadGameButtonComponent.interactable = false;
            buttonHoverEffects.enabled = false;

            if (loadGameButtonCanvasGroup != null)
            {
                loadGameButtonCanvasGroup.DOFade(0f, 0f).OnComplete(() => { loadGameButtonCanvasGroup.DOFade(0.5f, 1f); });
            }
        }
        else
        {
            loadGameButtonComponent.interactable = true;
            buttonHoverEffects.enabled = true;
        }
    }

    public async void OnClickButton(string str)
    {
        switch (str)
        {
            case "NewGame":
                // 새 게임 시작
                SaveManager.Instance.gameMode = eGameMode.NewGame;
                SaveManager.Instance.DeleteSaveFile();
                SceneLoadManager.Instance.ChangeScene(2, null, LoadSceneMode.Single);
                break;

            case "LoadStart":
                // 기존 게임을 로드
                SaveManager.Instance.gameMode = eGameMode.LoadGame;
                SceneLoadManager.Instance.ChangeScene(2, null, LoadSceneMode.Single);
                InventoryManager.Instance.ClearAllInventory();
                break;

            case "OptionUI":
                // IU매니저의 Show 메서드를 호출하여 OptionUI를 화면에 표시
                await UIManager.Instance.Show<SoundSettingUI>();
                break;

            case "Quit":
                // 어플리케이션 종료
                Application.Quit();
                break;
        }
        // 현재 팝업창 닫기
        Hide();
    }
}
