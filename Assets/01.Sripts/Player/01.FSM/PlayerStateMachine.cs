using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public PlayerManager Player { get; }

    public Vector2 MovementInput { get; set; } // 입력 방향 (WASD, 스틱)
    public float MovementSpeed { get; private set; } // 현재 이동 속도
    public float RotationDamping { get; private set; } // 회전할 때 부드럽게 보정하는 값
    public float MovementSpeedModifier { get; set; } = 1f; // 속도 보정 계수
    public float JumpForce { get; set; } //점프력
    public bool IsAttacking {  get; set; } //공격중인지
    public int ComboIndex {  get; set; } //콤보인덱스

    public Transform MainCamTransform { get; set; }

    public PlayerIdleState IdleState { get;}
    public PlayerWalkState WalkState { get;}
    public PlayerRunState RunState { get;}
    public PlayerJumpState JumpState { get;}
    public PlayerFallState FallState { get;}
    public PlayerComboAttackState ComboAttackState { get; set; }


    public PlayerStateMachine(PlayerManager player)
    {
        this.Player = player;

        MainCamTransform = Camera.main.transform;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        JumpState = new PlayerJumpState(this);
        FallState = new PlayerFallState(this);
        ComboAttackState = new PlayerComboAttackState(this);

        MovementSpeed = player.Data.GroundData.BaseSpeed;
        RotationDamping = player.Data.GroundData.BaseRotationDamping;
    }
}