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
    [SerializeField][Range(0f, 1f)] private float blendOffset = 0f;
    [SerializeField][Range(0f, 360f)] private float rotationOffset = 0f;

    private bool isNight = false;

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
        skyInitialize();
        blendSkybox.SetFloat("_Blend", blendOffset);
        blendSkybox.SetFloat("_Rotation", rotationOffset);
    }

    private void skyInitialize()
    {
        blendSkybox.SetFloat("_Blend", 0f); // 낮 시작
        blendSkybox.SetFloat("_FlipX", flipX ? 1f : 0f);
        blendSkybox.SetFloat("_FlipY", flipY ? 1f : 0f);
        blendSkybox.SetFloat("_Rotation", rotationOffset);
    }

    private void OnEnable()
    {
        //BattleManager.OnBattleStart += HandleBattleStart;
        //BattleManager.OnBattleClear += HandleBattleClear;
    }

    private void OnDisable()
    {
        //BattleManager.OnBattleStart -= HandleBattleStart;
        //BattleManager.OnBattleClear -= HandleBattleClear;
    }

    public void HandleBattleStart()
    {
        rotateSky();
        ToggleSky();
        
    }

    public void HandleBattleClear()
    {
        rotateSky();
        ToggleSky();
    }

    public void SetToDay()
    {
        blendSkybox.SetFloat("_Blend", 0f); // 낮 시작
        blendSkybox.SetFloat("_Rotation", 0f);
    }

    public void SetToNight()
    {
        blendSkybox.SetFloat("_Blend", 1f); 
        blendSkybox.SetFloat("_Rotation", 270f);
    }

    // 필요하면 런타임 중 Rotation도 Tween 가능
    public void ToggleSky()
    {
        // 낮(0) ↔ 밤(1) 토글
        float targetBlend = isNight ? 0f : 1f;
        

        blendSkybox
            .DOFloat(targetBlend, "_Blend", transitionTime)
            .SetEase(Ease.InOutSine);

        
        isNight = !isNight; // 상태 반전

        if (mainLight != null)
        {
            Vector3 lightRotation = isNight ? Vector3.zero : new Vector3(0f, 270f, 0f);
            mainLight.transform
                .DORotate(lightRotation, transitionTime)
                .SetEase(Ease.InOutSine);
        }
    }  

    public void rotateSky()
    {
        float targetRotation = isNight ? 0f : 270;

        blendSkybox
            .DOFloat(targetRotation, "_Rotation", transitionTime)
            .SetEase(Ease.InOutSine);
    }
}
