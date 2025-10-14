using System;
using UnityEngine;

public class SkillSubModule_Yuki
{
    public event Action OnSkillEnd; // FSM에게 신호 전달용

    private PlayerStateMachine sm;

    private Transform attackTarget;
    private Vector3 dashDir, returnDir;
    private float phaseTimer;

    private enum Phase { None, Dash, Wait, Return }
    private Phase phase = Phase.None;

    // 설정값
    private const float StopDistance = 2f;
    private const float DashPower = 12f;
    private const float ReturnPower = 8f;
    private const float DashDuration = 0.15f;
    private const float ReturnDuration = 0.1f;
    private const float WaitTime = 0.8f;

    public SkillSubModule_Yuki(PlayerStateMachine sm) => this.sm = sm;

    public void OnSkill()
    {
        var player = sm.Player;
        attackTarget = player.Attack.CurrentAttackTarget;

        player.Animator.CrossFade("Skill1", 0.1f);
        player.vFX.StartDash();

        var skillData = player.InfoData.SkillData.GetSkillInfoData(0);
        player.Attack.OnAttack(skillData.SkillName, skillData.HitCount, skillData.Interval, skillData.DamageMultiplier);

        if (attackTarget != null)
        {
            dashDir = (attackTarget.position - player.transform.position).normalized;
            dashDir.y = 0f;

            if (dashDir.sqrMagnitude > 0.01f)
                player.transform.rotation = Quaternion.LookRotation(dashDir);

            returnDir = -dashDir;
            phase = Phase.Dash;
        }
        else
        {
            dashDir = Vector3.zero;
            returnDir = -player.transform.forward;
            returnDir.y = 0f;
            phase = Phase.Wait;
        }

        phaseTimer = 0f;
    }

    public void OnSkillCanceled()
    {
        ExitSkill(sm.Player);
    }

    public void OnSkillUpdate()
    {
        phaseTimer += Time.deltaTime;
        var player = sm.Player;

        switch (phase)
        {
            case Phase.Dash: HandleDash(player); break;
            case Phase.Wait: HandleWait(player); break;
            case Phase.Return: HandleReturn(player); break;
        }
    }

    private void HandleDash(PlayerManager player)
    {
        if (attackTarget != null)
        {
            var toTarget = attackTarget.position - player.transform.position;
            toTarget.y = 0f;
            float distance = toTarget.magnitude;

            if (distance <= StopDistance)
            {
                TransitionToPhase(Phase.Wait);
                return;
            }

            float moveDistance = Mathf.Min(DashPower * Time.deltaTime, distance - StopDistance);
            player.ForceReceiver.AddForce(toTarget.normalized * moveDistance / Time.deltaTime);
        }
        else
        {
            TransitionToPhase(Phase.Wait);
        }

        if (phaseTimer >= DashDuration)
            TransitionToPhase(Phase.Wait);
    }

    private void HandleWait(PlayerManager player)
    {
        player.ForceReceiver.Reset();
        if (phaseTimer >= WaitTime)
            TransitionToPhase(Phase.Return);
    }

    private void HandleReturn(PlayerManager player)
    {
        player.ForceReceiver.AddForce(returnDir * ReturnPower);
        if (phaseTimer >= ReturnDuration)
        {
            player.ForceReceiver.Reset();
            ExitSkill(player);
        }
    }

    private void TransitionToPhase(Phase next)
    {
        phase = next;
        phaseTimer = 0f;
        sm.Player.ForceReceiver.Reset();
    }

    private void ExitSkill(PlayerManager player)
    {
        if (phase == Phase.None) return;

        player.vFX.StopDash();
        player.ForceReceiver.Reset();
        phase = Phase.None;

        // 이벤트로 FSM에게 종료 알림
        OnSkillEnd?.Invoke();
    }
}