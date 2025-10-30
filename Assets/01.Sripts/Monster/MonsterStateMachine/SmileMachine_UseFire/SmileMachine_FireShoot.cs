using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileMachine_FireShoot : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private Transform firePoint;
    private GameObject fireballPrefab;
    private GameObject groundFirePrefab;

    private int circlesFinished = 0;
    private int totalCircles = 0;

    public SmileMachine_FireShoot(MonsterStateMachine ms, MonsterSkillSO fireShootSkill) : base(ms)
    {
        skillData = fireShootSkill;
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));

        if (stateMachine.Monster is SmileMachine_UseFire monster)
        {
            firePoint = monster.firePoint.transform;
            fireballPrefab = monster.fireball;
            groundFirePrefab = monster.groundFire;
        }

        if (skillData == null || firePoint == null || fireballPrefab == null)
        {
            stateMachine.isAttacking = false;
            return;
        }

        stateMachine.isAttacking = true;

        // Number of circles to spawn
        totalCircles = 5;
        circlesFinished = 0;

        AreaEffectController aoeController = stateMachine.Monster.AreaEffectPoint.GetComponent<AreaEffectController>();
        if (aoeController == null)
            aoeController = stateMachine.Monster.AreaEffectPoint.gameObject.AddComponent<AreaEffectController>();

        aoeController.MultipleCircleInitialize(
            skillData.preCastTime * stateMachine.PreCastTimeMultiplier,
            skillData.range * stateMachine.RangeMultiplier,
            (int)(stateMachine.Monster.Stats.AttackPower * stateMachine.EffectValueMultiplier),
            skillData,
            totalCircles,   // number of circles
            4f,             // offset range
            0.4f,           // delay between circles
            stateMachine.Monster, // pass monster for AOEs
            FireAtPosition           // callback when telegraph finishes
        );
    }

    private void FireAtPosition(Vector3 targetPos)
    {
        if (fireballPrefab == null || firePoint == null) return;

        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill4));

        GameObject fireball = Object.Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        Vector3 dir = (targetPos - firePoint.position).normalized;
        if (dir != Vector3.zero)
            fireball.transform.rotation = Quaternion.LookRotation(-dir);

        ParticleSystem ps = fireball.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
            ps.Play();

        stateMachine.Monster.StartCoroutine(MoveFireballToTarget(fireball, targetPos, 0.7f));

        circlesFinished++;
    }

    private IEnumerator MoveFireballToTarget(GameObject fireball, Vector3 targetPos, float duration)
    {
        Vector3 start = fireball.transform.position;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            fireball.transform.position = Vector3.Lerp(start, targetPos, timer / duration);
            yield return null;
        }

        fireball.transform.position = targetPos;

        Object.Destroy(fireball);

        if (groundFirePrefab != null)
        {
            Object.Instantiate(groundFirePrefab, targetPos, groundFirePrefab.transform.rotation);
        }

        if (circlesFinished >= totalCircles)
        {
            stateMachine.ChangeState(stateMachine.MonsterIdleState);
        }
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill4));
        stateMachine.isAttacking = false;

        if (stateMachine.Monster != null)
            stateMachine.Monster.ClearAOEs(); // Clear all remaining AOEs
    }
}
