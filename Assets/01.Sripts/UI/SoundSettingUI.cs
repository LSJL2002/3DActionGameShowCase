using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundSettingUI : UIBase
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [SerializeField] private GameObject returnButton;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (SceneLoadManager.Instance.nowSceneIndex == 1)
        {
            returnButton.SetActive(true);
        }
        else
        {
            returnButton.SetActive(false);
        }
    }

    protected override void Start()
    {
        base.Start();

        // 슬라이더 초기값 설정: AudioManager로부터 저장된 값을 가져와서 설정
        masterVolumeSlider.value = AudioManager.Instance.MasterVolume;
        bgmVolumeSlider.value = AudioManager.Instance.BgmVolume;
        sfxVolumeSlider.value = AudioManager.Instance.SfxVolume;

        // 슬라이더 이벤트 연결: 슬라이더 값이 변경되면 SettingsManager의 함수를 호출
        // (슬라이더 이벤트는 예약어)
        masterVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        bgmVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetBgmVolume);
        sfxVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetSfxVolume);
    }

    public async void OnClickButton(string str)
    {
        switch (str)
        {
            // 이전 UI로 돌아가기
            case "Return":
                await UIManager.Instance.Show<HomeUI>();
                Hide();
                break;

            case "Quit":
                // Home씬으로 돌아가기 (거기서 종료가능)
                SceneLoadManager.Instance.ChangeScene(1, null, LoadSceneMode.Single);
                Hide();
                break;

            // 키세팅 UI 켜기
            case "KeySetting":
                await UIManager.Instance.Show<KeySettingUI>();
                Hide();
                break;

            // 사운드 초기설정으로 (0은 기본값, 1은 저장값)
            case "Reset":
                AudioManager.Instance.SetAllVolume(0);
                masterVolumeSlider.value = AudioManager.Instance.MasterVolume;
                bgmVolumeSlider.value = AudioManager.Instance.BgmVolume;
                sfxVolumeSlider.value = AudioManager.Instance.SfxVolume;
                break;

            // 사운드 설정 저장
            case "Save":
                // Save 버튼을 누르면 SettingsManager에 저장 요청
                SaveManager.Instance.SavePlayerPrefs(SaveManager.PlayerPrefsSaveType.Volume);
                break;
        }

        // 현재 팝업창 닫기
        //Hide();
    }
}