using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    private PlayerManager player;
    private EventManager eventManager;
    private ForceReceiver forceReceiver;
    private Transform playerTransform;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        eventManager = player.eventManager;
        forceReceiver = player.ForceReceiver;
        playerTransform = transform;

        eventManager?.Initialize(playerTransform, forceReceiver);
    }

    // 일반 호출
    public void MoveBehindTarget(Transform target)
    {
        eventManager?.MoveBehindTarget(target);
    }

    // 애니메이션 이벤트용 (매개변수 없이 호출 가능)
    public void MoveBehindTargetFromAnimationEvent()
    {
        var attackController = player.Attack; // PlayerAttackController
        if (attackController != null && attackController.CurrentAttackTarget != null)
        {
            eventManager?.MoveBehindTarget(attackController.CurrentAttackTarget);
        }
    }
}