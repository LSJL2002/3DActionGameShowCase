using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class ShaderContoller : MonoBehaviour
{
    [Header("Fade Settings")]
    public string fadeProperty = "_Fade";        // Shader 속성명
    [Range(0f, 1f)] public float startFade = 0f; //
    [Range(0f, 1f)] public float Fade = 0f;      // Inspector에서 직접 확인 가능
    public float FadeSpeed = 4f;                 // 1초에 0.5씩 올라감

    private Material _mat;

    void Awake()
    {
        Graphic graphic = GetComponent<Graphic>();
        if (graphic.material == null)
        {
            Debug.LogError("Material이 적용된 Graphic이 필요합니다!");
            enabled = false;
            return;
        }
        _mat = graphic.material;
        Fade = startFade;
        _mat.SetFloat(fadeProperty, Fade);
    }

    void OnEnable()
    {
        Fade = startFade;
        if (_mat != null)
            _mat.SetFloat(fadeProperty, Fade);
    }

    void Update()
    {
        if (_mat == null) return;

        if (Fade < 1f)
        {
            Fade += Time.unscaledDeltaTime * FadeSpeed;
            Fade = Mathf.Clamp01(Fade);
            _mat.SetFloat(fadeProperty, Fade);
        }
    }
}