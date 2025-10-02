using UnityEngine;

/// <summary>
/// Follow(추적) 상태
/// - 목표(플레이어)까지 Rigidbody.MovePosition으로 이동
/// - desiredFollowDistance 이하로 가까워지면 Idle로 전환
/// - 이동은 FixedUpdate에서 처리(물리 프레임)
/// </summary>
public class FollowState : PetState
{
    public FollowState(Companion comp) : base(comp) { }

    public override void Update(float dt)
    {
        if (!comp.player) return;

        // 전이 판정(논리 업데이트)
        // float dist = Vector3.Distance(comp.transform.position, comp.player.position);
        //if (dist <= comp.desiredFollowDistance)
        //{
        //    comp.ChangeState(new IdleState(comp));
        //}

        // 항상 Rigidbody 기준으로 거리 계산
        //float dist = Vector3.Distance(comp.rb.position, comp.player.position);

        //// 버퍼 포함: 너무 가까워지면 즉시 Idle로
        //if (dist <= comp.desiredFollowDistance - comp.stopBuffer)
        //{
        //    comp.ChangeState(new IdleState(comp));
        //}


        // ▼▼ 변경: y축을 무시하고 수평(XZ) 거리만 사용
        Vector3 off = comp.rb.position - comp.player.position; // 펫 - 플레이어 오프셋
        off.y = 0f; // CHANGED: 높이 차이는 무시
        float dist = off.magnitude; // CHANGED: 수평 거리만 계산


        // 기존 로직이 dist로 전환 조건을 판단했다면 그대로 사용
        if (dist <= comp.stopBuffer)
        {
            comp.ChangeState(new IdleState(comp));
            return;
        }
    }

    public override void FixedUpdate(float dt)
    {
        //if (!comp.player) return;

        //// 근접 시 감속: 멈출 때 튕김 방지
        //float dist = Vector3.Distance(comp.rb.position, comp.player.position);
        //float t = Mathf.InverseLerp(comp.desiredFollowDistance, comp.followStartDistance, dist); // 0~1
        //float speed = comp.moveSpeed * Mathf.Clamp01(t);

        //Vector3 fromPlayer = (comp.rb.position - comp.player.position).normalized;
        //Vector3 targetPos = comp.player.position + fromPlayer * comp.desiredFollowDistance;

        //comp.MoveTowards(comp.player.position, speed, dt);


        if (!comp.player) return;

        Vector3 playerPos = comp.player.position;
        Vector3 myPos = comp.rb.position;

        // 플레이어 뒤쪽으로 desiredFollowDistance만큼 떨어진 앵커
        // (카메라/전면을 가리지 않도록 뒤로 빠지기)
        //Vector3 back = -comp.player.forward;      // 플레이어가 바라보는 반대 방향
        //back.y = 0f; back.Normalize();
        Vector3 back = -comp.player.forward; 
        back.y = 0f; 
        back.Normalize();

        Vector3 anchor = playerPos + back * comp.desiredFollowDistance;
        //anchor.y = comp.rb.position.y;
        anchor.y = comp.LockedY;

        // (선택) 옆으로 0.3m 치우치기: 카메라 가림 더 줄이고 싶으면 사용
        // Vector3 side = Vector3.Cross(Vector3.up, comp.player.forward).normalized;
        // anchor += side * 0.3f;

        // 현재 앵커와의 거리
        float toAnchor = Vector3.Distance(myPos, anchor);

        // 앵커 근처면 확실히 정지(잔여 속도 제거)
        if (toAnchor <= comp.stopBuffer * 0.9f)
        {
            comp.rb.velocity = Vector3.zero;
            comp.rb.angularVelocity = Vector3.zero;
            return;
        }

        // 거리 기반 감속(원형 궤도에서 덜 튀게)
        float distToPlayer = Vector3.Distance(myPos, playerPos);
        float t = Mathf.InverseLerp(comp.desiredFollowDistance, comp.followStartDistance, distToPlayer);
        float speed = comp.moveSpeed * Mathf.Clamp01(t);

        // 앵커를 목표로 이동
        comp.MoveTowards(anchor, speed, dt);
    }
}
