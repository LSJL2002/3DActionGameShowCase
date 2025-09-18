using System.Collections;
using UnityEngine;

public class MonsterBaseAttackAlt : MonsterBaseState
{
    private float attackRange;
    private int damage;
    private Vector3 attackCenter;
    private float sphereRadius;
    private MonsterAnimationData.MonsterAnimationType animationType;

    public MonsterBaseAttackAlt(MonsterStateMachine stateMachine, MonsterAnimationData.MonsterAnimationType animType) : base(stateMachine)
    {
        attackRange = stateMachine.Monster.Stats.AttackRange;
        damage = stateMachine.Monster.Stats.AttackPower;
        sphereRadius = attackRange / 2f;

        animationType = animType; // store custom animation type
    }

    public override void Enter()
    {
        StopMoving();
        StartAnimation(stateMachine.Monster.animationData.GetHash(animationType));
        stateMachine.Monster.StartCoroutine(DoBaseAttack());
    }

    private IEnumerator DoBaseAttack()
    {
        yield return new WaitForSeconds(0.3f);
        attackCenter = stateMachine.Monster.transform.position + stateMachine.Monster.transform.forward * (attackRange / 2f);

        Collider[] hitColliders = Physics.OverlapSphere(attackCenter, sphereRadius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Player") && hit.TryGetComponent<IDamageable>(out var dmg))
            {
                dmg.OnTakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(1.2f);

        stateMachine.isAttacking = false;

        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }

    public override void Exit()
    {
        stateMachine.isAttacking = false;
        StopAnimation(stateMachine.Monster.animationData.GetHash(animationType));
    }
}
