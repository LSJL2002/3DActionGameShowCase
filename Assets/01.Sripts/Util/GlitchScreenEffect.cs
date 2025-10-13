using UnityEngine;

// 이 스크립트는 데미지 발생 시 화면 전체에 적용되는 글리치 효과의 강도를 제어합니다.
// URP Custom Renderer Feature가 '_GlobalGlitchIntensity' 속성을 읽어 사용해야 합니다.
public class GlitchScreenEffect : MonoBehaviour
{
    // URP 셰이더에 전역적으로 접근할 Float 속성 ID (성능 최적화)
    private static readonly int GlobalGlitchIntensityID = Shader.PropertyToID("_GlobalGlitchIntensity");

    [Header("Glitch Activation Settings")]
    [Tooltip("데미지를 입을 때 글리치 효과의 최대 강도 (0.0~1.0)")]
    public float maxIntensityOnHit = 0.8f;
    [Tooltip("글리치 효과가 자동으로 꺼지는 속도 (숫자가 높을수록 빠르게 감소)")]
    public float recoverySpeed = 5.0f;

    // 현재 글리치 강도
    private float currentIntensity = 0f;

    void Start()
    {
        // 시작 시 전역 강도를 0으로 설정하여 안전하게 초기화
        Shader.SetGlobalFloat(GlobalGlitchIntensityID, 0f);

        // [선택 사항] 씬에 이 스크립트의 인스턴스가 하나만 있는지 확인하는 싱글톤 로직 추가 가능
    }

    void Update()
    {
        // 1. 강도가 0보다 클 경우, deltaTime을 이용해 부드럽게 0으로 감소시킵니다.
        if (currentIntensity > 0)
        {
            currentIntensity = Mathf.MoveTowards(currentIntensity, 0f, Time.deltaTime * recoverySpeed);
        }

        // 2. 전역 셰이더 속성에 최종 강도를 적용합니다.
        // URP 렌더러 피처의 셰이더는 이 값을 프레임마다 읽습니다.
        Shader.SetGlobalFloat(GlobalGlitchIntensityID, currentIntensity);

        // --- [테스트용] ---
        if (Input.GetKeyDown(KeyCode.T))
        {
            ApplyDamageGlitch();
        }
    }

    /// <summary>
    /// 외부 데미지 시스템(예: PlayerHealth 스크립트)에서 호출하여 글리치 효과를 트리거합니다.
    /// </summary>
    public void ApplyDamageGlitch()
    {
        // 데미지를 입었을 때 순간적으로 최대 강도를 설정하고 무작위성을 부여합니다.
        currentIntensity = maxIntensityOnHit * Random.Range(0.9f, 1.0f);
    }

    void OnDisable()
    {
        // 스크립트 비활성화 시 효과 제거
        Shader.SetGlobalFloat(GlobalGlitchIntensityID, 0f);
    }
}
