using System.Collections;
using System.Linq;
using UnityEngine;

public class MonsterSkillOneState : MonsterBaseState
{
    private GameObject borderQuad;
    private GameObject fillQuad;
    private float fillDuration = 1.5f;
    private Vector3 targetScale = new Vector3(3f, 3f, 1f);
    private Collider[] hits;

    public MonsterSkillOneState(MonsterStateMachine ms) : base(ms)
    {
        TestMonster monster = ms.Monster.GetComponent<TestMonster>();
        borderQuad = monster.circleBorderPrefab;
        fillQuad = monster.circleFillPrefab;

        borderQuad.SetActive(false);
        fillQuad.SetActive(false);
    }

    public override void Enter()
    {
        StopMoving();

        // Enable quads
        borderQuad.SetActive(true);
        fillQuad.SetActive(true);

        fillQuad.transform.localScale = Vector3.zero;

        // Start the fill and attack coroutine
        stateMachine.Monster.StartCoroutine(FillCircleThenAttack());
    }

    private IEnumerator FillCircleThenAttack()
    {
        float timer = 0f; // Change to skill cast time later
        while (timer < fillDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fillDuration);
            fillQuad.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
            yield return null;
        }

        // Ensure final scale
        fillQuad.transform.localScale = targetScale;

        // Play attack animation
        //PlayTriggerAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Attack));

        yield return new WaitForSeconds(0.5f);

        ApplyDamage();
    }

    private void ApplyDamage()
    {
        Vector3 attackCenter = stateMachine.Monster.transform.position;
        hits = Physics.OverlapSphere(attackCenter, 3);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                IDamageable damageable = hit.GetComponent<IDamageable>();
                damageable?.OnTakeDamage(stateMachine.Monster.Stats.AttackPower);
            }
        }

        // Turn off the quads after attack
        fillQuad.SetActive(false);
        borderQuad.SetActive(false);
    }

    public override void Exit()
    {
        fillQuad.SetActive(false);
        borderQuad.SetActive(false);
    }


    public void OnAttackAnimationComplete()
    {
        fillQuad.SetActive(false);
        borderQuad.SetActive(false);

        stateMachine.isAttacking = false;
    }
}
