using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
    [SerializeField] private AudioMixer audioMixer; // MainMixer
    [SerializeField] private AudioMixerGroup bgmGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("BGM Clips")]
    [SerializeField] private List<NamedBGM> bgmList;

    [Header("SFX Clips")]
    [SerializeField] private List<NamedSFX> sfxList;
    public int sfxPoolSize = 5; //동시재생가능한수

    private List<AudioSource> sfxSources = new List<AudioSource>();
    private int currentSfxIndex = 0;
    private AudioSource bgmSource;

    private Dictionary<string, AudioClip> bgmDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();

    [SerializeField, Range(0f, 1f)] private float _masterVolume = 1f;

    [Range(0f, 1f)]
    [SerializeField] private float _bgmVolume = 0.5f;
    public float bgmVolume => _bgmVolume;
    [Range(0f, 1f)]
    [SerializeField] private float _sfxVolume = 0.5f;
    public float sfxVolume => _bgmVolume;


    private void Awake()
    {
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
        foreach (var bgm in bgmList)
        {
            if (!bgmDict.ContainsKey(bgm.name))
                bgmDict.Add(bgm.name, bgm.clip);
        }

        foreach (var sfx in sfxList)
        {
            if (!sfxDict.ContainsKey(sfx.name))
                sfxDict.Add(sfx.name, sfx.clip);
        }

        SetMasterVolume(_masterVolume);
        SetBgmVolume(_bgmVolume);
        SetSfxVolume(_sfxVolume);

        PlayBGM("1");

        float masterDB;
        if (audioMixer.GetFloat("Master_Volume", out masterDB))
            Debug.Log("Master dB after Awake: " + masterDB);
        else
            Debug.LogWarning("Master_Volume parameter not found in AudioMixer!");
    }

    // 원래 씬로드시 씬네임을 매개변수로 받아와서 씬네임과 같은 이름의 BGM을 재생하는 방식이었으나,
    // 현재 씬로드매니저를 따로 만들었기때문에 방식이 약간 변경돼서 일단 주석처리 추후 BGM 플레이 방식을 변경 필요
    // 예정 : 각 씬에 존재하는 씬오브젝트 Start 함수에서 재생
    //private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    //private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if (Enum.TryParse(scene.name, out GameScene gameScene))
    //    {
    //        string sceneName = SceneUtility.GetSceneName(gameScene);
    //        PlayBGM(sceneName);
    //    }
    //}

    void Update()
    {
        // 항상 볼륨 최신화 (옵션에서 슬라이더로 조절 시 반영되게)
        bgmSource.volume = bgmVolume;
        foreach (var sfx in sfxSources) sfx.volume = _sfxVolume;

        // 버튼 클릭 SFX
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clicked = EventSystem.current.currentSelectedGameObject;
            if (clicked != null && clicked.GetComponent<Button>() != null)
                PlayClickSFX();
        }
    }

    #region Mixer Volume Control
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

    public async UniTask FadeToBGM(string name, float duration)
    {
        if (!bgmDict.TryGetValue(name, out var newClip)) return;

        float startVolume = _bgmVolume;
        float time = 0f;

        // 페이드 아웃
        while (time < duration)
        {
            time += Time.deltaTime;
            SetBgmVolume(Mathf.Lerp(startVolume, 0f, time / duration));
            await UniTask.Yield();
        }

        // 새 클립 재생
        bgmSource.clip = newClip;
        bgmSource.Play();

        // 페이드 인
        time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            SetBgmVolume(Mathf.Lerp(0f, startVolume, time / duration));
            await UniTask.Yield();
        }
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
}