using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : UIBase
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    protected override void Start()
    {
        base.Start();

        // 슬라이더 초기값 설정
        masterVolumeSlider.value = AudioManager.Instance.MasterVolume;
        bgmVolumeSlider.value = AudioManager.Instance.BgmVolume;
        sfxVolumeSlider.value = AudioManager.Instance.SfxVolume;

        // 2. 슬라이더의 OnValueChanged 이벤트에 리스너 등록
        // 슬라이더 값이 변경될 때마다 오디오 매니저의 볼륨 설정 함수를 호출
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
                // UI매니저의 '이전UI' 변수를 찾아 활성화
                UIManager.Instance.previousUI.canvas.gameObject.SetActive(true);
                // UI매니저의 '현재UI' 변수에 이전 변수를 저장
                UIManager.Instance.currentUI = UIManager.Instance.previousUI;
                break;

            case "Quit":
                // Home씬으로 돌아가기 (거기서 종료가능)
                SceneLoadManager.Instance.LoadScene(2);
                break;

            // 키세팅 UI
            case "KeySetting":
                await UIManager.Instance.Show<KeySettingUI>();
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
