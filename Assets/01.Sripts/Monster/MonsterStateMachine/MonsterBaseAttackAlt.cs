using UnityEngine;

public class MonsterBaseAttackAlt : MonsterBaseState
{
    private int damage;
    private BoxCollider attackCollider;
    private MonsterAnimationData.MonsterAnimationType animationType;
    private bool hasHit;

    public MonsterBaseAttackAlt(MonsterStateMachine stateMachine, MonsterAnimationData.MonsterAnimationType animType) : base(stateMachine)
    {
        damage = stateMachine.Monster.Stats.AttackPower;
        animationType = animType;

        if (stateMachine.Monster is ToiletMonster toilet && toilet.baseAttackCollider != null)
        {
            attackCollider = toilet.baseAttackCollider as BoxCollider;
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
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));

        stateMachine.isAttacking = true;

        StartAnimation(stateMachine.Monster.animationData.GetHash(animationType));
    }

    public override void Exit()
    {
        stateMachine.isAttacking = false;

        StopAnimation(stateMachine.Monster.animationData.GetHash(animationType));

        if (attackCollider != null)
            attackCollider.enabled = false;
    }

    public override void OnAttackHit()
    {
        if (attackCollider != null && !hasHit)
        {
            // Set the damage
            AttackTrigger trigger = attackCollider.GetComponent<AttackTrigger>();
            if (trigger != null)
            {
                trigger.damage = damage;
                trigger.onHit = () =>
                {
                    hasHit = true;
                    attackCollider.enabled = false;
                };
            }

            attackCollider.enabled = true;
            Debug.Log("Alt attack collider enabled!");
        }
    }

    public override void OnAnimationComplete()
    {
        if (attackCollider != null)
            attackCollider.enabled = false;

        Debug.Log($"{stateMachine.Monster.name} finished alt base attack.");
        stateMachine.isAttacking = false;
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }
}
