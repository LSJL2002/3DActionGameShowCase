using Cysharp.Threading.Tasks;
using UnityEngine;

public class MonsterBaseAttack : MonsterBaseState
{
    private BoxCollider attackCollider;
    private bool hasHit;

    public MonsterBaseAttack(MonsterStateMachine stateMachine) : base(stateMachine)
    {
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
        stateMachine.isAttacking = true;
        StartAnimation(stateMachine.Monster.animationData.GetHash(
            MonsterAnimationData.MonsterAnimationType.BaseAttack));
    }

    public override void Exit()
    {
        stateMachine.isAttacking = false;

        StopAnimation(stateMachine.Monster.animationData.GetHash(
            MonsterAnimationData.MonsterAnimationType.BaseAttack));

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

        stateMachine.isAttacking = false;
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }
}
