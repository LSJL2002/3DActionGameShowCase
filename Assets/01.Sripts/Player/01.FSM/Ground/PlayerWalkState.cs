using UnityEngine;

public class PlayerWalkState : PlayerGroundState
{
    private float transitionTimer = 0f; // 걷기 유지 시간 카운터
    private float blend = 0f;           // 0 -> 1 Blend

    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        // 이전 Blend가 있으면 복원, 없으면 0
        if (stateMachine.LastWalkBlend > 0f)
        {
            blend = stateMachine.LastWalkBlend;
            transitionTimer = stateMachine.LastWalkTimer;

            // 복원 후 한 번 사용했으니 초기화 (다음 전환 대비)
            stateMachine.LastWalkBlend = 0f;
            stateMachine.LastWalkTimer = 0f;
        }
        else
        {
            blend = 0f;
            transitionTimer = 0f;
        }

        // Animator.speed 제거!
        // 대신 MovementSpeedModifier 파라미터만 업데이트
        stateMachine.MovementSpeedModifier = blend;
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


        float inputMagnitude = stateMachine.MovementInput.magnitude;

        // 입력 없으면 Idle 상태로 전환
        if (inputMagnitude <= 0.01f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        // Blend 값 증가
        transitionTimer += Time.deltaTime;
        float accelerationTime = stateMachine.GroundData.RunAccelerationTime;
        blend = Mathf.Clamp01(transitionTimer / accelerationTime);

        // MovementSpeedModifier를 이용해 Blend Tree에 적용
        stateMachine.MovementSpeedModifier = blend;

        // 루트모션 이동
        Vector3 deltaPosition = stateMachine.Player.Animator.deltaPosition;
        deltaPosition.y = 0f;
        stateMachine.Player.Controller.Move(deltaPosition);
    }
}