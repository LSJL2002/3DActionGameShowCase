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

    [Header("Air / Jump / Fall")]
    private string airBoolName = "Base/Switch_AirState";
    private string jumpTriggerName = "Base/Toggle_Jump"; //Trigger로 바꿔야함
    private string fallBoolName = "Base/Toggle_Fall";

    [Header("Attack")]
    private string attackBoolName = "Base/Switch_AttackState";
    private string finishAttackName = "Base/Toggle_FinishAttack"; // Trigger
    private string comboIntName = "Base/Index_ComboIndex"; // Int (콤보 상태)
    [Header("Skill")]
    private string skillBoolName = "Base/Switch_SkillState";
    private string skillIntName = "Base/Index_SkillIndex"; // Int (콤보 상태)

    [Header("Dodge")]
    private string dodgeParameterName = "Dodge/Switch_DodgeState";
    private string dodgeDirParameterName = "Dodge/Direction_DodgeDir";
    [Header("HitStop")]
    private string knockbackParameterName = "HitStop/Switch_KnockbackState";
    private string stunParameterName = "HitStop/Switch_StunState";

    [Header("Trigger")]
    private string dieParameterName = "Base/Toggle_DieState";
    private string swapOutParameterName = "Base/Switch_Swap_Out";
    private string swapInParameterName = "Base/Switch_Swap_In";

    [Header("Parameter")]
    private string horizontalFloatName = "Input/Horizontal";  //Blend
    private string verticalFloatName = "Input/Vertical";      //Blend
    private string velocity_XName = "Input/Velocity_X";       //Blend
    private string velocity_YName = "Input/Velocity_Y";       //Blend
    private string velocity_ZName = "Input/Velocity_Z";       //Blend


    // ============= 해시 값 ==============
    public int GroundBoolHash { get; private set; }
    public int IdleBoolHash { get; private set; }
    public int WaitingAnimationTriggerHash { get; private set; }
    public int GetUpTriggerHash { get; private set; }
    public int MoveSpeedHash { get; private set; }

    public int AirBoolHash { get; private set; }
    public int JumpTriggerHash { get; private set; }
    public int FallBoolHash { get; private set; }

    public int AttackBoolHash { get; private set; }
    public int ComboIntHash { get; private set; }
    public int FinishAttackHash { get; private set; }

    public int SkillBoolHash { get; private set; }
    public int SkillIntHash { get; private set; }

    public int DodgeParameterHash { get; private set; }
    public int DodgeDirParameterHash { get; private set; }
    public int KnockbackParameterHash { get; private set; }
    public int StunParameterHash { get; private set; }

    public int DieParameterHash { get; private set; }
    public int SwapOutParameterHash { get; private set; }
    public int SwapInParameterHash { get; private set; }

    public int HorizontalHash { get; private set; }
    public int VerticalHash { get; private set; }
    public int Velocity_XHash { get; private set; }
    public int Velocity_YHash { get; private set; }
    public int Velocity_ZHash { get; private set; }





    // ================== 초기화 ===================
    public void Initialize()
    {
        GroundBoolHash = Animator.StringToHash(groundBoolName);
        IdleBoolHash = Animator.StringToHash(idleBoolName);
        WaitingAnimationTriggerHash = Animator.StringToHash(waitingAnimationTriggerName);
        GetUpTriggerHash = Animator.StringToHash(getUpTriggerName);
        MoveSpeedHash = Animator.StringToHash(moveSpeedFloatName);

        AirBoolHash = Animator.StringToHash(airBoolName);
        JumpTriggerHash = Animator.StringToHash(jumpTriggerName);
        FallBoolHash = Animator.StringToHash(fallBoolName);

        AttackBoolHash = Animator.StringToHash(attackBoolName);
        ComboIntHash = Animator.StringToHash(comboIntName);
        FinishAttackHash = Animator.StringToHash(finishAttackName);

        SkillBoolHash = Animator.StringToHash(skillBoolName);
        SkillIntHash = Animator.StringToHash(skillIntName);

        DodgeParameterHash = Animator.StringToHash(dodgeParameterName);
        DodgeDirParameterHash = Animator.StringToHash(dodgeDirParameterName);
        KnockbackParameterHash = Animator.StringToHash(knockbackParameterName);
        StunParameterHash = Animator.StringToHash(stunParameterName);

        DieParameterHash = Animator.StringToHash(dieParameterName);
        SwapOutParameterHash = Animator.StringToHash(swapOutParameterName);
        SwapInParameterHash = Animator.StringToHash(swapInParameterName);

        HorizontalHash = Animator.StringToHash(horizontalFloatName);
        VerticalHash = Animator.StringToHash(verticalFloatName);
        Velocity_XHash = Animator.StringToHash(velocity_XName);
        Velocity_YHash = Animator.StringToHash(velocity_YName);
        Velocity_ZHash = Animator.StringToHash(velocity_ZName);
    }
}