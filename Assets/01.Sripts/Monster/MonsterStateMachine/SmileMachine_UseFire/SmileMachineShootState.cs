using System.Collections;
using UnityEngine;

public class SmileMachineShootState : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private Coroutine shootRoutine;

    public SmileMachineShootState(MonsterStateMachine ms, MonsterSkillSO shootSkill) : base(ms)
    {
        skillData = shootSkill;
    }

    public override void Enter()
    {
        stateMachine.isAttacking = true; // Lock pattern system
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Ready));
        shootRoutine = stateMachine.Monster.StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        // --- Ready phase ---
        yield return new WaitForSeconds(skillData.preCastTime);

        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Ready));
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill4));

        float shootDuration = 4f;
        float timer = 0f;

        // --- Shooting loop ---
        while (timer < shootDuration)
        {
            timer += Time.deltaTime;
            RotateTowardsPlayer();
            DealDamageInFront();
            yield return null;
        }

        // --- Finish shooting ---
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill4));

        // Release attack lock so pattern system continues
        stateMachine.isAttacking = false;

        // Return to Idle
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }

    private void RotateTowardsPlayer()
    {
        if (stateMachine.Monster.PlayerTarget == null) return;

        Vector3 dir = stateMachine.Monster.PlayerTarget.position - stateMachine.Monster.transform.position;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            float rotateSpeed = 5f; // smoother rotation
            stateMachine.Monster.transform.rotation = Quaternion.Slerp(
                stateMachine.Monster.transform.rotation,
                targetRot,
                Time.deltaTime * rotateSpeed
            );
        }
    }

    private void DealDamageInFront()
    {
        Vector3 origin = stateMachine.Monster.transform.position + Vector3.up * 1.5f;
        Vector3 direction = stateMachine.Monster.transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, skillData.range))
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var target))
            {
                target.OnTakeDamage(stateMachine.Monster.Stats.AttackPower);
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit SmileMachineShootState");
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Ready));
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill4));

        // Stop only this coroutine to avoid breaking other patterns
        if (shootRoutine != null)
        {
            stateMachine.Monster.StopCoroutine(shootRoutine);
            shootRoutine = null;
        }

        // Ensure isAttacking is reset (failsafe)
        stateMachine.isAttacking = false;
    }
}
