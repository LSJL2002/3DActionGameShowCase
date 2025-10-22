using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AwakenSubModule_Yuki
{
    public event Action OnAwakenEnd;

    private readonly PlayerStateMachine sm;
    private bool isAwakened;

    private const float AwakenThreshold = 50f;

    public bool IsAwakened => isAwakened;

    public AwakenSubModule_Yuki(PlayerStateMachine sm)
    {
        this.sm = sm;
    }

    public void OnUpdate()
    {
        if (!isAwakened) return;

        var gauge = sm.Player.Attr.AwakenGauge;
        if (gauge.Current <= 0f)
            ExitAwakenedMode();
    }

    public void OnEnemyHit(IDamageable target)
    {
        var gauge = sm.Player.Attr.AwakenGauge;
        if (gauge == null || IsAwakened) return;

        gauge.Add(3f);
    }

    public void OnAttackCanceled() { }



    public async UniTask TryEnterAwakenedMode()
    {
        var gauge = sm.Player.Attr.AwakenGauge;
        if (gauge.Current < AwakenThreshold || isAwakened) return;

        await EnterAwakenedMode();
        gauge.Use();
    }

    private async UniTask EnterAwakenedMode()
    {
        if (isAwakened) return;

        isAwakened = true;

        // 연출 시작
        var awakenLayerIndex = sm.Player.Animator.GetLayerIndex("Overall/Toggle_SpecialLayer");
        sm.Player.Animator.SetLayerWeight(awakenLayerIndex, 1f);
        sm.Player.Animator.SetTrigger("Action/Toggle_AwakenExit");

        sm.Player.skill.SpawnSkill("Awaken", sm.Player.Body.position, sm.Player.Body.rotation);
        sm.Player.ForceReceiver?.AddForce(-sm.Player.transform.forward * 15f, horizontalOnly: true);
        sm.Player.ForceReceiver?.BeginVerticalHold(1f, 1f);

        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        // 후반 연출
        sm.Player.skill.SpawnSkill("Awaken2", sm.Player.Body.position, sm.Player.Body.rotation);
        sm.Player.ForceReceiver?.EndVerticalHold();

        sm.Player.Animator.SetLayerWeight(awakenLayerIndex, 0f);
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