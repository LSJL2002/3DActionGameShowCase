using UnityEngine;


// 중앙 이동 처리 담당
[RequireComponent(typeof(CharacterController))]
public class CharacterMotor : MonoBehaviour
{
    [Header("References")]
    private PlayerCharacter player;
    private CharacterController controller;
    private ForceReceiver force;
    private VerticalController vertical;
    public PlayerStateMachine sm;

    public bool AllowMovement { get; set; } = true;
    public bool AllowInput { get; set; } = true;

    public void Initialize(PlayerCharacter pc)
    {
        player = pc;
        controller = pc.Controller;
        force = pc.ForceReceiver;
        vertical = pc.Vertical;
        this.sm = pc.StateMachine;
    }

    void Update()
    {
        vertical.UpdateVertical(); // 중력, 점프, 공중 유지
        force.UpdateForce(); // 넉백 & 외부 힘 감속

        Vector3 inputDir = Vector3.zero;
        Vector2 moveInput = sm.MovementInput;

        // 1. 카메라 기준 방향 계산 (입력 있을 때만)
        if (moveInput.sqrMagnitude > 0.01f)
        {
            Vector3 camF = player._camera.MainCamera.forward;
            Vector3 camR = player._camera.MainCamera.right;
            camF.y = camR.y = 0;
            camF.Normalize();
            camR.Normalize();

            inputDir = (camF * moveInput.y + camR * moveInput.x).normalized;
        }

        // 2. 실제 회전 (AllowMovement에 종속됨)
        if (AllowMovement && inputDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(inputDir);
            player.transform.rotation = Quaternion.Slerp(
                player.transform.rotation,
                targetRot,
                Time.deltaTime * sm.RotationDamping
            );
        }

        // 3. 이동
        Vector3 move = Vector3.zero;

        if (AllowMovement)
            move = inputDir * sm.MovementSpeed;

        move += force.HorizontalVelocity;
        move.y = vertical.VerticalVelocity;

        controller.Move(move * Time.deltaTime);

        // 4. Animator 파라미터 처리
        if (AllowInput) // ✅ 이제 이게 "Animator 업데이트 허용" 역할
        {
            // 실제 이동 속도
            player.Animator.SetFloat(player.AnimationData.Velocity_XHash, move.x);
            player.Animator.SetFloat(player.AnimationData.Velocity_YHash, move.y);
            player.Animator.SetFloat(player.AnimationData.Velocity_ZHash, move.z);

            // 입력값 (WASD/스틱)
            player.Animator.SetFloat(player.AnimationData.HorizontalHash, sm.MovementInput.x);
            player.Animator.SetFloat(player.AnimationData.VerticalHash, sm.MovementInput.y);
        }
        // else: 회피/공격 중에는 값 변경 안 함 (기존 값 유지 -> BlendTree 안 튐)
    }
}