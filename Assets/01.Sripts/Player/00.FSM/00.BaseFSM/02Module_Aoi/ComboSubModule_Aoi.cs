using System;
using System.Collections.Generic;
using UnityEngine;

public class ComboSubModule_Aoi
{
    public event Action OnComboEnd;
    public ComboHandler ComboHandler { get; private set; }

    private PlayerStateMachine sm;
    private List<AttackInfoData> normalAttacks;

    public ComboSubModule_Aoi(PlayerStateMachine sm)
    {
        this.sm = sm;
        normalAttacks = sm.Player.InfoData.AttackData.AttackInfoDatas;
        ComboHandler = new ComboHandler(normalAttacks, sm.Player.Animator, sm.Player.Attack);

        ComboHandler.OnComboFinished += HandleComboEnd;
    }

    private void HandleComboEnd() => OnComboEnd?.Invoke();

    public void OnAttack() => ComboHandler.RegisterInput();
    public void OnAttackCanceled() { }
    public void OnUpdate() => ComboHandler.Update();
    public void OnEnemyHit(IDamageable target) { }
    public void ResetCombo()
    {
        if (ComboHandler == null) return;
        ComboHandler.OnComboFinished -= HandleComboEnd;
        ComboHandler.Reset();
        ComboHandler.OnComboFinished += HandleComboEnd;
    }
}
