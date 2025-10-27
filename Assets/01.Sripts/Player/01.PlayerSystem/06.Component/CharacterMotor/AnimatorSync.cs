using UnityEngine;

// 애니메이터와 연동
public class AnimatorSync : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;
    [SerializeField] private VerticalController vertical;

    private void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!controller) controller = GetComponent<CharacterController>();
        if (!vertical) vertical = GetComponent<VerticalController>();
    }

    private void Update()
    {
        //Vector3 horizontalVelocity = new Vector3(
        //    controller.velocity.x, 0f, controller.velocity.z);
        //
        //animator.SetFloat("Speed", horizontalVelocity.magnitude);
        //animator.SetFloat("Vertical", vertical.VerticalVelocity);
    }
}