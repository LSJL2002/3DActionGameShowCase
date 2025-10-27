using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;

    private float rootMotionMultiplier = 1f;

    void OnAnimatorMove()
    {
        Vector3 delta = animator.deltaPosition * rootMotionMultiplier;
        if (controller != null && controller.enabled)
            controller.Move(delta);
        else
            transform.position += delta;

        transform.rotation *= animator.deltaRotation;
    }

    public void SetRootMotionMultiplier(float multiplier)
    {
        rootMotionMultiplier = multiplier;
    }

    public void ResetRootMotionMultiplier()
    {
        rootMotionMultiplier = 1f;
    }
}
