using UnityEngine;
using DG.Tweening;

public class SkyboxBlendController : MonoBehaviour
{
    [Header("Blend Skybox Material (Shader = Skybox/PanoramicBlend)")]
    [SerializeField] private Material blendSkybox;

    [Header("Transition Settings")]
    [SerializeField] private float transitionTime = 3f;

    [Header("Flip & Rotation")]
    [SerializeField] private bool flipX = false;
    [SerializeField] private bool flipY = false;
    [SerializeField][Range(0f, 360f)] private float rotationOffset = 0f;
    [SerializeField][Range(0f, 1f)] private float blendOffset = 0f;

    [Header("Directional Light (Sun)")]
    public Light mainLight;
    private void Awake()
    {
        // 씬에 Directional Light 하나만 있으면 자동으로 찾아오기
        if (mainLight == null)
            mainLight = FindObjectOfType<Light>();
    }
    private void Start()
    {
        // Skybox 적용
        RenderSettings.skybox = blendSkybox;

        // 초기 세팅
        blendSkybox.SetFloat("_Blend", 0f); // 낮 시작
        blendSkybox.SetFloat("_FlipX", flipX ? 1f : 0f);
        blendSkybox.SetFloat("_FlipY", flipY ? 1f : 0f);
        blendSkybox.SetFloat("_Rotation", rotationOffset);


    }

    private void OnEnable()
    {
        BattleManager.OnBattleStart += HandleBattleStart;
        BattleManager.OnBattleClear += HandleBattleClear;
    }

    private void OnDisable()
    {
        BattleManager.OnBattleStart -= HandleBattleStart;
        BattleManager.OnBattleClear -= HandleBattleClear;
    }

    private void HandleBattleStart(BattleZone zone)
    {
        // 낮 → 밤 전환
        blendSkybox.DOFloat(1f, "_Blend", transitionTime).SetEase(Ease.InOutSine);
        blendSkybox.DOFloat(270f, "_Rotation",transitionTime).SetEase(Ease.InOutSine);

        if (mainLight != null)
        {
            // 빛도 같이 회전
            mainLight.transform.DORotate(new Vector3(0f, 270f, 0f), transitionTime)
                             .SetEase(Ease.InOutSine);
        }
    }

    private void HandleBattleClear(BattleZone zone)
    {
        // 밤 → 낮 전환
        blendSkybox.DOFloat(0f, "_Blend", transitionTime).SetEase(Ease.InOutSine);
        blendSkybox.DOFloat(0f, "_Rotation", transitionTime).SetEase(Ease.InOutSine);

        if (mainLight != null)
        {
            mainLight.transform
                .DORotate(Vector3.zero, transitionTime) // (0,0,0)
                .SetEase(Ease.InOutSine);
        }
    }

    // 필요하면 런타임 중 Rotation도 Tween 가능
    public void SetRotation(float targetRotation, float duration = 1f)
    {
        blendSkybox.DOFloat(targetRotation, "_Rotation", duration).SetEase(Ease.InOutSine);
    }

    // Flip 값 런타임 변경
    public void SetFlip(bool flipXValue, bool flipYValue)
    {
        blendSkybox.SetFloat("_FlipX", flipXValue ? 1f : 0f);
        blendSkybox.SetFloat("_FlipY", flipYValue ? 1f : 0f);
    }
}
