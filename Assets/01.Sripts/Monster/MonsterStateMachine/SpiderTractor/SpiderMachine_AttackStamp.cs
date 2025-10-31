using System.Data;
using Cysharp.Threading.Tasks.Triggers;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderMachine_AttackStamp : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private bool hasHit;
    private BoxCollider attackCollider;
    private GameObject stampEffect;
    public SpiderMachine_AttackStamp(MonsterStateMachine ms, MonsterSkillSO stampSkill) : base(ms)
    {
        skillData = stampSkill;

        if (stateMachine.Monster is SpiderTractor_UseGrenade monster && monster.stampEffect != null)
        {
            stampEffect = monster.stampEffect;
        }
        if (stateMachine.Monster is BaseMonster bm && bm.baseAttackCollider != null)
        {
            attackCollider = bm.baseAttackCollider as BoxCollider;
            if (attackCollider != null)
            {
                attackCollider.isTrigger = true;
                attackCollider.enabled = false;
            }
        }
    }

    public override void Enter()
    {
        StopMoving();
        hasHit = false;
        stateMachine.isAttacking = true;
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill1));
    }
    public override void Exit()
    {
        stateMachine.isAttacking = false;

        StopAnimation(stateMachine.Monster.animationData.GetHash(
            MonsterAnimationData.MonsterAnimationType.Skill1));

        if (attackCollider != null)
            attackCollider.enabled = false;
    }

    public override void OnAttackHit()
    {
        if (attackCollider != null && !hasHit)
        {
            AttackTrigger trigger = attackCollider.GetComponent<AttackTrigger>();
            if (trigger != null)
            {
                int attackPower = stateMachine.Monster.Stats.AttackPower;
                int finalDamage = Mathf.RoundToInt(attackPower);
                trigger.SetDamage(finalDamage);

                trigger.onHit = () =>
                {
                    hasHit = true;
                    attackCollider.enabled = false;
                };
            }

            attackCollider.enabled = true;

            SpawnStampEffect();
        }
    }

    private void SpawnStampEffect()
    {
        if (stampEffect == null)
        {
            return;
        }

        Transform monsterTransform = stateMachine.Monster.transform;
        Vector3 forward = monsterTransform.forward;
        Vector3 spawnPos = monsterTransform.position + forward * 1.5f + Vector3.up * 0.1f;

        GameObject effect = Object.Instantiate(stampEffect, spawnPos, Quaternion.LookRotation(forward));
        Object.Destroy(effect, 3f);
    }

    public override void OnAnimationComplete()
    {
        if (attackCollider != null)
            attackCollider.enabled = false;

        stateMachine.isAttacking = false;
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }

}
