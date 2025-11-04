using System.Collections;
using UnityEngine;

public class SpiderMachine_TurnRight : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private Coroutine moveCoroutine;
    private float moveSpeed = 12f;
    private float rightOffset = 5f;

    public SpiderMachine_TurnRight(MonsterStateMachine stateMachine, MonsterSkillSO turnRightSkill) : base(stateMachine)
    {
        skillData = turnRightSkill;
    }

    public override void Enter()
    {
        Debug.Log("Moving monster right");
        if (moveCoroutine != null)
            stateMachine.Monster.StopCoroutine(moveCoroutine);
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Run));
        moveCoroutine = stateMachine.Monster.StartCoroutine(MoveAroundPlayer90());
        stateMachine.isAttacking = true;
    }

    private IEnumerator MoveAroundPlayer90()
    {
        Debug.Log("Moving monster right");
        Transform monsterTransform = stateMachine.Monster.transform;
        Vector3 playerPos = stateMachine.Monster.PlayerTarget.position;

        Vector3 direction = monsterTransform.position - playerPos;
        direction.y = 0;

        Vector3 targetDirection = Quaternion.Euler(0, 90f, 0) * direction;

        Vector3 targetPos = playerPos + targetDirection.normalized * rightOffset;

        while (Vector3.Distance(monsterTransform.position, targetPos) > 0.05f)
        {
            monsterTransform.position = Vector3.MoveTowards(monsterTransform.position, targetPos, moveSpeed * Time.deltaTime);
            monsterTransform.rotation = Quaternion.Lerp(monsterTransform.rotation, Quaternion.LookRotation(playerPos - monsterTransform.position), Time.deltaTime * 5f);
            yield return null;
        }

        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Run));
        stateMachine.isAttacking = false;
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }

    public override void Exit()
    {
        if (moveCoroutine != null)
        {
            stateMachine.Monster.StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        stateMachine.isAttacking = false;
    }
}
