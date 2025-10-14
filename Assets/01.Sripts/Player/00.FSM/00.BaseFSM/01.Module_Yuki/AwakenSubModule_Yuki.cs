using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AwakenSubModule_Yuki
{
    private PlayerStateMachine sm;
    private bool isAwakened;
    private float currentGauge;
    private const float decayRate = 10f;
    private bool isHoldingAttack;
    private bool isCheckingHold;
    private float holdStartTime;
    private const float awakenThreshold = 100f;

    public bool IsAwakened => isAwakened;

    public AwakenSubModule_Yuki(PlayerStateMachine sm)
    {
        this.sm = sm;
        currentGauge = 0f;
    }

    public void OnUpdate()
    {
        if (!isAwakened) return;

        currentGauge -= decayRate * Time.deltaTime;
        if (currentGauge <= 0f)
            ExitAwakenedMode();
    }

    public void OnEnemyHit(IDamageable target)
    {
        if (!isAwakened)
            sm.Player.Stats.AddAwakenGauge(100f);
    }

    public void CheckAwakenHoldStart()
    {
        if (!isCheckingHold)
            HoldCheckAsync().Forget();
    }

    public void OnAttackCanceled()
    {
        isHoldingAttack = false;
    }

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
        currentGauge = sm.Player.Stats.MaxAwakenGauge;

        sm.Player.Animator.CrossFade("Awaken", 0.1f);
        sm.Player.skill.SpawnSkill("Awaken", sm.Player.Body.position, sm.Player.Body.rotation);

        // 공중 유지, 후퇴 힘
        if (sm.Player.ForceReceiver != null)
        {
            sm.Player.ForceReceiver.AddForce(-sm.Player.transform.forward * 10f, horizontalOnly: true);
            sm.Player.ForceReceiver.BeginVerticalHold(1f, 1f);
        }

        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        sm.Player.skill.SpawnSkill("Awaken2", sm.Player.Body.position, sm.Player.Body.rotation);
        sm.Player.ForceReceiver?.EndVerticalHold();

        sm.Player.Animator.SetTrigger("ExitAwaken");
        sm.Player.camera?.SetColorGradingEnabled(true);
    }

    private void ExitAwakenedMode()
    {
        isAwakened = false;
        sm.Player.camera?.SetColorGradingEnabled(false);
    }
}
