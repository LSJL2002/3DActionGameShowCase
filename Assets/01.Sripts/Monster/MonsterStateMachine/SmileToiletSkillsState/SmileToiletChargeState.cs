using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SmileToiletChargeState : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private GameObject aoeInstance;
    private AreaEffectController aoeController;
    private bool attackActive;

    public SmileToiletChargeState(MonsterStateMachine ms, MonsterSkillSO chargeSkill) : base(ms)
    {
        skillData = chargeSkill;
    }

    public override void Enter()
    {
        StopMoving();
        stateMachine.isAttacking = true;

        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Charge));

        Vector3 spawnPos = stateMachine.Monster.AreaEffectPoint.transform.position;
        aoeInstance = Object.Instantiate(skillData.areaEffectPrefab, spawnPos, skillData.areaEffectPrefab.transform.rotation);
        stateMachine.Monster.RegisterAOE(aoeInstance);
        aoeController = aoeInstance.GetComponent<AreaEffectController>();

        if (aoeController == null)
        {
            stateMachine.isAttacking = false;
            return;
        }

        aoeController.OnTelegraphStarted += OnTelegraphStart;
        aoeController.OnTelegraphFinished += OnTelegraphComplete;

        aoeController.DashInitialize(skillData.preCastTime, skillData.range, stateMachine.Monster.Stats.AttackPower, stateMachine.Monster.transform);
    }

    private void OnTelegraphStart()
    {
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Charge));
    }

    private void OnTelegraphComplete()
    {
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Charge));
        attackActive = true;

        var mc = stateMachine.Monster.GetComponent<MonsterController>();
        if (mc != null)
        {
            mc.SetRootMotionMultiplier(2.0f); // boost root motion for dash
        }

        // You can optionally play skill impact animation here
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill3));
    }


    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!attackActive) return;

        if (hit.collider.CompareTag("Player"))
        {
            IDamageable dmg = hit.collider.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.OnTakeDamage(stateMachine.Monster.Stats.AttackPower);
                Vector3 sourcePos = stateMachine.Monster.transform.position;
                dmg.ApplyEffect(skillData.monsterEffectType, sourcePos, skillData.knockbackDistance, skillData.duration);
                attackActive = false;
            }
        }
    }

    public override void OnAnimationComplete()
    {
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill3));
        stateMachine.isAttacking = false;
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }


    public override void Exit()
    {
        if (aoeController != null)
        {
            aoeController.OnTelegraphStarted -= OnTelegraphStart;
            aoeController.OnTelegraphFinished -= OnTelegraphComplete;
        }

        if (aoeInstance != null)
        {
            stateMachine.Monster.UnregisterAOE(aoeInstance);
            Object.Destroy(aoeInstance);
        }
        var mc = stateMachine.Monster.GetComponent<MonsterController>();
        if (mc != null)
        {
            mc.ResetRootMotionMultiplier(); // reset to normal
        }
    }
    
    public override void OnAttackHit()
    {
        var player = stateMachine.Monster.PlayerTarget;
        if (player == null) return;

        IDamageable damageable = player.GetComponent<IDamageable>();
        damageable?.OnTakeDamage(stateMachine.Monster.Stats.AttackPower);
    }
}
