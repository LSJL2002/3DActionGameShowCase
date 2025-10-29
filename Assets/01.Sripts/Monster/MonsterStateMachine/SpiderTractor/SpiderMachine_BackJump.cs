using System.Collections;
using UnityEngine;

public class SpiderMachine_BackJump : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private float jumpDistance = 10f; 
    private float jumpHeight = 5f;
    private float jumpDuration = 1f;

    public SpiderMachine_BackJump(MonsterStateMachine ms, MonsterSkillSO backJumpSkill) : base(ms)
    {
        skillData = backJumpSkill;
    }

    public override void Enter()
    {
        if (stateMachine.Monster.PlayerTarget == null)
        {
            stateMachine.ChangeState(stateMachine.MonsterIdleState);
            return;
        }

        stateMachine.isAttacking = true;

        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill5));
        stateMachine.Monster.StartCoroutine(JumpAway());
    }

    private IEnumerator JumpAway()
    {
        Transform monsterTransform = stateMachine.Monster.transform;
        Vector3 startPos = monsterTransform.position;

        // Calculate jump target: away from player
        Vector3 directionAway = (monsterTransform.position - stateMachine.Monster.PlayerTarget.position).normalized;
        Vector3 targetPos = startPos + directionAway * jumpDistance;

        float elapsed = 0f;

        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / jumpDuration);

            // Horizontal movement
            Vector3 horizontal = Vector3.Lerp(startPos, targetPos, t);

            // Vertical arc
            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            monsterTransform.position = horizontal + Vector3.up * height;

            // Face away from player during jump
            monsterTransform.rotation = Quaternion.Lerp(monsterTransform.rotation,
                Quaternion.LookRotation(directionAway),
                Time.deltaTime * 5f);

            yield return null;
        }

        monsterTransform.position = targetPos;

        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill5));
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
        stateMachine.isAttacking = false;
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill5));
        stateMachine.isAttacking = false;
    }
}
