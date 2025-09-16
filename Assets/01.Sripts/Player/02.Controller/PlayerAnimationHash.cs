using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnimationHash
{
    [Header("Ground / Movement")]
    [SerializeField] private string groundBoolName = "Base/Switch_GroundState";
    [SerializeField] private string idleBoolName = "Base/Toggle_Idle";
    [SerializeField] private string waitingAnimationTriggerName = "Base/Toggle_Idle2"; //Trigger
    [SerializeField] private string getUpTriggerName = "Base/Toggle_GetUp"; //Trigger

    [SerializeField] private string moveSpeedFloatName = "Base/Blend_MovementSpeed"; //Blend

    [Header("Air / Jump / Fall")]
    [SerializeField] private string airBoolName = "Base/Switch_AirState";
    [SerializeField] private string jumpTriggerName = "Base/Toggle_Jump"; //Trigger로 바꿔야함
    [SerializeField] private string fallBoolName = "Base/Toggle_Fall";

    [Header("Combat")]
    [SerializeField] private string attackBoolName = "Base/Switch_AttackState";
    [SerializeField] private string attackTriggerName = "Base/Toggle_Attack"; // Trigger (AnyState 공격)
    [SerializeField] private string finishAttackName = "Base/Toggle_FinishAttack"; // Trigger
    [SerializeField] private string comboAttackBoolName = "Base/Toggle_ComboAttack"; // Bool (콤보 진입 중)
    [SerializeField] private string comboIntName = "Base/Index_ComboIndex"; // Int (콤보 상태)

    [SerializeField] private string dodgeParameterName = "Dodge/Switch_DodgeState";
    [SerializeField] private string dodgeDirParameterName = "Dodge/Direction_DodgeDir";

    [SerializeField] private string dieParameterName = "Base/Toggle_DieState";




    // ===== 해시 값 =====
    public int GroundBoolHash { get; private set; }
    public int IdleBoolHash { get; private set; }
    public int WaitingAnimationTriggerHash { get; private set; }
    public int GetUpTriggerHash { get; private set; }
    public int MoveSpeedHash { get; private set; }

    public int AirBoolHash { get; private set; }
    public int JumpTriggerHash { get; private set; }
    public int FallBoolHash { get; private set; }

    public int AttackBoolHash { get; private set; }
    public int AttackTriggerHash { get; private set; }
    public int ComboBoolHash { get; private set; }
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

        AirBoolHash = Animator.StringToHash(airBoolName);
        JumpTriggerHash = Animator.StringToHash(jumpTriggerName);
        FallBoolHash = Animator.StringToHash(fallBoolName);

        AttackBoolHash = Animator.StringToHash(attackBoolName);
        AttackTriggerHash = Animator.StringToHash(attackTriggerName);
        ComboBoolHash = Animator.StringToHash(comboAttackBoolName);
        ComboIntHash = Animator.StringToHash(comboIntName);
        FinishAttackHash = Animator.StringToHash(finishAttackName);

        DodgeParameterHash = Animator.StringToHash(dodgeParameterName);
        DodgeDirParameterHash = Animator.StringToHash(dodgeDirParameterName);

        DieParameterHash = Animator.StringToHash(dieParameterName);
    }
}