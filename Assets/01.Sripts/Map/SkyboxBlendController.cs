using DG.Tweening;
using UnityEngine;

public class SkyboxBlendController : MonoBehaviour
{
    [Header("Blend Skybox Material (Shader = Skybox/PanoramicBlendSingle_URP)")]
    [SerializeField] private Material blendSkybox; // 스카이박스 머티리얼 (Inspector에서 직접 참조)

    [Header("Transition Settings")]
    [SerializeField] private float transitionTime = 3f; // 전환 시간

    [Header("Flip & Rotation")]
    [SerializeField] private bool flipX = false;
    [SerializeField] private bool flipY = false;
    [SerializeField][Range(0f, 1f)] private float blendOffset = 0.5f;
    [SerializeField][Range(0f, 360f)] private float rotationOffset = 270f;

    private bool isNight = false; // 밤 상태 여부

    [Header("Directional Light (Sun)")]
    public Light mainLight;

    // ===============================================================
    // Skybox 초기 설정
    // ===============================================================
    public void skyInitialize()
    {

        if (mainLight == null)
            mainLight = FindAnyObjectByType<Light>();

        //  Skybox 설정
        RenderSettings.skybox = blendSkybox;

        //  머티리얼 기본 값 세팅
        blendSkybox.SetFloat("_Blend", 0f);
        blendSkybox.SetFloat("_FlipX", flipX ? 1f : 0f);
        blendSkybox.SetFloat("_FlipY", flipY ? 1f : 0f);
        blendSkybox.SetFloat("_Rotation", rotationOffset);

        // URP 환경 업데이트
        DynamicGI.UpdateEnvironment();
    }

    // ===============================================================
    // 전투 시작 시 → 밤으로 전환
    // ===============================================================
    public void HandleBattleStart()
    {
        if (isNight) return;
        rotateSky();
        SetToNight();
    }

    // ===============================================================
    // 전투 종료(클리어) 시 → 저녁으로 전환
    // ===============================================================
    public void HandleBattleClear()
    {
        if(SaveManager.Instance.playerData.LastClearStage == MapManager.Instance.bossZoneId)
        {
            HandleAllClear();
            return;
        }
        if (!isNight) return;
        rotateSky();
        SetToSunset();
    }

    // ===============================================================
    // 올클리어 시 → 낮으로 전환
    // ===============================================================
    public void HandleAllClear()
    {
        rotateSky();
        SetToDay();
    }

    // ===============================================================
    // 저녁(기본 상태)
    // ===============================================================
    public void SetToSunset()
    {
        blendSkybox
            .DOFloat(0f, "_Blend", transitionTime)
            .SetEase(Ease.InOutSine)
            .OnUpdate(() => RenderSettings.skybox = blendSkybox);

        if (mainLight != null)
        {
            mainLight.transform
                .DORotate(new Vector3(15f, 270f, 0f), transitionTime)
                .SetEase(Ease.InOutSine);
        }

        isNight = false;
    }

    // ===============================================================
    // 낮 (최종 보스 클리어)
    // ===============================================================
    public void SetToDay()
    {
        blendSkybox
            .DOFloat(1f, "_Blend", transitionTime)
            .SetEase(Ease.InOutSine)
            .OnUpdate(() => RenderSettings.skybox = blendSkybox);

        if (mainLight != null)
        {
            mainLight.transform
                .DORotate(new Vector3(50f, 0f, 0f), transitionTime)
                .SetEase(Ease.InOutSine);
        }

        isNight = false;
    }

    // ===============================================================
    // 밤 (전투 시)
    // ===============================================================
    public void SetToNight()
    {
        blendSkybox
            .DOFloat(0.5f, "_Blend", transitionTime)
            .SetEase(Ease.InOutSine)
            .OnUpdate(() => RenderSettings.skybox = blendSkybox);

        if (mainLight != null)
        {
            mainLight.transform
                .DORotate(new Vector3(340f, 180f, 0f), transitionTime)
                .SetEase(Ease.InOutSine);
        }

        isNight = true;
    }

    // ===============================================================
    // 회전 전환
    // ===============================================================
    public void rotateSky()
    {
        float currentRotation = blendSkybox.GetFloat("_Rotation");
        float targetRotation = currentRotation + 360f; // 항상 한 바퀴 회전

        blendSkybox
            .DOFloat(targetRotation, "_Rotation", transitionTime)
            .SetEase(Ease.InOutSine)
            .OnUpdate(() =>
            {
                RenderSettings.skybox = blendSkybox;
                DynamicGI.UpdateEnvironment();
            });
    }
}
