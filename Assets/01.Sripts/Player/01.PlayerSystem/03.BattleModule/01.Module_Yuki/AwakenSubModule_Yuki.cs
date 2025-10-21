using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AwakenSubModule_Yuki
{
    public event Action OnAwakenEnd;

    private PlayerStateMachine sm;
    private bool isAwakened;

    private const float DecayRate = 10f;
    private const float AwakenThreshold = 50f;

    public bool IsAwakened => isAwakened;

    public AwakenSubModule_Yuki(PlayerStateMachine sm)
    {
        this.sm = sm;
    }

    public void OnUpdate()
    {
        if (!isAwakened) return;

        // 각성 상태에서 게이지 감소
        var gauge = sm.Player.Attr.AwakenGauge;
        gauge.Add(-DecayRate * Time.deltaTime);

        if (gauge.Current <= 0f)
            ExitAwakenedMode();
    }

    public void OnEnemyHit(IDamageable target)
    {
        var gauge = sm.Player.Attr.AwakenGauge;
        if (gauge == null || IsAwakened) return;

        gauge.Add(3f);
    }

    public void CheckAwakenHoldStart()
    {
        if (!isCheckingHold)
            HoldCheckAsync().Forget();
    }

    public void OnAttackCanceled() => isHoldingAttack = false;

    private bool isHoldingAttack;
    private bool isCheckingHold;
    private float holdStartTime;

    private async UniTask HoldCheckAsync()
    {
        if (isCheckingHold) return;
        isCheckingHold = true;

        try
        {
            isHoldingAttack = true;
            holdStartTime = Time.time;

            await UniTask.WaitUntil(() =>
                !isHoldingAttack || Time.time - holdStartTime >= 0.8f
            );

            if (isHoldingAttack)
                await TryEnterAwakenedMode();
        }
        finally
        {
            isCheckingHold = false;
        }
    }

    private async UniTask TryEnterAwakenedMode()
    {
        var gauge = sm.Player.Attr.AwakenGauge;
        if (gauge.Current >= AwakenThreshold && !isAwakened)
        {
            await EnterAwakenedMode();
            gauge.Use(); // 사용 시작 → 자동 감소 시작
        }
    }

    private async UniTask EnterAwakenedMode()
    {
        if (isAwakened) return;

        isAwakened = true;

        // 연출 시작
        sm.Player.Animator.CrossFade("Awaken", 0.1f);
        sm.Player.skill.SpawnSkill("Awaken", sm.Player.Body.position, sm.Player.Body.rotation);

        if (sm.Player.ForceReceiver != null)
        {
            sm.Player.ForceReceiver.AddForce(-sm.Player.transform.forward * 10f, horizontalOnly: true);
            sm.Player.ForceReceiver.BeginVerticalHold(1f, 1f);
        }

        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        // 후반 연출
        sm.Player.skill.SpawnSkill("Awaken2", sm.Player.Body.position, sm.Player.Body.rotation);
        sm.Player.ForceReceiver?.EndVerticalHold();

        sm.Player.Animator.SetTrigger("Base/Toggle_AwakenExit");
        sm.Player._camera?.SetColorGradingEnabled(true);
    }

    private void ExitAwakenedMode()
    {
        if (!isAwakened) return;

        isAwakened = false;
        sm.Player._camera?.SetColorGradingEnabled(false);

        OnAwakenEnd?.Invoke();
    }
}