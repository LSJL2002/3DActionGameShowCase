using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHash
{
    [Header("Ground / Movement")]
    private string groundBoolName = "Base/Switch_GroundState";
    private string idleBoolName = "Base/Toggle_Idle";
    private string waitingAnimationTriggerName = "Base/Toggle_Idle2"; //Trigger
    private string getUpTriggerName = "Base/Toggle_GetUp"; //Trigger

    private string moveSpeedFloatName = "Base/Blend_MovementSpeed"; //Blend
    private string horizontalFloatName = "Horizontal";  //Blend
    private string verticalFloatName = "Vertical"; //Blend


    [Header("Air / Jump / Fall")]
    private string airBoolName = "Base/Switch_AirState";
    private string jumpTriggerName = "Base/Toggle_Jump"; //Trigger로 바꿔야함
    private string fallBoolName = "Base/Toggle_Fall";

    [Header("Combat")]
    private string attackBoolName = "Base/Switch_AttackState";
    private string attackTriggerName = "Base/Toggle_Attack"; // Trigger (AnyState 공격)
    private string finishAttackName = "Base/Toggle_FinishAttack"; // Trigger
    private string comboAttackName = "Base/Toggle_ComboAttack"; // Trigger (콤보 공격)
    private string comboIntName = "Base/Index_ComboIndex"; // Int (콤보 상태)

    private string dodgeParameterName = "Dodge/Switch_DodgeState";
    private string dodgeDirParameterName = "Dodge/Direction_DodgeDir";

    private string dieParameterName = "Base/Toggle_DieState";




    // ===== 해시 값 =====
    public int GroundBoolHash { get; private set; }
    public int IdleBoolHash { get; private set; }
    public int WaitingAnimationTriggerHash { get; private set; }
    public int GetUpTriggerHash { get; private set; }
    public int MoveSpeedHash { get; private set; }
    public int HorizontalHash { get; private set; }
    public int VerticalHash { get; private set; }


    public int AirBoolHash { get; private set; }
    public int JumpTriggerHash { get; private set; }
    public int FallBoolHash { get; private set; }

    public int AttackBoolHash { get; private set; }
    public int AttackTriggerHash { get; private set; }
    public int ComboTriggerHash { get; private set; }
    public int ComboIntHash { get; private set; }
    public int FinishAttackHash { get; private set; }

    public int DodgeParameterHash { get; private set; }
    public int DodgeDirParameterHash { get; private set; }

    public int DieParameterHash { get; private set; }




    // ===== 초기화 =====
    public void Initialize()
    {
        GroundBoolHash = Animator.StringToHash(groundBoolName);
        IdleBoolHash = Animator.StringToHash(idleBoolName);
        WaitingAnimationTriggerHash = Animator.StringToHash(waitingAnimationTriggerName);
        GetUpTriggerHash = Animator.StringToHash(getUpTriggerName);
        MoveSpeedHash = Animator.StringToHash(moveSpeedFloatName);
        HorizontalHash = Animator.StringToHash(horizontalFloatName);
        VerticalHash = Animator.StringToHash(verticalFloatName);

        AirBoolHash = Animator.StringToHash(airBoolName);
        JumpTriggerHash = Animator.StringToHash(jumpTriggerName);
        FallBoolHash = Animator.StringToHash(fallBoolName);

        AttackBoolHash = Animator.StringToHash(attackBoolName);
        AttackTriggerHash = Animator.StringToHash(attackTriggerName);
        ComboTriggerHash = Animator.StringToHash(comboAttackName);
        ComboIntHash = Animator.StringToHash(comboIntName);
        FinishAttackHash = Animator.StringToHash(finishAttackName);

        DodgeParameterHash = Animator.StringToHash(dodgeParameterName);
        DodgeDirParameterHash = Animator.StringToHash(dodgeDirParameterName);

        DieParameterHash = Animator.StringToHash(dieParameterName);
    }
}