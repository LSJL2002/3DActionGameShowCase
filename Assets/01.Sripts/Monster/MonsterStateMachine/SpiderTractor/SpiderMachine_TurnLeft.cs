using UnityEngine;
using System.Collections;

public class SpiderMachine_TurnLeft : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private float moveSpeed = 12f;
    private float rotationSpeed = 12f;
    private Vector3 targetPosition;
    private bool hasArrived;
    private float stopDistance = 1f;
    private float desiredDistanceFromPlayer = 5f;

    private Coroutine moveCoroutine;

    public SpiderMachine_TurnLeft(MonsterStateMachine stateMachine, MonsterSkillSO turnLeftSkill)
        : base(stateMachine)
    {
        skillData = turnLeftSkill;
    }

    public override void Enter()
    {
        base.Enter();
        hasArrived = false;

        Transform monster = stateMachine.Monster.transform;
        Transform player = stateMachine.Monster.PlayerTarget;

        if (player == null)
        {
            Debug.LogWarning("[TurnLeft] No player target found!");
            stateMachine.ChangeState(stateMachine.MonsterIdleState);
            return;
        }
        Vector3 toPlayer = (player.position - monster.position).normalized;
        Vector3 leftDirection = Quaternion.Euler(0, -90f, 0) * toPlayer;
        targetPosition = player.position + leftDirection * desiredDistanceFromPlayer;

        Debug.Log($"[TurnLeft] Monster: {monster.position}, Player: {player.position}, Target: {targetPosition}");

        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Run));

        moveCoroutine = stateMachine.Monster.StartCoroutine(MoveToTarget(monster, targetPosition));
    }

    private IEnumerator MoveToTarget(Transform monster, Vector3 target)
    {
        while (!hasArrived)
        {
            Vector3 direction = (target - monster.position).normalized;
            float distance = Vector3.Distance(monster.position, target);

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                monster.rotation = Quaternion.Slerp(monster.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }

            monster.position += direction * moveSpeed * Time.deltaTime;

            if (distance <= stopDistance)
            {
                hasArrived = true;
                Debug.Log("[TurnLeft] Arrived at target position.");

                StopAnimation(stateMachine.Monster.animationData.GetHash(
                    MonsterAnimationData.MonsterAnimationType.Run));

                stateMachine.ChangeState(stateMachine.MonsterIdleState);
            }

            yield return null;
        }
    }

    public override void Exit()
    {
        base.Exit();

        if (moveCoroutine != null)
        {
            stateMachine.Monster.StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        Debug.Log("[TurnLeft] Exiting state.");
    }
}
