using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

// 중력 / 점프 / 공중 유지
public class VerticalController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    private Animator anim;

    [Header("Settings")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float holdLerpSpeed = 8f;

    private float verticalVelocity;
    private bool holdActive;
    private float holdTargetY;
    private float holdEndTime;

    public float VerticalVelocity => verticalVelocity;

    public void Initialize(PlayerCharacter pc)
    {
        controller = pc.Controller;
        anim = pc.Animator;
    }

    public void UpdateVertical()
    {
        if (holdActive)
        {
            float currentY = controller.transform.position.y;
            float newY = Mathf.Lerp(currentY, holdTargetY, holdLerpSpeed * Time.deltaTime);
            verticalVelocity = (newY - currentY) / Time.deltaTime;

            // 목표 높이에 도달하면 속도 0으로 고정
            // 목표 높이에 도달해도 holdActive 유지 (시간 끝나면만 종료)
            if (Time.time >= holdEndTime)
            {
                holdActive = false;
                anim.applyRootMotion = true;
            }
            else if (Mathf.Approximately(newY, holdTargetY))
            {
                verticalVelocity = 0f; // 계속 떠 있게
            }
        }
        else
        {
            if (controller.isGrounded)
            {
                verticalVelocity = -0.1f; // verticalVelocity를 0으로 초기화
            }
            else
                verticalVelocity += gravity * Time.deltaTime;
        }
    }

    public void Jump(float force)
    {
        if (controller.isGrounded)
            verticalVelocity = force;
    }

    // ================= Hover / 공중 연출 =================
    public void Hold(float targetHeight, float duration)
    {
        holdTargetY = targetHeight;
        holdEndTime = Time.time + duration;
        holdActive = true;
        anim.applyRootMotion = false;
    }
}