using UnityEngine;

public class PlayerWalkState : PlayerGroundState
{
    private float transitionTimer = 0f; // 걷기 유지 시간 카운터
    private float blend = 0f;           // 0 -> 1 Blend

    public PlayerWalkState(PlayerStateMachine sm) : base(sm) { }

    public override void Enter()
    {
        base.Enter();

        // 이전 Blend가 있으면 복원, 없으면 0
        if (sm.LastWalkBlend > 0f)
        {
            blend = sm.LastWalkBlend;
            transitionTimer = sm.LastWalkTimer;

            // 복원 후 한 번 사용했으니 초기화 (다음 전환 대비)
            sm.LastWalkBlend = 0f;
            sm.LastWalkTimer = 0f;
        }
        else
        {
            blend = 0f;
            transitionTimer = 0f;
        }

        sm.MovementSpeedModifier = blend;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // 입력 방향 크기
        float inputMagnitude = sm.MovementInput.magnitude;

        // Blend 증가 (가속)
        transitionTimer += Time.deltaTime;
        float accelerationTime = Mathf.Max(sm.GroundData.RunAccelerationTime, 0.01f);
        blend = Mathf.Clamp01(transitionTimer / accelerationTime);
        // Blend Tree에 적용
        sm.MovementSpeedModifier = blend;

        // 이동 방향
        Vector3 moveDir = GetMovementDir();
        float moveSpeed = sm.MovementSpeed * sm.MovementSpeedModifier;

        // 루트모션 + 외부 힘 적용
        Vector3 deltaMove = sm.Player.Animator.deltaPosition;
        deltaMove.y = 0f; // 수직 이동 제외
        deltaMove += sm.Player.ForceReceiver.Movement;

        sm.Player.Controller.Move(deltaMove);
    }
}