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

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Blend 증가 (가속)
        transitionTimer += Time.deltaTime;
        float accelerationTime = Mathf.Max(sm.GroundData.RunAccelerationTime, 0.01f);
        blend = Mathf.Clamp01(transitionTimer / accelerationTime);

        // Blend Tree에 적용
        sm.MovementSpeedModifier = blend;
    }
}