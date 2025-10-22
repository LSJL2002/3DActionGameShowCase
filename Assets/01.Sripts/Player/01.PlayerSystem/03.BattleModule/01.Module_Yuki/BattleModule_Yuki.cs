using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;


public class BattleModule_Yuki : BattleModule
{
    private ComboSubModule_Yuki comboSub;
    private SkillSubModule_Yuki skillSub;
    private AwakenSubModule_Yuki awakenSub;

    public BattleModule_Yuki(PlayerStateMachine sm) : base(sm)
    {
        comboSub = new ComboSubModule_Yuki(sm);
        skillSub = new SkillSubModule_Yuki(sm);
        awakenSub = new AwakenSubModule_Yuki(sm);

        // 초기 콤보 핸들러 연결
        comboHandler = comboSub.ComboHandler;

        // 하위 모듈의 이벤트를 받아서 자신 이벤트로 중계
        skillSub.OnSkillEnd += RaiseSkillEnd;
        comboSub.OnComboEnd += RaiseAttackEnd;
        // 각성 종료 시 콤보 초기화 + AttackEnd 이벤트 전달
        awakenSub.OnAwakenEnd += () =>
        {
            comboSub.ResetCombo();   // 콤보 상태 초기화
            RaiseAttackEnd();        // FSM에 공격 종료 알림
        };
    }

    // AbilitySystem 초기화 후 BattleModule도 초기화할 때 호출
    public override void Initialize(InputSystem input)
    {
        base.Initialize(input);
        // Attack Hold 입력 구독
        input.HoldSystem.OnHoldTriggered += OnAttackHoldTriggered;
    }

    public override void Dispose()
    {
        input.HoldSystem.OnHoldTriggered -= OnAttackHoldTriggered;
    }

    private void OnAttackHoldTriggered(string actionName)
    {
        if (actionName == "Attack")
            awakenSub.TryEnterAwakenedMode().Forget();
    }

    // ================== 기본 공격 =================
    public override void OnAttackStart()
    {
        comboSub.SetAwakened(awakenSub.IsAwakened);
        comboSub.OnAttack();
    }

    public override void OnAttackCanceled()
    {
        comboSub.OnAttackCanceled();
        awakenSub.OnAttackCanceled();
    }

    // ================== 스킬 =================
    public override void OnSkillStart() => skillSub.OnSkillStart();
    public override void OnSkillCanceled() => skillSub.OnSkillCanceled();


    // ================== 업데이트 =================
    public override void OnUpdate()
    {
        comboSub.OnUpdate();
        awakenSub.OnUpdate();
    }
    public override void OnSkillUpdate() => skillSub.OnSkillUpdate();


    // ================== 기타 =================
    public override void OnEnemyHit(IDamageable target, Vector3 hitPoint, float damageMultiplier = 1f)
    {
        comboSub.OnEnemyHit(target);
        awakenSub.OnEnemyHit(target);
    }

    public override void ResetCombo() => comboSub.ResetCombo();
}