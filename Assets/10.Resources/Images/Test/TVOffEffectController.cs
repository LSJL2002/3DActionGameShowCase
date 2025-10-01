using UnityEngine;

public class TVOffEffectController : MonoBehaviour
{
    public Material tvMaterial;   // 적용할 머티리얼
    [SerializeField] private float duration = 1.0f; // 꺼지는 데 걸리는 시간
    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private float elapsed = 0f;
    private bool isPlaying = false;

    private void Reset()
    {
        // 자동으로 Renderer 머티리얼 가져오기
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            tvMaterial = rend.material;
        }
    }

    void Update()
    {
        if (!isPlaying) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);

        // AnimationCurve를 이용해 자연스럽게 꺼지는 속도 제어
        float scaleFactor = curve.Evaluate(t);

        tvMaterial.SetFloat("_ScaleFactor", scaleFactor);

        if (t >= 1f)
        {
            isPlaying = false;
        }
    }

    /// <summary>
    /// TV 끄기 애니메이션 시작
    /// </summary>
    public void Play()
    {
        elapsed = 0f;
        isPlaying = true;
    }
}
