using UnityEngine;

public class MonsterBaseAttack : MonsterBaseState
{
    private int damage;
    private BoxCollider attackCollider;
    private bool hasHit;

    public MonsterBaseAttack(MonsterStateMachine stateMachine) : base(stateMachine)
    {
        damage = stateMachine.Monster.Stats.AttackPower;

        // Get reference to the collider from ToiletMonster
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
        stateMachine.isAttacking = true;
        // Start attack animation
        StartAnimation(stateMachine.Monster.animationData.GetHash(
            MonsterAnimationData.MonsterAnimationType.BaseAttack));
    }

    public override void Exit()
    {
        stateMachine.isAttacking = false;

        // Stop animation and disable collider
        StopAnimation(stateMachine.Monster.animationData.GetHash(
            MonsterAnimationData.MonsterAnimationType.BaseAttack));

        if (attackCollider != null)
            attackCollider.enabled = false;
    }

    // Call this from an animation event
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
                    Debug.Log("Attack Hit Once");
                };
            }
            
            attackCollider.enabled = true;
            Debug.Log("Attack collider enabled!");
        }
    }


    public override void OnAnimationComplete()
    {
        if (attackCollider != null)
            attackCollider.enabled = false;

        Debug.Log($"{stateMachine.Monster.name} finished base attack.");
        stateMachine.isAttacking = false;
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }
}
