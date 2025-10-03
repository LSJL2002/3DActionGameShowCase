using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoManager : MonoBehaviour
{
    [Header("Source (clip OR url)")]
    [Tooltip("If clip is set it will be used. Otherwise URL will be used if non-empty.")]
    public VideoClip clip;
    [Tooltip("Use if you want to stream or play a file by url.")]
    public string url;

    [Header("Output")]
    public RawImage targetRawImage;          // UI 출력 (optional)
    public Renderer targetRenderer;          // 3D 오브젝트 메터리얼에 출력 (optional)
    public string targetMaterialTextureName = "_MainTex"; // 렌더러에 쓸 텍스처 이름

    [Header("Audio")]
    public bool useAudioSource = true;
    public AudioSource audioSource;          // optional: 연결된 AudioSource (루프/볼륨 등 제어에 사용)
    public bool audioMute = false;

    [Header("Playback")]
    public bool playOnStart = true;
    public bool loop = false;
    [Range(0.25f, 3f)] public float playbackSpeed = 1f;

    // runtime
    VideoPlayer vp;
    bool prepared = false;

    void Awake()
    {
        vp = GetComponent<VideoPlayer>();
        SetupVideoPlayerDefaults();
    }

    void Start()
    {
        // assign clip or URL
        if (clip != null)
        {
            vp.source = VideoSource.VideoClip;
            vp.clip = clip;
        }
        else if (!string.IsNullOrEmpty(url))
        {
            vp.source = VideoSource.Url;
            vp.url = url;
        }

        vp.playOnAwake = false;
        vp.isLooping = loop;
        vp.playbackSpeed = playbackSpeed;

        // audio setup
        if (useAudioSource)
        {
            // if no audioSource specified, try to find on the same GameObject
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                // create temporary AudioSource
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }

            vp.audioOutputMode = VideoAudioOutputMode.AudioSource;
            vp.EnableAudioTrack(0, true);
            vp.SetTargetAudioSource(0, audioSource);
            audioSource.mute = audioMute;
        }
        else
        {
            vp.audioOutputMode = VideoAudioOutputMode.None;
        }

        vp.prepareCompleted += OnPrepareCompleted;
        vp.errorReceived += OnErrorReceived;

        // prepare then optionally play
        StartCoroutine(PrepareAndMaybePlay());
    }

    void SetupVideoPlayerDefaults()
    {
        vp.playOnAwake = false;
        vp.renderMode = VideoRenderMode.APIOnly; // we'll push texture to UI/Renderer
        vp.waitForFirstFrame = true;
        vp.skipOnDrop = true;
    }

    IEnumerator PrepareAndMaybePlay()
    {
        prepared = false;

        // If neither clip nor url set, do nothing
        if (vp.clip == null && string.IsNullOrEmpty(vp.url))
        {
            Debug.LogWarning($"[{name}] VideoBox: No clip or URL assigned.");
            yield break;
        }

        vp.Prepare();

        // wait until prepared or error
        float timeout = 10f;
        float t = 0f;
        while (!vp.isPrepared && t < timeout)
        {
            t += Time.deltaTime;
            yield return null;
        }

        if (!vp.isPrepared)
        {
            Debug.LogWarning($"[{name}] VideoBox: Video failed to prepare within {timeout} seconds (or isStillPreparing).");
            yield break;
        }

        // assign texture to outputs
        AssignTextureToTargets();

        prepared = true;
        vp.isLooping = loop;
        vp.playbackSpeed = playbackSpeed;

        if (playOnStart)
        {
            Play();
        }
    }

    void AssignTextureToTargets()
    {
        var tex = vp.texture;
        if (tex == null)
        {
            // texture may be null until first frame; try attach in Update as fallback
            StartCoroutine(AssignTextureNextFrame());
            return;
        }

        if (targetRawImage != null)
        {
            targetRawImage.texture = tex;
        }

        if (targetRenderer != null)
        {
            if (targetRenderer.material != null)
                targetRenderer.material.SetTexture(targetMaterialTextureName, tex);
        }
    }

    IEnumerator AssignTextureNextFrame()
    {
        // wait until texture available
        float wait = 2f;
        float t = 0f;
        while (vp.texture == null && t < wait)
        {
            t += Time.deltaTime;
            yield return null;
        }

        var tex = vp.texture;
        if (tex == null) yield break;

        if (targetRawImage != null) targetRawImage.texture = tex;
        if (targetRenderer != null && targetRenderer.material != null)
            targetRenderer.material.SetTexture(targetMaterialTextureName, tex);
    }

    void OnPrepareCompleted(VideoPlayer source)
    {
        // optional callback
        Debug.Log($"[{name}] Video prepared. duration: {vp.length:F2}s, widthxheight: {vp.texture?.width}x{vp.texture?.height}");
        AssignTextureToTargets();
    }

    void OnErrorReceived(VideoPlayer source, string message)
    {
        Debug.LogError($"[{name}] VideoPlayer error: {message}");
    }

    void OnDestroy()
    {
        if (vp != null)
        {
            vp.prepareCompleted -= OnPrepareCompleted;
            vp.errorReceived -= OnErrorReceived;
        }
    }

    // --- public control API ---
    public void Play()
    {
        if (!prepared && !vp.isPrepared)
        {
            // try prepare then play
            StartCoroutine(PrepareThenPlay());
            return;
        }

        if (vp.isPlaying) return;
        vp.Play();
        if (useAudioSource && audioSource != null) audioSource.Play();
    }

    IEnumerator PrepareThenPlay()
    {
        if (!vp.isPrepared) vp.Prepare();
        float timeout = 10f;
        float t = 0f;
        while (!vp.isPrepared && t < timeout)
        {
            t += Time.deltaTime;
            yield return null;
        }
        AssignTextureToTargets();
        vp.Play();
        if (useAudioSource && audioSource != null) audioSource.Play();
        prepared = true;
    }

    public void Pause()
    {
        if (vp.isPlaying) vp.Pause();
        if (useAudioSource && audioSource != null && audioSource.isPlaying) audioSource.Pause();
    }

    public void Stop()
    {
        vp.Stop();
        if (useAudioSource && audioSource != null) audioSource.Stop();
    }

    public void TogglePlayPause()
    {
        if (vp.isPlaying) Pause();
        else Play();
    }

    public void SeekSeconds(double seconds)
    {
        if (!vp.canSetTime)
        {
            Debug.LogWarning($"[{name}] VideoPlayer cannot set time for this source.");
            return;
        }
        seconds = Mathf.Clamp((float)seconds, 0f, (float)vp.length);
        vp.time = seconds;
    }

    public void SetLoop(bool shouldLoop)
    {
        loop = shouldLoop;
        vp.isLooping = shouldLoop;
    }

    public void SetPlaybackSpeed(float speed)
    {
        playbackSpeed = Mathf.Max(0.01f, speed);
        vp.playbackSpeed = playbackSpeed;
    }

    // Replace the clip at runtime and optionally play
    public void SetClipAndPlay(VideoClip newClip, bool playImmediately = true)
    {
        clip = newClip;
        vp.source = VideoSource.VideoClip;
        vp.clip = newClip;
        prepared = false;
        StartCoroutine(PrepareThenMaybePlay(playImmediately));
    }

    // Use URL at runtime
    public void SetUrlAndPlay(string newUrl, bool playImmediately = true)
    {
        url = newUrl;
        vp.source = VideoSource.Url;
        vp.url = newUrl;
        prepared = false;
        StartCoroutine(PrepareThenMaybePlay(playImmediately));
    }

    IEnumerator PrepareThenMaybePlay(bool playImmediately)
    {
        vp.Prepare();
        float timeout = 10f;
        float t = 0f;
        while (!vp.isPrepared && t < timeout)
        {
            t += Time.deltaTime;
            yield return null;
        }

        AssignTextureToTargets();
        prepared = true;
        if (playImmediately) Play();
    }
}