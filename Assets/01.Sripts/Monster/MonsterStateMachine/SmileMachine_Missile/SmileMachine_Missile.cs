using System.Collections;
using UnityEngine;

public class SmileMachine_Missile : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private Coroutine missileRoutine;
    private LineRenderer lineRenderer;
    private Transform firePoint;
    private Transform player;

    public SmileMachine_Missile(MonsterStateMachine ms, MonsterSkillSO missileSkill) : base(ms)
    {
        skillData = missileSkill;

        var monster = ms.Monster as SmileMachine_UseMissile;
        if (monster != null)
        {
            lineRenderer = monster.lineRender;
            firePoint = monster.firepoint;
        }

        player = ms.Monster.PlayerTarget != null
            ? ms.Monster.PlayerTarget
            : GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
        stateMachine.isAttacking = true;
        missileRoutine = stateMachine.Monster.StartCoroutine(MissileRoutine());
    }

    private IEnumerator MissileRoutine()
    {
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill3));

        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
        }

        float aimDuration = 3f;
        float elapsed = 0f;

        while (elapsed < aimDuration)
        {
            if (lineRenderer != null && firePoint != null && player != null)
            {
                RotateTowardsPlayer();
                lineRenderer.SetPosition(0, firePoint.position);
                lineRenderer.SetPosition(1, player.position);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Vector3 fireDirection = Vector3.zero;
        if (player != null && firePoint != null)
            fireDirection = (player.position - firePoint.position).normalized;
        else
            fireDirection = firePoint.forward;

        // --- Fire missile ---
        if (stateMachine.Monster is SmileMachine_UseMissile monster && monster.missile != null && firePoint != null)
        {
            GameObject missile = Object.Instantiate(monster.missile, firePoint.position, Quaternion.LookRotation(fireDirection));

            if (missile.TryGetComponent<Rigidbody>(out var rb))
            {
                float missileSpeed = 10f; // tweak this for faster/slower missiles
                rb.linearVelocity = fireDirection * missileSpeed;
            }
        }

        // --- Cleanup ---
        if (lineRenderer != null)
            lineRenderer.enabled = false;

        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill3));

        // Short delay before switching back
        yield return new WaitForSeconds(0.5f);

        stateMachine.isAttacking = false;
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }

    private void RotateTowardsPlayer()
    {
        if (player == null) return;

        Vector3 dir = player.position - stateMachine.Monster.transform.position;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            float rotateSpeed = 10f;
            stateMachine.Monster.transform.rotation = Quaternion.Slerp(
                stateMachine.Monster.transform.rotation,
                targetRot,
                Time.deltaTime * rotateSpeed
            );
        }
    }

    public override void Exit()
    {
        base.Exit();

        if (missileRoutine != null)
        {
            stateMachine.Monster.StopCoroutine(missileRoutine);
            missileRoutine = null;
        }

        if (lineRenderer != null)
            lineRenderer.enabled = false;

        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill3));
        stateMachine.isAttacking = false;
    }
}