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

        if (skillData == null)
        {
            Debug.LogError("SmileToieltChargeState: skill Data is null");
            stateMachine.isAttacking = false;
            return;
        }

        Vector3 spawnPos = stateMachine.Monster.transform.position;
        aoeInstance = Object.Instantiate(skillData.areaEffectPrefab, spawnPos, skillData.areaEffectPrefab.transform.rotation);
        stateMachine.Monster.RegisterAOE(aoeInstance);
        aoeController = aoeInstance.GetComponent<AreaEffectController>();

        if (aoeController == null)
        {
            Debug.LogError("AOE prefab missing");
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
        Debug.Log("Starting Charge animation, hash: ");
    }

    private void OnTelegraphComplete()
    {
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Charge));
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill3));
        attackActive = true;
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
                Debug.Log("Player hit by charge via OnControllerColliderHit!");
                attackActive = false;
            }
        }
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

        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill3));
    }
    public override void OnAttackHit()
    {
        var player = stateMachine.Monster.PlayerTarget;
        if (player == null) return;

        IDamageable damageable = player.GetComponent<IDamageable>();
        damageable?.OnTakeDamage(stateMachine.Monster.Stats.AttackPower);

        Debug.Log("Player hit by charge!");
    }
}
