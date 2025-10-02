using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : Singleton<SettingsManager>
{
    // 볼륨 설정을 저장할 PlayerPrefs 키
    private const string Master_VOLUME_KEY = "MasterVolume";
    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    private float masterVolume = 0.5f;
    private float bgmVolume = 0.5f;
    private float sfxVolume = 0.5f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        LoadVolumes(1); // 볼륨 설정 로드 함수 호출 (0은 기본값, 1은 설정값)
    }

    // 볼륨 설정 변경 함수
    public void SetMasterVolume(float volume)
    {
        // SettingsManager 내부 변수 업데이트
        masterVolume = volume;
        // 실제 오디오를 제어하는 AudioManager에 값 전달
        AudioManager.Instance.SetMasterVolume(masterVolume);
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        AudioManager.Instance.SetBgmVolume(bgmVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        AudioManager.Instance.SetSfxVolume(sfxVolume);
    }

    // PlayerPrefs에 설정 저장하는 함수
    public void SaveVolumes()
    {
        PlayerPrefs.SetFloat(Master_VOLUME_KEY, masterVolume);
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, bgmVolume);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
        PlayerPrefs.Save();
    }

    // PlayerPrefs에서 설정 가져오는 함수
    public void LoadVolumes(int value)
    {
        switch(value)
        {
            // 기본값 세팅
            case 0:
                masterVolume = 0.5f;
                bgmVolume = 0.5f;
                sfxVolume = 0.5f;
                break;

            // 저장값 세팅
            case 1:
                masterVolume = PlayerPrefs.GetFloat(Master_VOLUME_KEY, 0.5f);
                bgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 0.5f);
                sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.5f);
                break;
        }

        // 로드 즉시 볼륨 적용
        ApplyLoadedVolumes();
    }

    // 로드된 값을 즉시 AudioManager에 적용하는 함수 추가
    private void ApplyLoadedVolumes()
    {
        SetMasterVolume(masterVolume);
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);
    }

    // 볼륨 값 가져오기 함수 (AudioManager가 아닌 SettingsManager의 값을 반환)
    public float GetMasterVolume() => masterVolume;
    public float GetBGMVolume() => bgmVolume;
    public float GetSFXVolume() => sfxVolume;
}