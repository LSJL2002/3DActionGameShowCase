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

        if (stateMachine.Monster is BaseMonster bm && bm.baseAttackCollider != null)
        {
            attackCollider = bm.baseAttackCollider as BoxCollider;
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
            AttackTrigger trigger = attackCollider.GetComponent<AttackTrigger>();
            if (trigger != null)
            {
                int attackPower = stateMachine.Monster.Stats.AttackPower;
                int finalDamage = Mathf.RoundToInt(attackPower);
                trigger.SetDamage(finalDamage);

                trigger.onHit = () =>
                {
                    hasHit = true;
                    attackCollider.enabled = false;
                };

                Debug.Log($"[MonsterBaseAttack] Passed {finalDamage} damage to AttackTrigger.");
            }

            attackCollider.enabled = true;
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
