using UnityEngine;

public class GlitchFadeControl : MonoBehaviour
{
    public Material material;
    public float duration = 2f;
    float time = 0f;

    void Update()
    {
        time += Time.deltaTime;
        float fade = Mathf.Clamp01(1 - (time / duration));

        material.SetFloat("_Fade", fade); // 사라짐
        material.SetFloat("_GlitchAmount", Random.Range(0f, fade)); // 글리치 정도
    }
}
