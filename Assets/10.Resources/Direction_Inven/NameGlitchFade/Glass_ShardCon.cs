using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ShardFadeController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float Fade = 0f;           // Inspector에서 직접 확인 가능
    private float FadeSpeed = 2f;    // 1초에 0.5씩 올라감

    private Material _mat;

    void Awake()
    {
        Image img = GetComponent<Image>();
        if (img.material == null)
        {
            Debug.LogError("Material이 적용된 Image가 필요합니다!");
            enabled = false;
            return;
        }
        _mat = img.material;
    }

    void OnEnable()
    {
        // 켜질 때마다 초기화
        Fade = 0f;
        _mat.SetFloat("_Fade", Fade);
    }

    void Update()
    {
        if (Fade < 1f)
        {
            Fade += Time.deltaTime * FadeSpeed;
            Fade = Mathf.Clamp01(Fade);
            _mat.SetFloat("_Fade", Fade);
        }
    }
}