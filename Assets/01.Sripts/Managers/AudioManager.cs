using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class NamedBGM
{
    public string name;
    public AudioClip clip;
}
[Serializable]
public class NamedSFX
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup bgmGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("BGM Clips")]
    [SerializeField] private List<NamedBGM> bgmList;

    [Header("SFX Clips")]
    [SerializeField] private List<NamedSFX> sfxList;
    [SerializeField] private int sfxPoolSize = 5; //동시재생 가능 수

    private List<AudioSource> sfxSources = new();
    private int currentSfxIndex = 0;
    private AudioSource bgmSource;

    private Dictionary<string, AudioClip> bgmDict = new();
    private Dictionary<string, AudioClip> sfxDict = new();

    [SerializeField, Range(0f, 1f)] private float _masterVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float _bgmVolume = 0.5f;
    [SerializeField, Range(0f, 1f)] private float _sfxVolume = 0.5f;

    public float MasterVolume => _masterVolume;
    public float BgmVolume => _bgmVolume;
    public float SfxVolume => _sfxVolume;

    protected override void Awake()
    {
        base.Awake();

        // BGM 오디오 소스 생성
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.outputAudioMixerGroup = bgmGroup;
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        // SFX 풀 초기화
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource sfx = gameObject.AddComponent<AudioSource>();
            sfx.playOnAwake = false;
            sfx.outputAudioMixerGroup = sfxGroup;
            sfxSources.Add(sfx);
        }

        // 딕셔너리 초기화
        InitDictionary(bgmList, bgmDict);
        InitDictionary(sfxList, sfxDict);

        // 초기 볼륨 세팅
        SetMasterVolume(_masterVolume);
        SetBgmVolume(_bgmVolume);
        SetSfxVolume(_sfxVolume);

        // 테스트용 BGM 재생 (나중에 삭제 가능)
        PlayBGM("1");
    }


    void Update()
    {
        // 항상 볼륨 최신화
        bgmSource.volume = _bgmVolume;
        foreach (var sfx in sfxSources) sfx.volume = _sfxVolume;

        // UI 버튼 클릭 SFX 처리
        CheckUIButtonClick();
    }


    #region Dictionary Init
    private void InitDictionary<T>(List<T> list, Dictionary<string, AudioClip> dict) where T : class
    {
        dict.Clear();
        foreach (var item in list)
        {
            if (item is NamedBGM bgm && !dict.ContainsKey(bgm.name))
                dict.Add(bgm.name, bgm.clip);
            else if (item is NamedSFX sfx && !dict.ContainsKey(sfx.name))
                dict.Add(sfx.name, sfx.clip);
        }
    }
    #endregion

    #region Mixer Volume
    private float LinearToDecibel(float linear) => linear <= 0f ? -80f : Mathf.Log10(linear) * 20f;

    public void SetMasterVolume(float volume)
    {
        _masterVolume = Mathf.Clamp01(volume);
        audioMixer.SetFloat("Master_Volume", LinearToDecibel(_masterVolume));
    }

    public void SetBgmVolume(float volume)
    {
        _bgmVolume = Mathf.Clamp01(volume);
        audioMixer.SetFloat("BGM_Volume", LinearToDecibel(_bgmVolume));
    }

    public void SetSfxVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        audioMixer.SetFloat("SFX_Volume", LinearToDecibel(_sfxVolume));
    }
    #endregion

    #region BGM
    public void PlayBGM(string name)
    {
        if (!bgmDict.TryGetValue(name, out var clip)) return;
        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource.isPlaying)
            bgmSource.Stop();
    }

    public async UniTask FadeToBGM(string name, float duration)
    {
        if (!bgmDict.TryGetValue(name, out var newClip)) return;

        float startVolume = _bgmVolume;
        float time = 0f;

        // Fade Out
        while (time < duration)
        {
            time += Time.deltaTime;
            SetBgmVolume(Mathf.Lerp(startVolume, 0f, time / duration));
            await UniTask.Yield();
        }

        // 새로운 BGM 재생
        bgmSource.clip = newClip;
        bgmSource.Play();

        // Fade In
        time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            SetBgmVolume(Mathf.Lerp(0f, startVolume, time / duration));
            await UniTask.Yield();
        }
    }

    public async UniTask FadeOutBGM(float duration)
    {
        if (!bgmSource.isPlaying) return;

        float startVolume = _bgmVolume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            SetBgmVolume(Mathf.Lerp(startVolume, 0f, time / duration));
            await UniTask.Yield();
        }

        StopBGM();
        SetBgmVolume(startVolume); // 원래 볼륨 복원
    }
    #endregion

    #region SFX
    public void PlaySFX(string name)
    {
        if (!sfxDict.TryGetValue(name, out var clip)) return;

        sfxSources[currentSfxIndex].PlayOneShot(clip);
        currentSfxIndex = (currentSfxIndex + 1) % sfxPoolSize;
    }

    public void PlayClickSFX() => PlaySFX("Click");
    #endregion

    #region UI Click Check
    private void CheckUIButtonClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clicked = EventSystem.current.currentSelectedGameObject;
            if (clicked != null && clicked.GetComponent<Button>() != null)
                PlayClickSFX();
        }
    }
    #endregion
}