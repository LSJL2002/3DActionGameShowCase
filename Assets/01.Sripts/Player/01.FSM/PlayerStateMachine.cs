using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public PlayerManager Player { get; }

    // 원본 데이터
    public readonly PlayerGroundData GroundData;
    public readonly PlayerAirData AirData;
    // 현재 공격 데이터 (SO에서 가져온 참조본)
    public AttackInfoData AttackInfo { get; private set; }

    public Vector2 MovementInput { get; set; } // 입력 방향 (WASD, 스틱)
    public float MovementSpeed { get; private set; } // 현재 이동 속도
    public float RotationDamping { get; private set; } // 회전할 때 부드럽게 보정하는 값

    private float _movementSpeedModifier = 1f;
    public float MovementSpeedModifier // 속도 보정 계수
    { 
        get => _movementSpeedModifier; 
        set 
        {
            _movementSpeedModifier = value;
            Player.Animator.SetFloat(
            Player.AnimationData.MoveSpeedParameterHash,
            MovementSpeedModifier);
        }
    }

    public float JumpForce { get; set; } //점프력
    public bool IsInvincible { get; set; } //무적상태
    public bool IsDodge { get; set; } //회피상태
    public bool IsAttacking {  get; set; } //공격중인지
    public int ComboIndex {  get; set; } //콤보인덱스

    public Transform MainCamTransform { get; set; }

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
    // 독립적인 Sub-State Dodge 로직
    public PlayerDodgeState DodgeState { get; }



    public PlayerStateMachine(PlayerManager player)
    {
        this.Player = player;

        MainCamTransform = Camera.main.transform;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        DodgeState = new PlayerDodgeState(this);
        JumpState = new PlayerJumpState(this);
        FallState = new PlayerFallState(this);
        AttackState = new PlayerAttackState(this);
        ComboAttackState = new PlayerComboAttackState(this);

        MovementSpeed = player.Data.GroundData.BaseSpeed;
        RotationDamping = player.Data.GroundData.BaseRotationDamping;
        GroundData = player.Data.GroundData;

        // AttackInfo 초기화
        ComboIndex = 0;
        SetAttackInfo(ComboIndex);
    }

    public void SetAttackInfo(int comboIndex)
    {
        ComboIndex = comboIndex;
        AttackInfo = Player.Data.AttackData.GetAttackInfoData(comboIndex);
    }
}