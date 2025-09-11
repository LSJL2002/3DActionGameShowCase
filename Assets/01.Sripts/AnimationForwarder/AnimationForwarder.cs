using UnityEngine;

public class AnimationEventForwarder : MonoBehaviour
{
    public BaseMonster parentMonster;

    private void Awake()
    {
        // Try to automatically get the correct parent if not set manually
        if (parentMonster)
        {
            parentMonster = GetComponentInParent<BaseMonster>();
        }
    }

    // Called by the animation event
    public void OnAttackAnimationComplete()
    {
        if (parentMonster != null)
        {
            parentMonster.OnAttackAnimationComplete();
        }
    }

    // Called by the animation event
    public void OnAttackHit()
    {
        if (parentMonster != null)
        {
            parentMonster.OnAttackHit();
        }
    }
}
