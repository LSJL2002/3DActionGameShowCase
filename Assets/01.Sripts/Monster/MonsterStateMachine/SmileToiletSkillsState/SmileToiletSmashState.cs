using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class SmileToiletSmashState : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private GameObject aoeInstance;
    private AreaEffectController aoeController;
    public SmileToiletSmashState(MonsterStateMachine ms, MonsterSkillSO smashSkill) : base(ms)
    {
        skillData = smashSkill;
    }

    public override void Enter()
    {
        StopMoving();

        if (skillData == null)
        {
            Debug.LogError("SmileToiletSmashState: skillData is null!");
            stateMachine.isAttacking = false;
            return;
        }

        stateMachine.isAttacking = true;

        Vector3 spawnPos = stateMachine.Monster.transform.position;
        Quaternion spawnRot = Quaternion.LookRotation(stateMachine.Monster.transform.forward) * Quaternion.Euler(0, 90f, 0);

        // Spawn the AOE effect
        aoeInstance = Object.Instantiate(skillData.areaEffectPrefab, spawnPos, spawnRot);
        stateMachine.Monster.RegisterAOE(aoeInstance);
        aoeController = aoeInstance.GetComponent<AreaEffectController>();

        if (aoeController == null)
        {
            Debug.LogError("SmileToiletSmashState: AOE prefab missing AreaEffectController!");
            stateMachine.isAttacking = false;
            return;
        }

        aoeController.OnTelegraphFinished += OnTelegraphComplete;
        aoeController.HalfCircleInitialize(skillData.preCastTime, skillData.range, stateMachine.Monster.Stats.AttackPower, stateMachine.Monster.transform, skillData, 180f);    }

    private void OnTelegraphComplete()
    {
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill2));
    }

    // Called from animation event
    public override void OnAttackHit()
    {
        if (aoeController == null) return;

        Debug.Log("OnAttackHitSmash");
        aoeController.EnableDamage(stateMachine.Monster.transform);
        stateMachine.Monster.StartCoroutine(DisableColliderNextFrame());
    }

    private IEnumerator DisableColliderNextFrame()
    {
        yield return null;
        aoeController.DisableDamage();
        if (aoeInstance != null)
            Object.Destroy(aoeInstance);

        // Transition back to Idle safely
        stateMachine.isAttacking = false;
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }

    public override void Exit()
    {
        if (aoeController != null)
            aoeController.OnTelegraphFinished -= OnTelegraphComplete;

        if (aoeInstance != null)
        {
            stateMachine.Monster.UnregisterAOE(aoeInstance);
            Object.Destroy(aoeInstance);
        }
            

        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill2));
    }
}
