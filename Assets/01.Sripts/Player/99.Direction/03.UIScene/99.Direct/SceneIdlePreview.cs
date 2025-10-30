#if UNITY_EDITOR
using UnityEngine;

[ExecuteInEditMode]
public class SceneIdlePreview : MonoBehaviour
{
    public Animator animator;
    public string idleStateName = "Idle";

    void Update()
    {
        if (animator)
        {
            animator.Play(idleStateName, 0, 0f);
            animator.Update(0f);
        }
    }
}
#endif