using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;


public class BattleModule_Yuki : BattleModule
{
    private List<AttackInfoData> normalAttacks;
    private List<AttackInfoData> awakenedAttacks;
    private bool isAwakened;
    public bool IsAwakened => isAwakened;
    private bool isHoldingAttack;
    private bool isCheckingHold;
    private float holdStartTime;
    private float awakenThreshold = 100f;

    private float awakenDecayRate = 10f; // 1초당 감소량 (게이지 기준)
    private float currentAwakenGauge;

    public BattleModule_Yuki(PlayerStateMachine sm) : base(sm)
    {
        // 입력 콜백 등록은 필요하면 여기서(또는 외부에서)해도됨
        // 기본적으로 입력은 PlayerBaseState -> stateMachine.HandleAttackInput() 으로 들어온다.
        // 기본/각성 평타 데이터 캐싱
        normalAttacks = sm.Player.InfoData.AttackData.AttackInfoDatas;
        awakenedAttacks = (sm.Player.InfoData.ModuleData as ModuleData_Yuki)?.AttackInfoDatas;

        // comboHandler 초기화 → 항상 기본 평타 콤보
        comboHandler = new ComboHandler(normalAttacks, sm.Player.Animator, sm.Player.Attack);
    }

    public override void OnAttack()
    {
        isHoldingAttack = true;
        holdStartTime = Time.time;

        var attacks = isAwakened ? awakenedAttacks : normalAttacks;
        if (attacks == null)
            return;

        // 각성 상태가 바뀌면 ComboHandler를 새로 만들어줌
        if (comboHandler == null ||
            comboHandler.AttackListReference != attacks) // Attack 리스트 비교
        {
            comboHandler = new ComboHandler(attacks, sm.Player.Animator, sm.Player.Attack);
        }

        comboHandler.RegisterInput();

        // 홀드 검사 시작
        if (!isCheckingHold)
            CheckHoldForAwaken().Forget();
    }

    public override void OnSkill()
    {
        // Yuki 특수 스킬 없음
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!isAwakened) return;

        // 매 프레임 게이지 감소
        currentAwakenGauge -= awakenDecayRate * Time.deltaTime;

        if (currentAwakenGauge <= 0f)
        {
            ExitAwakenedMode();
        }
    }

    public override void OnAttackCanceled() // PlayerBaseState에서 호출하도록 연결
    {
        isHoldingAttack = false;
        // 혹시 콤보 진행 중이면 그대로 두기
        // 필요하다면 여기서 comboHandler.Reset() 추가 가능
    }

    // 공격이 성공했을 때 호출
    public override void OnEnemyHit(IDamageable target)
    {
        if (!isAwakened)
        {
            // 일반 상태일 때만 게이지 상승
            sm.Player.Stats.AddAwakenGauge(100f);
        }
        else
        {
            // 각성 상태일 때는 게이지 채우지 않음
            // (필요하면 유지시간 약간 연장해도 됨)
            // currentAwakenGauge = Mathf.Min(currentAwakenGauge + 5f, sm.Player.Stats.MaxAwakenGauge);
        }
    }

    // ============= 비동기 홀드 검사 (권장: UniTask) ================
    private async UniTask CheckHoldForAwaken()
    {
        isCheckingHold = true;

        try
        {
            // 조건 만족(버튼 떼이거나 홀드시간 초과)까지 대기
            await UniTask.WaitUntil(() => !isHoldingAttack || Time.time - holdStartTime >= 0.5f);

            // 여기에 도달한 건: 버튼이 여전히 눌려있거나 떼였기 때문
            if (isHoldingAttack && !isAwakened)
            {
                await TryEnterAwakenedMode(); // await 가능한 UniTask
            }
        }
        finally
        {
            isCheckingHold = false;
        }
    }

    private async UniTask TryEnterAwakenedMode()
    {
        var stats = sm.Player.Stats;
        if (stats.AwakenGauge >= awakenThreshold)
        {
            await EnterAwakenedMode();
            stats.AwakenGauge = 0;
        }
    }

    private async UniTask EnterAwakenedMode()
    {
        if (isAwakened) return;

        isAwakened = true;
        currentAwakenGauge = sm.Player.Stats.MaxAwakenGauge; // 현재 게이지 기반으로 시작
        sm.Player.Animator.CrossFade("Awaken", 0.1f);

        comboHandler = new ComboHandler(awakenedAttacks, sm.Player.Animator, sm.Player.Attack);

        sm.Player.skill.SpawnSkill("Awaken", sm.Player.Body.position, sm.Player.Body.rotation);

        if (sm.Player.ForceReceiver != null)
        {
            Vector3 backwardForce = -sm.Player.transform.forward * 10f;
            sm.Player.ForceReceiver.AddForce(backwardForce, horizontalOnly: true);

            // 공중으로 부드럽게 올라가서 2초 유지
            sm.Player.ForceReceiver.BeginVerticalHold(1f, 1f);
        }


        // 2초 대기 (연출 타이밍)
        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        sm.Player.skill.SpawnSkill("Awaken2", sm.Player.Body.position, sm.Player.Body.rotation);


        // 공중 유지 해제 → 이제 Y축 중력 적용
        sm.Player.ForceReceiver?.EndVerticalHold();

        // 트리거 발동 → 다음 상태로 부드럽게 전환
        sm.Player.Animator.SetTrigger("ExitAwaken");

        // 화면 어둡게
        sm.Player.camera?.SetColorGradingEnabled(true);
    }

    private void ExitAwakenedMode()
    {
        isAwakened = false;
        comboHandler = new ComboHandler(normalAttacks, sm.Player.Animator, sm.Player.Attack);

        // 각성 종료 시 화면 원래대로
        sm.Player.camera?.SetColorGradingEnabled(false);
    }
}