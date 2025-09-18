using System.Collections;
using UnityEngine;

public class MonsterBaseAttack : MonsterBaseState
{
    private float attackRange;
    private int damage;
    private Vector3 attackCenter;
    private float sphereRadius;

    public MonsterBaseAttack(MonsterStateMachine stateMachine) : base(stateMachine)
    {
        attackRange = stateMachine.Monster.Stats.AttackRange;
        damage = stateMachine.Monster.Stats.AttackPower;
        sphereRadius = attackRange / 2f;
    }

    public override void Enter()
    {
        StopMoving();
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.BaseAttack));
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
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.BaseAttack));
    }
    private IEnumerator DrawAttackSphere()
    {
        float duration = 0.5f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            int segments = 30;
            for (int i = 0; i < segments; i++)
            {
                float angle = i * Mathf.PI * 2f / segments;
                float nextAngle = (i + 1) * Mathf.PI * 2f / segments;

                Vector3 pointA = attackCenter + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * sphereRadius;
                Vector3 pointB = attackCenter + new Vector3(Mathf.Cos(nextAngle), 0, Mathf.Sin(nextAngle)) * sphereRadius;

                Debug.DrawLine(pointA, pointB, Color.red, 0.1f);
            }

            yield return null;
        }
    }

}
