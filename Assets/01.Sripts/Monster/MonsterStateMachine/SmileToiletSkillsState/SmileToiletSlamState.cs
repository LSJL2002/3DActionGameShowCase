using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SmileToiletSlamState : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private GameObject aoeInstance;
    private AreaEffectController aoeController;

    public SmileToiletSlamState(MonsterStateMachine ms, MonsterSkillSO slamSkill) : base(ms)
    {
        skillData = slamSkill;
    }

    public override void Enter()
    {
        StopMoving();
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
        if (skillData == null)
        {
            Debug.LogError("SmileToiletSlamState: skillData is null!");
            stateMachine.isAttacking = false;
            return;
        }

        stateMachine.isAttacking = true;
        StopMoving();

        Vector3 spawnPos = stateMachine.Monster.AreaEffectPoint.transform.position;
        Debug.Log(spawnPos);

        aoeInstance = Object.Instantiate(skillData.areaEffectPrefab, spawnPos, Quaternion.identity);
        stateMachine.Monster.RegisterAOE(aoeInstance);

        aoeController = aoeInstance.GetComponent<AreaEffectController>();
        if (aoeController == null)
        {
            Debug.LogError("SmileToiletSlamState: AOE prefab is missing AreaEffectController!");
            stateMachine.isAttacking = false;
            return;
        }
        aoeController.OnTelegraphFinished += OnTelegraphComplete;
        aoeController.CircleInitialize(
            skillData.preCastTime * stateMachine.PreCastTimeMultiplier,
            skillData.range * stateMachine.RangeMultiplier,
            (int)(stateMachine.Monster.Stats.AttackPower * stateMachine.EffectValueMultiplier),
            skillData
        );
    }

    private void OnTelegraphComplete()
    {
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill2));
    }

    public override void OnAttackHit()
    {
        if (aoeController == null) return;

        Debug.Log("OnAttackHitSlam");
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
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill2));
        Debug.Log("Finished Animation");
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
        stateMachine.isAttacking = false;
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill2));
    }
}
