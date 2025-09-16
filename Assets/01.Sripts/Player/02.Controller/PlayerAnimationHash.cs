using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnimationHash
{
    [Header("Ground / Movement")]
    [SerializeField] private string groundBoolName = "@Ground";
    [SerializeField] private string idleBoolName = "Idle";
    [SerializeField] private string moveSpeedFloatName = "MovementSpeed"; //Blend

    [Header("Air / Jump / Fall")]
    [SerializeField] private string airBoolName = "@Air";
    [SerializeField] private string jumpTriggerName = "Jump"; //Trigger로 바꿔야함
    [SerializeField] private string fallBoolName = "Fall";

    [Header("Combat")]
    [SerializeField] private string attackBoolName = "@Attack";
    [SerializeField] private string attackTriggerName = "Attack"; // Trigger (AnyState 공격)
    [SerializeField] private string comboAttackBoolName = "ComboAttack"; // Bool (콤보 진입 중)
    [SerializeField] private string comboIntName = "ComboIndex"; // Int (콤보 상태)
    [SerializeField] private string finishAttackName = "FinishAttack"; // Trigger

    [SerializeField] private string dodgeParameterName = "Dodge";
    [SerializeField] private string dodgeDirParameterName = "DodgeDir";



    // ===== 해시 값 =====
    public int GroundBoolHash { get; private set; }
    public int IdleBoolHash { get; private set; }
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



    // ===== 초기화 =====
    public void Initialize()
    {
        GroundBoolHash = Animator.StringToHash(groundBoolName);
        IdleBoolHash = Animator.StringToHash(idleBoolName);
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
    }
}