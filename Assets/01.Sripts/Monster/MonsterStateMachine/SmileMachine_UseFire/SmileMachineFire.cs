using System.Collections;
using UnityEngine;

public class SmileMachineFire : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private Coroutine flameRoutine;
    private Transform firePoint;
    private GameObject flameObject;
    private bool isFlameActive = false;

    public SmileMachineFire(MonsterStateMachine ms, MonsterSkillSO fireSkill) : base(ms)
    {
        skillData = fireSkill;
    }

    public override void Enter()
    {
        stateMachine.isAttacking = true;
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));

        if (stateMachine.Monster is SmileMachine_UseFire monster)
        {
            firePoint = monster.firePoint.transform;
            flameObject = monster.flameThrowerEffect;
        }

        if (flameObject != null)
        {
            flameObject.SetActive(true);
            isFlameActive = true;
        }

        flameRoutine = stateMachine.Monster.StartCoroutine(FlameRoutine());
    }

    private IEnumerator FlameRoutine()
    {
        float elapsed = 0f;
        float duration = skillData.duration;

        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill4));

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            RotateTowardsPlayer();
            yield return null;
        }

        EndFlame();
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }

    private void EndFlame()
    {
        if (isFlameActive && flameObject != null)
        {
            flameObject.SetActive(false);
            isFlameActive = false;
        }

        stateMachine.isAttacking = false;
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill4));
    }

    public override void Exit()
    {
        if (flameRoutine != null)
        {
            stateMachine.Monster.StopCoroutine(flameRoutine);
            flameRoutine = null;
        }

        EndFlame();
    }

    private void RotateTowardsPlayer()
    {
        if (stateMachine.Monster.PlayerTarget == null || firePoint == null) return;

        Transform monsterTransform = stateMachine.Monster.transform;
        Vector3 targetPos = stateMachine.Monster.PlayerTarget.position;

        Vector3 flatDir = targetPos - monsterTransform.position;
        flatDir.y = 0f;

        if (flatDir.sqrMagnitude > 0.001f)
        {
            Quaternion bodyRot = Quaternion.LookRotation(flatDir);
            monsterTransform.rotation = Quaternion.Slerp(monsterTransform.rotation, bodyRot, Time.deltaTime * 5f);
        }

        Vector3 aimDir = targetPos - firePoint.position;
        if (aimDir.sqrMagnitude > 0.001f)
        {
            firePoint.rotation = Quaternion.Slerp(firePoint.rotation, Quaternion.LookRotation(aimDir), Time.deltaTime * 10f);
        }
    }
}
