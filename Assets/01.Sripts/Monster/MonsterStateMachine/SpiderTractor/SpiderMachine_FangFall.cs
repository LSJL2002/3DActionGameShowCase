using System.Collections;
using UnityEngine;

public class SpiderMachine_FangFall : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private GameObject aoeInstance;
    private AreaEffectController aoeController;
    private GameObject fangFallEffect;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float jumpHeight = 5f;
    private float jumpDuration = 1f;

    public SpiderMachine_FangFall(MonsterStateMachine stateMachine, MonsterSkillSO fangFallSkill) : base(stateMachine)
    {
        skillData = fangFallSkill;
        if (stateMachine.Monster is SpiderTractor_UseGrenade monster && monster.fangFallEffect != null)
        {
            fangFallEffect = monster.fangFallEffect;
        }
    }

    public override void Enter()
    {
        stateMachine.isAttacking = true;
        if (skillData == null)
        {
            stateMachine.isAttacking = false;
            return;
        }

        startPos = stateMachine.Monster.transform.position;
        targetPos = stateMachine.Monster.PlayerTarget.position;

        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));

        if (Physics.Raycast(targetPos + Vector3.up, Vector3.down, out RaycastHit hit, 10f, LayerMask.GetMask("Ground")))
            targetPos = hit.point;

        aoeInstance = Object.Instantiate(skillData.areaEffectPrefab, targetPos, Quaternion.identity);
        stateMachine.Monster.RegisterAOE(aoeInstance);

        aoeController = aoeInstance.GetComponent<AreaEffectController>();
        if (aoeController == null)
        {
            Debug.LogError("AoeError");
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

    public void TriggerJump()
    {
        stateMachine.Monster.StartCoroutine(JumpToTarget());
    }

    private IEnumerator JumpToTarget()
    {
        Vector3 start = stateMachine.Monster.transform.position;
        Vector3 end = targetPos;

        float elapsed = 0f;

        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / jumpDuration);

            Vector3 horizontal = Vector3.Lerp(start, end, t);

            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            stateMachine.Monster.transform.position = horizontal + Vector3.up * height;

            yield return null;
        }

        stateMachine.Monster.transform.position = end;
        if (fangFallEffect != null)
        {
            Object.Instantiate(fangFallEffect, end, fangFallEffect.transform.rotation);
        }
        OnAttackHit();
        yield return new WaitForSeconds(1f);
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }

    public override void OnAttackHit()
    {
        if (aoeController == null) return;

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

    public override void Exit()
    {
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill2));
        if (aoeController != null)
            aoeController.OnTelegraphFinished -= OnTelegraphComplete;

        if (aoeInstance != null)
        {
            stateMachine.Monster.UnregisterAOE(aoeInstance);
            Object.Destroy(aoeInstance);
        }

        stateMachine.isAttacking = false;
    }
}
