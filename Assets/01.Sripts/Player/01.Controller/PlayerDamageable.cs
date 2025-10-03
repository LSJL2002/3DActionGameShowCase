using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.VFX;
using static CartoonFX.CFXR_Effect;
using static SkillSO;
using static UnityEngine.Rendering.DebugUI;


// 전투 대상 / 피해 처리 전용
public class PlayerDamageable : MonoBehaviour, IDamageable
{
    private PlayerManager player;


    private void Awake()
    {
        player ??= GetComponent<PlayerManager>();
    }


    // IDamageable 구현 예시 (플레이어가 맞았을 때)
    public void OnTakeDamage(int amount)
    {
        player?.Stats.TakeDamage(amount); // HP 변경은 Stats에서만
        Debug.Log(amount + "의 데미지");
        // 피격 애니메이션, 넉백 등도 여기서 처리 가능
    }

    public void ApplyEffect(MonsterEffectType Type, Vector3 sourcePos, float value = 0, float duration = 0)
    {
        switch (Type)
        {
            case MonsterEffectType.Knockback:
                var dir = (transform.position - sourcePos).normalized;
                player.stateMachine.KnockbackState.Setup(dir, value, duration);
                player.stateMachine.ChangeState(player.stateMachine.KnockbackState);
                break;

            case MonsterEffectType.Groggy:
                player.stateMachine.StunState.Setup(duration);
                player.stateMachine.ChangeState(player.stateMachine.StunState);
                break;
        }
    }
}