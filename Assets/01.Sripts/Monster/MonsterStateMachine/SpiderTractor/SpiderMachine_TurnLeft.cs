using UnityEngine;
using System.Collections;

public class SpiderMachine_TurnLeft : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private Coroutine moveCoroutine;

    private float moveSpeed = 12f;
    private float stopDistance = 0.1f;
    private float leftOffset = 5f;

    public SpiderMachine_TurnLeft(MonsterStateMachine stateMachine, MonsterSkillSO turnLeftSkill)
        : base(stateMachine)
    {
        skillData = turnLeftSkill;
    }

    public override void Enter()
    {
        if (moveCoroutine != null)
            stateMachine.Monster.StopCoroutine(moveCoroutine);
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Run));
        moveCoroutine = stateMachine.Monster.StartCoroutine(MoveToLeftOfPlayer());
        stateMachine.isAttacking = true;
    }

    private IEnumerator MoveToLeftOfPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found for TurnLeft state.");
            stateMachine.ChangeState(stateMachine.MonsterIdleState);
            yield break;
        }

        Transform monsterTransform = stateMachine.Monster.transform;
        Vector3 playerPos = player.transform.position;

        // Calculate left side of the player (relative to player's facing direction)
        Vector3 leftSide = player.transform.position - player.transform.right * leftOffset;

        // Rotate monster to face the player while moving
        Quaternion targetRotation = Quaternion.LookRotation(playerPos - monsterTransform.position);

        while (Vector3.Distance(monsterTransform.position, leftSide) > stopDistance)
        {
            monsterTransform.position = Vector3.MoveTowards(monsterTransform.position, leftSide, moveSpeed * Time.deltaTime);
            monsterTransform.rotation = Quaternion.Lerp(monsterTransform.rotation, targetRotation, Time.deltaTime * 5f);
            yield return null;
        }

        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Run));
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
