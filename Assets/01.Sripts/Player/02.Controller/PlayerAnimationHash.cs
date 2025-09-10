using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnimationHash
{
    [SerializeField] private string groundParameterName = "@Ground";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string moveSpeedParameterName = "MovementSpeed";
    [SerializeField] private string shiftParameterName = "Shift";

    [SerializeField] private string airParameterName = "@Air";
    [SerializeField] private string jumpParameterName = "Jump";
    [SerializeField] private string fallParameterName = "Fall";

    [SerializeField] private string attackParameterName = "@Attack";
    [SerializeField] private string comboAttackParameterName = "ComboAttack";

    [SerializeField] private string baseAttackParameterName = "BaseAttack";


    public int GroundParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int MoveSpeedParameterHash { get; private set; }
    public int ShiftParameterHash { get; private set; }


    public int AirParameterHash { get; private set; }
    public int JumpParameterHash { get; private set; }
    public int FallParameterHash { get; private set; }

    public int AttackParameterHash { get; private set; }
    public int ComboAttackParameterHash { get; private set; }

    public int baseAttackParameterHash { get; private set; }


    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        MoveSpeedParameterHash = Animator.StringToHash(moveSpeedParameterName);
        ShiftParameterHash = Animator.StringToHash(shiftParameterName);

        AirParameterHash = Animator.StringToHash(airParameterName);
        JumpParameterHash = Animator.StringToHash(jumpParameterName);
        FallParameterHash = Animator.StringToHash(fallParameterName);

        AttackParameterHash = Animator.StringToHash(attackParameterName);
        ComboAttackParameterHash = Animator.StringToHash(comboAttackParameterName);

        baseAttackParameterHash = Animator.StringToHash(baseAttackParameterName);
    }
}