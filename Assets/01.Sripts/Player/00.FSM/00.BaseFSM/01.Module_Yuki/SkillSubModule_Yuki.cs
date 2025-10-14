using UnityEngine;

public class SkillSubModule_Yuki
{
    private PlayerStateMachine sm;

    private Transform attackTarget;
    private Vector3 dashDir;
    private Vector3 returnDir;
    private float phaseTimer;

    private enum Phase { None, Dash, Wait, Return }
    private Phase phase = Phase.None;

    // ====== 설정값 ======
    private readonly float stopDistance = 2f;
    private readonly float dashPower = 12f;
    private readonly float returnPower = 8f;
    private readonly float dashDuration = 0.15f;
    private readonly float returnDuration = 0.1f;
    private readonly float waitTime = 0.8f;

    public SkillSubModule_Yuki(PlayerStateMachine sm)
    {
        this.sm = sm;
    }

    public void OnSkill()
    {
        var player = sm.Player;

        attackTarget = player.Attack.CurrentAttackTarget;

        // 애니메이션, 파티클
        var anim = player.AnimationData;
        player.Animator.SetTrigger(anim.SkillTriggerHash);
        player.vFX.StartDash();

        // 스킬 데이터 공격
        var skillData = player.InfoData.SkillData.GetSkillInfoData(0);
        player.Attack.OnAttack(
            skillData.SkillName,
            skillData.HitCount,
            skillData.Interval,
            skillData.DamageMultiplier
        );

        // 방향 설정
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
        sm.IsSkill = true;
    }

    public void OnSkillUpdate()
    {
        var player = sm.Player;
        phaseTimer += Time.deltaTime;

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
            Vector3 toTarget = attackTarget.position - player.transform.position;
            toTarget.y = 0f;
            float distance = toTarget.magnitude;

            if (distance <= stopDistance)
            {
                TransitionToPhase(Phase.Wait);
                return;
            }

            float moveDistance = Mathf.Min(dashPower * Time.deltaTime, distance - stopDistance);
            player.ForceReceiver.AddForce(toTarget.normalized * moveDistance / Time.deltaTime);
        }
        else
        {
            TransitionToPhase(Phase.Wait);
        }

        if (phaseTimer >= dashDuration)
            TransitionToPhase(Phase.Wait);
    }

    private void HandleWait(PlayerManager player)
    {
        player.ForceReceiver.Reset();
        if (phaseTimer >= waitTime)
            TransitionToPhase(Phase.Return);
    }

    private void HandleReturn(PlayerManager player)
    {
        player.ForceReceiver.AddForce(returnDir * returnPower);
        if (phaseTimer >= returnDuration)
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
        sm.IsSkill = false;
        phase = Phase.None;

        sm.ChangeState(sm.IdleState);
    }
}