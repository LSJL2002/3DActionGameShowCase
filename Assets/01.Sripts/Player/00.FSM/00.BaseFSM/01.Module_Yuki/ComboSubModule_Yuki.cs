using System.Collections.Generic;
using UnityEngine;

public class ComboSubModule_Yuki
{
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
    }

    public void SetAwakened(bool value)
    {
        if (isAwakened == value) return;
        isAwakened = value;
        var list = isAwakened ? awakenedAttacks : normalAttacks;
        ComboHandler = new ComboHandler(list, sm.Player.Animator, sm.Player.Attack);
    }

    public void OnAttack() => ComboHandler.RegisterInput();
    public void OnAttackCanceled() { }
    public void OnUpdate() => ComboHandler.Update();
    public void OnEnemyHit(IDamageable target) { }
    public void ResetCombo() => ComboHandler = null;
}
