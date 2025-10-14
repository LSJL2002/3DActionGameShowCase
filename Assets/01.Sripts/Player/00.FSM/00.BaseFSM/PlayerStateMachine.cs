using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;




public class PlayerStateMachine : StateMachine
{
    public PlayerManager Player { get; }

    public BattleModule CurrentBattleModule { get; private set; }

    public void SetBattleModule(BattleModule module)
    {
        CurrentBattleModule = module;
    }

    public void HandleAttackInput() => CurrentBattleModule?.OnAttack();
    public void HandleSkillInput() => CurrentBattleModule?.OnSkill();
    public void HandleUpdate() => CurrentBattleModule?.OnUpdate();
    public void HandleSkillUpdate()
    {
        if (IsSkill)
            CurrentBattleModule?.OnSkillUpdate();
    }


    // 원본 데이터
    public readonly PlayerGroundData GroundData;
    public readonly PlayerAirData AirData;
    // 현재 공격 데이터 (SO에서 가져온 참조본)
    public AttackInfoData AttackInfo { get; private set; }

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
    public bool IsInvincible { get; set; } //무적상태
    public bool IsDodge { get; set; } //회피상태
    public bool IsAttacking { get; set; } //공격중인지
    public int ComboIndex { get; set; } //콤보인덱스
    public bool IsKnockback { get; set; }
    public bool IsStun { get; set; }
    public bool IsSkill { get; set; }




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



    public PlayerStateMachine(PlayerManager player)
    {
        this.Player = player;

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

        MovementSpeed = player.InfoData.GroundData.BaseSpeed;
        RotationDamping = player.InfoData.GroundData.BaseRotationDamping;
        GroundData = player.InfoData.GroundData;

        // AttackInfo 초기화
        ComboIndex = 0;
        SetAttackInfo(ComboIndex);

        // 캐릭터 타입에 따른 전투 모듈 설정
        switch (player.CharacterType)
        {
            case CharacterType.Yuki:
                SetBattleModule(new BattleModule_Yuki(this));
                break;
            case CharacterType.Aoi:
                SetBattleModule(new BattleModule_Aoi(this));
                break;
            case CharacterType.Mika:
                SetBattleModule(new BattleModule_Mika(this));
                break;
        }
    }

    public void SetAttackInfo(int comboIndex)
    {
        ComboIndex = comboIndex;
        AttackInfo = Player.InfoData.AttackData.GetAttackInfoData(comboIndex);
    }
}