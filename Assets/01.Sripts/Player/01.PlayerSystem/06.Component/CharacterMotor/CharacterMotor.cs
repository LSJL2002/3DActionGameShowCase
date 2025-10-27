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
    public bool AllowRotation { get; set; } = true;

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

        // 입력 기반 이동
        Vector3 inputDir = Vector3.zero;
        if (AllowMovement)
        {
            Vector3 moveInput = sm.MovementInput;
            Vector3 camF = player._camera.MainCamera.forward;
            Vector3 camR = player._camera.MainCamera.right;
            camF.y = camR.y = 0;
            inputDir = camF * moveInput.y + camR * moveInput.x;
            inputDir.Normalize();

            if (AllowRotation && inputDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(inputDir);
                player.transform.rotation = Quaternion.Slerp(
                    player.transform.rotation,
                    targetRot,
                    Time.deltaTime * sm.RotationDamping
                );
            }
        }

        // 입력 + 외부 힘 적용
        Vector3 move = inputDir * sm.MovementSpeed + force.HorizontalVelocity;
        move.y = vertical.VerticalVelocity;
        controller.Move(move * Time.deltaTime);

        // Animator에 속도 전달
        player.Animator.SetFloat(player.AnimationData.Velocity_XHash, move.x);
        player.Animator.SetFloat(player.AnimationData.Velocity_YHash, move.y);
        player.Animator.SetFloat(player.AnimationData.Velocity_ZHash, move.z);
        // Animator에 입력값 전달
        player.Animator.SetFloat(player.AnimationData.HorizontalHash, sm.MovementInput.x);
        player.Animator.SetFloat(player.AnimationData.VerticalHash, sm.MovementInput.y);
    }
}