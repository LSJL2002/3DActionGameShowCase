#if UNITY_EDITOR
using UnityEngine;

[ExecuteInEditMode] // 에디터 모드에서만 동작
public class SceneIdlePreview : MonoBehaviour
{
    public Animator animator;
    public string idleClipName = "Idle";

    void Update()
    {
        if (animator)
        {
            animator.Play(idleClipName, 0, 0f); // Idle 애니메이션 강제 적용
            animator.Update(0f); // 씬뷰에 바로 반영
        }
    }
}
#endif