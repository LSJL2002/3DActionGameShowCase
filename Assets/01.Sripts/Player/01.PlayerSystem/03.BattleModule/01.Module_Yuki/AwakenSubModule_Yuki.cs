using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AwakenSubModule_Yuki
{
    public event Action OnAwakenEnd;

    private PlayerStateMachine sm;
    private bool isAwakened;
    private float currentGauge;

    private const float DecayRate = 10f;
    private const float AwakenThreshold = 100f;

    private bool isHoldingAttack;
    private bool isCheckingHold;
    private float holdStartTime;

    public bool IsAwakened => isAwakened;

    public AwakenSubModule_Yuki(PlayerStateMachine sm)
    {
        this.sm = sm;
        currentGauge = 0f;
    }

    public void OnUpdate()
    {
        if (!isAwakened) return;

        currentGauge -= DecayRate * Time.deltaTime;
        if (currentGauge <= 0f)
            ExitAwakenedMode();
    }

    public void OnEnemyHit(IDamageable target)
    {
        if (!isAwakened)
            sm.Player.Stats.AddAwakenGauge(3f);
    }

    public void CheckAwakenHoldStart()
    {
        if (!isCheckingHold)
            HoldCheckAsync().Forget();
    }

    public void OnAttackCanceled() => isHoldingAttack = false;

    private async UniTask HoldCheckAsync()
    {
        isCheckingHold = true;
        try
        {
            isHoldingAttack = true;
            holdStartTime = Time.time;

            await UniTask.WaitUntil(() => !isHoldingAttack || Time.time - holdStartTime >= 0.5f);

            if (isHoldingAttack && !isAwakened)
                await TryEnterAwakenedMode();
        }
        finally
        {
            isCheckingHold = false;
        }
    }

    private async UniTask TryEnterAwakenedMode()
    {
        var stats = sm.Player.Stats;
        if (stats.AwakenGauge >= AwakenThreshold)
        {
            await EnterAwakenedMode();
            stats.AwakenGauge = 0;
        }
    }

    private async UniTask EnterAwakenedMode()
    {
        if (isAwakened) return;

        isAwakened = true;
        currentGauge = sm.Player.Stats.MaxAwakenGauge;

        // 연출 시작
        sm.Player.Animator.CrossFade("Awaken", 0.1f);
        sm.Player.skill.SpawnSkill("Awaken", sm.Player.Body.position, sm.Player.Body.rotation);

        // 연출용 물리효과
        if (sm.Player.ForceReceiver != null)
        {
            sm.Player.ForceReceiver.AddForce(-sm.Player.transform.forward * 10f, horizontalOnly: true);
            sm.Player.ForceReceiver.BeginVerticalHold(1f, 1f);
        }

        // 1초 연출 대기
        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        // 후반 연출
        sm.Player.skill.SpawnSkill("Awaken2", sm.Player.Body.position, sm.Player.Body.rotation);
        sm.Player.ForceReceiver?.EndVerticalHold();

        sm.Player.Animator.SetTrigger("Base/Toggle_AwakenExit");
        sm.Player._camera?.SetColorGradingEnabled(true);

        await UniTask.CompletedTask;
        sm.ChangeState(sm.IdleState);
    }

    private void ExitAwakenedMode()
    {
        if (!isAwakened) return;
        isAwakened = false;
        sm.Player._camera?.SetColorGradingEnabled(false);

        OnAwakenEnd?.Invoke(); // FSM으로 알림
    }
}
