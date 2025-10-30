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
        stateMachine.isAttacking = true;
        shootRoutine = stateMachine.Monster.StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        // --- Play shoot animation immediately ---
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill4));

        if (stateMachine.Monster is SmileMachine_UseGun machine)
        {
            machine.rifleParticleEffect.SetActive(true);
        }

        // --- Fire one shot ---
        RotateTowardsPlayer();
        DealDamageInFront();

        // Small delay so the animation/particle can play
        yield return new WaitForSeconds(0.5f);

        // --- End skill ---
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill4));

        if (stateMachine.Monster is SmileMachine_UseGun machineEnd)
        {
            machineEnd.rifleParticleEffect.SetActive(false);
        }

        stateMachine.isAttacking = false;
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
            float rotateSpeed = 10f;
            stateMachine.Monster.transform.rotation = Quaternion.Slerp(
                stateMachine.Monster.transform.rotation,
                targetRot,
                Time.deltaTime * rotateSpeed
            );
        }
    }

    private void DealDamageInFront()
    {
        Vector3 origin = stateMachine.Monster.transform.position + Vector3.up;
        Vector3 direction = stateMachine.Monster.transform.forward;
        Vector3 end = origin + direction * skillData.range;


        if (Physics.Raycast(origin, direction, out RaycastHit hit, skillData.range))
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var target))
            {
                target.OnTakeDamage((int)(stateMachine.Monster.Stats.AttackPower * skillData.effectValue));
            }
        }
        else
        {
        }
    }


    public override void Exit()
    {
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill4));

        if (shootRoutine != null)
        {
            stateMachine.Monster.StopCoroutine(shootRoutine);
            shootRoutine = null;
        }

        stateMachine.isAttacking = false;
    }
}
