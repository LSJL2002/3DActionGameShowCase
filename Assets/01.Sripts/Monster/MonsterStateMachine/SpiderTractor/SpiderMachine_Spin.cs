using UnityEngine;

public class SpiderMachine_Spin : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private GameObject aoeInstance;
    private AreaEffectController aoeController;
    public SpiderMachine_Spin(MonsterStateMachine stateMachine, MonsterSkillSO spinSkill) : base(stateMachine)
    {
        skillData = spinSkill;
    }

    public override void Enter()
    {
        StopMoving();
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
        if (skillData == null)
        {
            stateMachine.isAttacking = false;
            return;
        }
        stateMachine.isAttacking = true;

        Vector3 spawnPos = stateMachine.Monster.AreaEffectPoint.transform.position;
        aoeInstance = Object.Instantiate(skillData.areaEffectPrefab, spawnPos, Quaternion.identity);
        stateMachine.Monster.RegisterAOE(aoeInstance);

        aoeController = aoeInstance.GetComponent<AreaEffectController>();
        if (aoeController == null)
        {
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
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill3));
    }

    public void TriggerSpin()
    {
        
    }

    public override void OnAttackHit()
    {
        if (aoeController == null) return;

        aoeController.EnableDamage(stateMachine.Monster.transform);
    }

    public override void Exit()
    {
        if (aoeController != null)
        {
            aoeController.OnTelegraphFinished -= OnTelegraphComplete;
        }

        if (aoeInstance != null)
        {
            stateMachine.Monster.UnregisterAOE(aoeInstance);
            Object.Destroy(aoeInstance);
        }
        stateMachine.isAttacking = false;
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill3));
    }
}
