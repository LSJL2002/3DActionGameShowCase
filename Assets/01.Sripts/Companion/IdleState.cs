using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Idle(대기) 상태
/// - 플레이어와의 거리가 followStartDistance를 넘으면 Follow로 전환
/// - 이동/물리는 수행하지 않음
/// </summary>
public class IdleState : PetState
{
    public IdleState(Companion comp) : base(comp) { }

    public override void Enter()
    {
        // 남은 물리 속도 제거(살짝 미끄러지는 현상 방지)
        comp.rb.velocity = Vector3.zero;
        comp.rb.angularVelocity = Vector3.zero;
    }

    public override void Update(float dt)
    {
        if (!comp.player) return;

        //float dist = Vector3.Distance(comp.transform.position, comp.player.position);
        //if (dist > comp.followStartDistance)
        //{
        //    comp.ChangeState(new FollowState(comp));
        //}


        // ▼▼ 변경: y축을 무시하고 수평(XZ) 거리만 사용
        Vector3 off = comp.rb.position - comp.player.position; // 펫 - 플레이어 오프셋
        off.y = 0f; // CHANGED: 높이 차이는 무시
        float dist = off.magnitude; // CHANGED: 수평 거리만 계산


        // 기존 로직이 dist로 전환 조건을 판단했다면 그대로 사용
        if (dist > comp.followStartDistance)
        {
            comp.ChangeState(new FollowState(comp));
            return;
        }
    }

    
}
