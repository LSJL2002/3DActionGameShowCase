using System.Collections;
using UnityEngine;

public class SmileMachineFire : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private Coroutine flameRoutine;
    private Transform firePoint;
    private GameObject flameEffect;
    private ParticleSystem flameParticles;

    private float damageInterval = 0.2f;
    private float lastDamageTime;

    public SmileMachineFire(MonsterStateMachine ms, MonsterSkillSO fireSkill) : base(ms)
    {
        skillData = fireSkill;
    }

    public override void Enter()
    {
        stateMachine.isAttacking = true;

        if (stateMachine.Monster is SmileMachine_UseFire monster)
        {
            firePoint = monster.firePoint.transform;
            flameEffect = monster.flameThrowerEffect;
        }

        if (flameEffect != null)
        {
            flameEffect.SetActive(true);
            flameParticles = flameEffect.GetComponent<ParticleSystem>();

            var collision = flameParticles.collision;
            collision.enabled = true;
            collision.type = ParticleSystemCollisionType.World;
            collision.mode = ParticleSystemCollisionMode.Collision3D;
            collision.sendCollisionMessages = true;

            flameParticles.Play();
        }

        flameRoutine = stateMachine.Monster.StartCoroutine(FlameRoutine());
    }

    private IEnumerator FlameRoutine()
    {
        float duration = skillData.duration;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            RotateTowardsPlayer();
            yield return null;
        }

        ExitFlame();
    }

    private void ExitFlame()
    {
        if (flameEffect != null)
        {
            flameParticles.Stop();
            flameEffect.SetActive(false);
        }

        stateMachine.isAttacking = false;
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }

    private void RotateTowardsPlayer()
    {
        if (stateMachine.Monster.PlayerTarget == null || firePoint == null)
            return;

        Transform monsterTransform = stateMachine.Monster.transform;
        Vector3 targetPos = stateMachine.Monster.PlayerTarget.position;

        Vector3 flatDir = targetPos - monsterTransform.position;
        flatDir.y = 0f;

        if (flatDir.sqrMagnitude > 0.001f)
        {
            Quaternion bodyRot = Quaternion.LookRotation(flatDir);
            float rotateSpeed = 5f;
            monsterTransform.rotation = Quaternion.Slerp(
                monsterTransform.rotation,
                bodyRot,
                Time.deltaTime * rotateSpeed
            );
        }

        Vector3 aimDir = targetPos - firePoint.position;
        if (aimDir.sqrMagnitude > 0.001f)
        {
            Quaternion aimRot = Quaternion.LookRotation(aimDir);
            firePoint.rotation = Quaternion.Slerp(
                firePoint.rotation,
                aimRot,
                Time.deltaTime * 10f
            );
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit flamethrower");

        if (flameRoutine != null)
        {
            stateMachine.Monster.StopCoroutine(flameRoutine);
            flameRoutine = null;
        }

        ExitFlame();
    }
}
