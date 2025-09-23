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

        Vector3 spawnPos = stateMachine.Monster.transform.position;

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
        aoeController.CircleInitialize(skillData.preCastTime, skillData.range, stateMachine.Monster.Stats.AttackPower);
    }

    private void OnTelegraphComplete()
    {
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill1));
    }

    public override void OnAttackHit()
    {
        if (aoeController == null) return;

        Debug.Log("OnAttackHitSlam");
        aoeController.EnableDamage();
        stateMachine.Monster.StartCoroutine(DisableColliderNextFrame());
    }

    private IEnumerator DisableColliderNextFrame()
    {
        yield return null;
        aoeController.DisableDamage();
        if (aoeInstance != null)
            Object.Destroy(aoeInstance);
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
        
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill1));
    }
}
