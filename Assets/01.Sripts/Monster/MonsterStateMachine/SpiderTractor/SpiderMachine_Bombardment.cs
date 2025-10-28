using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderMachine_Bombardment : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private Transform firePointMissile;
    public SpiderMachine_Bombardment(MonsterStateMachine ms, MonsterSkillSO bombardmentSkill) : base(ms)
    {
        skillData = bombardmentSkill;
        var monster = ms.Monster as SpiderTractor_UseGrenade;
        if (monster != null)
        {
            firePointMissile = monster.firePointMissile;
        }
    }

    public override void Enter()
    {
        if (skillData == null)
        {
            stateMachine.isAttacking = false;
            return;
        }

        stateMachine.isAttacking = true;
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
        GameObject aoeObj = Object.Instantiate(skillData.areaEffectPrefab, stateMachine.Monster.transform.position, Quaternion.identity);
        AreaEffectController aoeController = aoeObj.GetComponent<AreaEffectController>();
        if (aoeController != null)
        {
            aoeController.MultipleCircleInitialize(
                skillData.preCastTime,
                skillData.range,
                stateMachine.Monster.Stats.AttackPower,
                skillData,
                10,
                10f,
                3f,
                null
            );

            aoeController.OnTelegraphFinished += () =>
            {
                Vector3 targetPos = aoeObj.transform.position + Vector3.up * 0.1f;
                ShootMissile(targetPos);
            };
        }
    }

    private void ShootMissile(Vector3 targetPos)
    {
        if (!(stateMachine.Monster is SpiderTractor_UseGrenade monster)) return;
        if (monster.missile == null || firePointMissile == null) return;

        GameObject missile = Object.Instantiate(monster.missile, firePointMissile.position, Quaternion.identity);

        if (missile.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            // Launch missile in an arc toward target
            Vector3 velocity = CalculateLaunchVelocity(firePointMissile.position, targetPos, 45f); // 45° launch angle
            rb.velocity = velocity;
        }
    }

    private Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 target, float launchAngle)
    {
        float gravity = Physics.gravity.y;
        float rad = launchAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(target.x, 0, target.z);
        Vector3 planarPosition = new Vector3(start.x, 0, start.z);

        float distance = Vector3.Distance(planarTarget, planarPosition);
        float yOffset = target.y - start.y;

        float initialVelocity = Mathf.Sqrt(-gravity * distance * distance / 
            (2 * (yOffset - Mathf.Tan(rad) * distance) * Mathf.Pow(Mathf.Cos(rad), 2)));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(rad), initialVelocity * Mathf.Cos(rad));
        Vector3 dir = (planarTarget - planarPosition).normalized;
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, dir);

        return rot * velocity;
    }
}
