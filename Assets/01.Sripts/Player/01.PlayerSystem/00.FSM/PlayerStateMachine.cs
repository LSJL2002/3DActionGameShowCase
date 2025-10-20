using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PlayerStateMachine : StateMachine
{
    public PlayerCharacter Player { get; }

    public BattleModule CurrentBattleModule { get; private set; }

    public void SetBattleModule(BattleModule module) => CurrentBattleModule = module;


    // 원본 데이터
    public readonly PlayerGroundData GroundData;
    public readonly PlayerAirData AirData;
    // 현재 공격 데이터 (SO에서 가져온 참조본)
    public AttackInfoData AttackInfo { get; private set; }

    // 이동 관련
    public Vector2 MovementInput { get; set; } // 입력 방향 (WASD, 스틱)
    public float RotationDamping { get; set; } // 회전할 때 부드럽게 보정하는 값
    public float MovementSpeed { get; private set; } // 현재 이동 속도
    public float LastWalkBlend { get; set; } = 0f;     // 걷기 Blend 유지용
    public float LastWalkTimer { get; set; } = 0f;

    private float _movementSpeedModifier;
    public float MovementSpeedModifier // 속도 보정 계수
    { 
        get => _movementSpeedModifier; 
        set 
        {
            _movementSpeedModifier = value;
            Player.Animator.SetFloat(
            Player.AnimationData.MoveSpeedHash,
            MovementSpeedModifier);
        }
    }

    public float JumpForce { get; set; } //점프력
    public int ComboIndex { get; set; } //콤보인덱스



    // ================== 상태 오브젝트=====================
    //Ground 로직
    public PlayerIdleState IdleState { get;}
    public PlayerWalkState WalkState { get;}
    public PlayerRunState RunState { get;}
    //Air 로직
    public PlayerJumpState JumpState { get;}
    public PlayerFallState FallState { get;}
    //Attack 로직
    public PlayerAttackState AttackState { get;}
    public PlayerComboAttackState ComboAttackState { get; set; }
    public PlyerFinishAttackState FinishAttackState { get; set; }
    //Skill 로직
    public PlayerSkillState SkillState { get; set; }
    // 독립적인 Sub-State Dodge 로직
    public PlayerDodgeState DodgeState { get; }
    public PlayerKnockbackState KnockbackState { get;}
    public PlayerStunState StunState { get;}
    public PlayerDeathState DeathState { get;}
    // Swap 관리
    public PlayerSwapOutState SwapOutState { get; }
    public PlayerSwapInState SwapInState { get; }



    public PlayerStateMachine(PlayerCharacter player)
    {
        Player = player;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        DodgeState = new PlayerDodgeState(this);
        JumpState = new PlayerJumpState(this);
        FallState = new PlayerFallState(this);
        AttackState = new PlayerAttackState(this);
        ComboAttackState = new PlayerComboAttackState(this);
        FinishAttackState = new PlyerFinishAttackState(this);
        SkillState = new PlayerSkillState(this);
        KnockbackState = new PlayerKnockbackState(this);
        StunState = new PlayerStunState(this);
        DeathState = new PlayerDeathState(this);
        SwapOutState = new PlayerSwapOutState(this);
        SwapInState = new PlayerSwapInState(this);

        MovementSpeed = player.InfoData.GroundData.BaseSpeed;
        RotationDamping = player.InfoData.GroundData.BaseRotationDamping;
        GroundData = player.InfoData.GroundData;

        // AttackInfo 초기화
        ComboIndex = 0;
        SetAttackInfo(ComboIndex);

        // 캐릭터 타입별 BattleModule 연결
        switch (player.CharacterType)
        {
            case CharacterType.Yuki: SetBattleModule(new BattleModule_Yuki(this)); break;
            case CharacterType.Aoi: SetBattleModule(new BattleModule_Aoi(this)); break;
            case CharacterType.Mika: SetBattleModule(new BattleModule_Mika(this)); break;
        }
    }

    public void SetAttackInfo(int comboIndex)
    {
        ComboIndex = comboIndex;
        AttackInfo = Player.InfoData.AttackData.GetAttackInfoData(comboIndex);
    }

    // ===================== 타겟 탐지 (공통) =====================
    /// <summary>
    /// 반경 내 가장 가까운 Enemy 탐색
    /// </summary>
    /// <param name="radius">탐색 반경</param>
    /// <param name="faceTarget">찾은 타겟을 바라볼지 여부</param>
    /// <returns>가장 가까운 Enemy Transform</returns>
    protected Transform FindNearestMonster(float radius, bool faceTarget = false)
    {
        Collider[] hits = Physics.OverlapSphere(
            Player.transform.position,
            radius,
            LayerMask.GetMask("Enemy")
        );
        Transform nearest = null;
        float minDist = float.MaxValue;
        foreach (var hit in hits)
        {
            // 한 단계 위 부모 가져오기
            Transform target = hit.transform.parent ?? hit.transform;

            float dist = Vector3.Distance(
                Player.transform.position,
                target.position
            );

            if (dist < minDist)
            {
                minDist = dist;
                nearest = target;
            }
        }
        if (faceTarget && nearest != null)
        {
            Vector3 dir = (nearest.position - Player.transform.position).normalized;
            dir.y = 0;
            Player.transform.forward = dir;
        }
        return nearest;
    }

    public void UpdateAttackTarget()
    {
        var target = FindNearestMonster(Player.InfoData.AttackData.AttackRange, true);
        if (target != null)
            Player._camera.ToggleLockOnTarget(target);
    }
}