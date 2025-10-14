using System;
using System.Collections.Generic;
using UnityEngine;

public class ComboSubModule_Yuki
{
    public event Action OnComboEnd;

    private PlayerStateMachine sm;
    public ComboHandler ComboHandler { get; private set; }

    private List<AttackInfoData> normalAttacks;
    private List<AttackInfoData> awakenedAttacks;
    private bool isAwakened;

    public ComboSubModule_Yuki(PlayerStateMachine sm)
    {
        this.sm = sm;
        normalAttacks = sm.Player.InfoData.AttackData.AttackInfoDatas;
        awakenedAttacks = (sm.Player.InfoData.ModuleData as ModuleData_Yuki)?.AttackInfoDatas;
        ComboHandler = new ComboHandler(normalAttacks, sm.Player.Animator, sm.Player.Attack);

        // 콤보 핸들러에 콜백 연결
        ComboHandler.OnComboFinished += HandleComboEnd;
    }

    public void SetAwakened(bool value)
    {
        if (isAwakened == value) return;
        isAwakened = value;
        var list = isAwakened ? awakenedAttacks : normalAttacks;
        ComboHandler = new ComboHandler(list, sm.Player.Animator, sm.Player.Attack);

        ComboHandler.OnComboFinished += HandleComboEnd; // 새 핸들러에 이벤트 재연결
    }
    private void HandleComboEnd()
    {
        OnComboEnd?.Invoke(); // FSM으로 전달
    }


    public void OnAttack() => ComboHandler.RegisterInput();
    public void OnAttackCanceled() { }
    public void OnUpdate() => ComboHandler.Update();
    public void OnEnemyHit(IDamageable target) { }
    public void ResetCombo() => ComboHandler = null;
}
