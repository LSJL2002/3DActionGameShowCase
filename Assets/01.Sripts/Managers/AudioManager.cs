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
    [Range(0f, 1f)] public float volume = 1f;
}
[Serializable]
public class NamedSFX
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
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

    private Dictionary<string, NamedBGM> bgmDict = new();
    private Dictionary<string, NamedSFX> sfxDict = new();

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
        InitBgmDictionary(bgmList);
        InitSfxDictionary(sfxList);

        // 초기 볼륨 세팅
        SetMasterVolume(_masterVolume);
        SetBgmVolume(_bgmVolume);
        SetSfxVolume(_sfxVolume);

        // 테스트용 BGM 재생 (나중에 삭제 가능)
        PlayBGM("InGameBGM");
    }


    void Update()
    {
        // 항상 볼륨 최신화
        bgmSource.volume = _bgmVolume;
        foreach (var sfx in sfxSources) sfx.volume = _sfxVolume;

        // UI 버튼 클릭 SFX 처리
        CheckUIButtonHoverClick();
    }


    #region Dictionary Init
    private void InitBgmDictionary(List<NamedBGM> list)
    {
        bgmDict.Clear();
        foreach (var bgm in list)
        {
            if (!bgmDict.ContainsKey(bgm.name))
                bgmDict.Add(bgm.name, bgm);
        }
    }

    private void InitSfxDictionary(List<NamedSFX> list)
    {
        sfxDict.Clear();
        foreach (var sfx in list)
        {
            if (!sfxDict.ContainsKey(sfx.name))
                sfxDict.Add(sfx.name, sfx);
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
        if (!bgmDict.TryGetValue(name, out var bgmData)) return;
        if (bgmData.clip == null) return;

        // 이미 같은 클립 재생 중이면 무시
        if (bgmSource.clip == bgmData.clip && bgmSource.isPlaying) return;

        bgmSource.Stop();
        bgmSource.clip = bgmData.clip;
        bgmSource.volume = _bgmVolume * bgmData.volume; // 개별 볼륨 적용
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource.isPlaying)
            bgmSource.Stop();
    }

    public async UniTask FadeToBGM(string name, float duration)
    {
        if (!bgmDict.TryGetValue(name, out var newBgm)) return;

        float startVolume = _bgmVolume * (bgmSource.clip != null && bgmDict.ContainsValue(newBgm) ? newBgm.volume : 1f);
        float time = 0f;

        // Fade Out
        while (time < duration)
        {
            time += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            await UniTask.Yield();
        }

        // 새로운 BGM 재생
        bgmSource.clip = newBgm.clip;
        bgmSource.Play();

        // Fade In
        time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, _bgmVolume * newBgm.volume, time / duration);
            await UniTask.Yield();
        }
    }

    public async UniTask FadeOutBGM(float duration)
    {
        if (!bgmSource.isPlaying) return;

        float startVolume = bgmSource.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            await UniTask.Yield();
        }

        StopBGM();
    }
    #endregion

    #region SFX
    public void PlaySFX(string name)
    {
        if (!sfxDict.TryGetValue(name, out var sfxData)) return;
        if (sfxData.clip == null) return;

        var source = sfxSources[currentSfxIndex];
        source.PlayOneShot(sfxData.clip, _sfxVolume * sfxData.volume); // 개별 볼륨 적용
        currentSfxIndex = (currentSfxIndex + 1) % sfxPoolSize;
    }

    public void PlayClickSFX() => PlaySFX("Click");
    public void PlayHoverSFX() => PlaySFX("Hover");
    #endregion

    #region UI Hover & Click
    private GameObject lastHoveredButton;

    private void CheckUIButtonHoverClick()
    {
        // 현재 마우스가 가리키는 UI 버튼
        GameObject hoveredButton = GetHoveredButton();

        // Hover 사운드 (마우스가 버튼 위로 들어올 때만)
        if (hoveredButton != null && hoveredButton != lastHoveredButton)
        {
            PlayHoverSFX(); // Hover용 SFX 재생
            lastHoveredButton = hoveredButton;
        }

        // Hover 종료 시 초기화
        if (hoveredButton == null || hoveredButton.GetComponent<Button>() == null)
        {
            lastHoveredButton = null;
        }

        // Click 사운드
        if (Input.GetMouseButtonDown(0) && hoveredButton != null && hoveredButton.GetComponent<Button>() != null)
        {
            PlayClickSFX();
        }
    }

    // 마우스가 가리키는 버튼 가져오기
    private GameObject GetHoveredButton()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<Button>() != null)
                return result.gameObject;
        }

        return null;
    }
#endregion
}