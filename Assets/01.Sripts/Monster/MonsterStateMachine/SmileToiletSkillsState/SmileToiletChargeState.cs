using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SmileToiletChargeState : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private GameObject aoeInstance;
    private AreaEffectController aoeController;
    private CharacterController controller;
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!attackActive || controller == null) return;

        // Check for player collision in front of the monster
        Vector3 center = controller.transform.position + controller.center;
        float radius = controller.radius;
        float height = controller.height;

        Collider[] hits = Physics.OverlapCapsule(center + Vector3.up * (height/2 - radius),
                                                center - Vector3.up * (height/2 - radius),
                                                radius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                IDamageable dmg = hit.GetComponent<IDamageable>();
                dmg?.OnTakeDamage(stateMachine.Monster.Stats.AttackPower);
                Debug.Log("Player hit by charge!");
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
            Object.Destroy(aoeInstance);

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
