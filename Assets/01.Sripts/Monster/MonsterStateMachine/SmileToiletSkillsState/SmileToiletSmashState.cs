using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class SmileToiletSmashState : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private GameObject aoeInstance;
    private AreaEffectController aoeController;
    private bool hasHit = false;
    public SmileToiletSmashState(MonsterStateMachine ms, MonsterSkillSO smashSkill) : base(ms)
    {
        skillData = smashSkill;
    }

    public override void Enter()
    {
        StopMoving();
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));

        if (skillData == null)
        {
            Debug.LogError("SmileToiletSmashState: skillData is null!");
            stateMachine.isAttacking = false;
            return;
        }

        stateMachine.isAttacking = true;

        Vector3 spawnPos = stateMachine.Monster.AreaEffectPoint.transform.position;
        if (Physics.Raycast(spawnPos + Vector3.up, Vector3.down, out RaycastHit hit, 10f, LayerMask.GetMask("Ground")))
        {
            spawnPos = hit.point + Vector3.up * 0.1f; // lift 0.1 above ground
        }
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
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill1));
    }

    // Called from animation event
    public override void OnAttackHit()
    {
        if (hasHit || aoeController == null) return;
        hasHit = true;
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
    }

    public override void OnAnimationComplete()
    {
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill1));
        Debug.Log("Finished Animation");
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }

    public override void Exit()
    {
        Debug.Log("Exiting Smash State");
        if (aoeController != null)
            aoeController.OnTelegraphFinished -= OnTelegraphComplete;

        if (aoeInstance != null)
        {
            stateMachine.Monster.UnregisterAOE(aoeInstance);
            Object.Destroy(aoeInstance);
        }
        stateMachine.isAttacking = false;
        hasHit = false;
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill2));
    }
}
