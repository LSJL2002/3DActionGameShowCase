using UnityEngine;
using System.Collections;

public class SpiderMachine_TurnLeft : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private Coroutine moveCoroutine;
    private float moveSpeed = 12f;
    private float leftOffset = 5f;
    private bool hitWall = false;

    public SpiderMachine_TurnLeft(MonsterStateMachine stateMachine, MonsterSkillSO turnLeftSkill)
        : base(stateMachine)
    {
        skillData = turnLeftSkill;
    }

    public override void Enter()
    {
        Debug.Log("Moving monster left");

        if (moveCoroutine != null)
            stateMachine.Monster.StopCoroutine(moveCoroutine);

        hitWall = false;
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Run));
        moveCoroutine = stateMachine.Monster.StartCoroutine(MoveAroundPlayer90());
        stateMachine.isAttacking = true;
    }

    private IEnumerator MoveAroundPlayer90()
    {
        Transform monsterTransform = stateMachine.Monster.transform;
        CharacterController controller = stateMachine.Monster.Controller;
        Vector3 playerPos = stateMachine.Monster.PlayerTarget.position;

        Vector3 direction = monsterTransform.position - playerPos;
        direction.y = 0;

        Vector3 targetDirection = Quaternion.Euler(0, -90f, 0) * direction;
        Vector3 targetPos = playerPos + targetDirection.normalized * leftOffset;

        while (Vector3.Distance(monsterTransform.position, targetPos) > 0.05f)
        {
            if (hitWall)
            {
                Debug.Log("Hit wall — stopping movement early.");
                break;
            }

            Vector3 moveDir = (targetPos - monsterTransform.position).normalized;
            controller.Move(moveDir * moveSpeed * Time.deltaTime);

            monsterTransform.rotation = Quaternion.Lerp(monsterTransform.rotation,
                Quaternion.LookRotation(playerPos - monsterTransform.position),
                Time.deltaTime * 5f);

            yield return null;
        }

        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Run));
        stateMachine.isAttacking = false;
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }
    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            hitWall = true;
        }
    }

    public override void Exit()
    {
        if (moveCoroutine != null)
        {
            stateMachine.Monster.StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }
}
