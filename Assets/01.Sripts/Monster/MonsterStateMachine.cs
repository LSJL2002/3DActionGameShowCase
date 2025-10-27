using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.UIElements;

public class MonsterStateMachine : StateMachine
{
    public BaseMonster Monster { get; }
    public float MovementSpeedModifier { get; set; }
    public float RangeMultiplier = 1f;
    public float PreCastTimeMultiplier = 1f;
    public float EffectValueMultiplier = 1f;
    public MonsterAIEvents AIEvents => aiEvents;

    public MonsterDeathState MonsterDeathState { get; }
    public MonsterIdleState MonsterIdleState { get; }
    public MonsterChaseState MonsterChaseState { get; }
    public MonsterBaseAttack MonsterBaseAttack { get; private set; }
    public MonsterBaseAttackAlt MonsterBaseAttackAlt { get; private set; }

    //SmileToilet 스킬
    public SmileToiletSlamState SmileToiletSlamState { get; private set; }
    public SmileToiletSmashState SmileToiletSmashState { get; private set; }
    public SmileToiletChargeState SmileToiletChargeState { get; private set; }

    //SmileMachine_UseGun 스킬
    public SmileMachineShootState SmileMachineShootState { get; private set; }
    public SmileMachineGroggyShoot SmileMachineGroggyShoot { get; private set; }

    //smileMachine_UseFire 스킬
    public SmileMachineFire SmileMachineFire { get; private set; }
    public SmileMachine_FireShoot SmileMachine_FireShoot { get; private set; }

    //SmileMachine_UseMissile 스킬
    public SmileMachine_Missile SmileMachine_Missile { get; private set; }
    public SmileMachine_Gernade SmileMachine_Gernade { get; private set; }

    //SpiderMachine
    public SpiderMachine_AttackStamp SpiderMachine_AttackStamp { get; private set; }
    public SpiderMachine_TurnLeft SpiderMachine_TurnLeft { get; private set; }
    public SpiderMachine_FangFall SpiderMachine_FangFall { get; private set; }


    private MonsterAIEvents aiEvents;
    public bool isAttacking = false;

    private Istate currentStateInternal;
    public Istate CurrentState => currentStateInternal;

    public MonsterStateMachine(BaseMonster monster)
    {
        Monster = monster;
        MovementSpeedModifier = 1f;

        MonsterIdleState = new MonsterIdleState(this);
        MonsterChaseState = new MonsterChaseState(this);
        MonsterBaseAttack = new MonsterBaseAttack(this);
        MonsterDeathState = new MonsterDeathState(this);
        MonsterBaseAttackAlt = new MonsterBaseAttackAlt(this, MonsterAnimationData.MonsterAnimationType.BaseAttack2); // new animation

        if (monster is ToiletMonster)
        {
            var slamSkill = monster.Stats.GetSkill("SmileMachine_Slam");
            SmileToiletSlamState = new SmileToiletSlamState(this, slamSkill);
            var smashSkill = monster.Stats.GetSkill("SmileMachine_Smash");
            SmileToiletSmashState = new SmileToiletSmashState(this, smashSkill);
            var chargeSkill = monster.Stats.GetSkill("SmileMachine_Charge");
            SmileToiletChargeState = new SmileToiletChargeState(this, chargeSkill);
        }
        else if (monster is SmileMachine_UseGun)
        {
            var slamSkill = monster.Stats.GetSkill("SmileMachine_Slam");
            SmileToiletSlamState = new SmileToiletSlamState(this, slamSkill);
            var smashSkill = monster.Stats.GetSkill("SmileMachine_Smash");
            SmileToiletSmashState = new SmileToiletSmashState(this, smashSkill);
            var chargeSkill = monster.Stats.GetSkill("SmileMachine_Charge");
            SmileToiletChargeState = new SmileToiletChargeState(this, chargeSkill);
            var shootSkill = monster.Stats.GetSkill("SmileMachine_Shoot");
            SmileMachineShootState = new SmileMachineShootState(this, shootSkill);
            var groggyShootSkill = monster.Stats.GetSkill("SmileMachine_GroggyShoot");
            SmileMachineGroggyShoot = new SmileMachineGroggyShoot(this, groggyShootSkill);
        }
        else if (monster is SmileMachine_UseFire)
        {
            var slamSkill = monster.Stats.GetSkill("SmileMachine_Slam");
            SmileToiletSlamState = new SmileToiletSlamState(this, slamSkill);
            var smashSkill = monster.Stats.GetSkill("SmileMachine_Smash");
            SmileToiletSmashState = new SmileToiletSmashState(this, smashSkill);
            var chargeSkill = monster.Stats.GetSkill("SmileMachine_Charge");
            SmileToiletChargeState = new SmileToiletChargeState(this, chargeSkill);
            var fireSkill = monster.Stats.GetSkill("SmileMachine_Fire");
            SmileMachineFire = new SmileMachineFire(this, fireSkill);
            var fireShootSkill = monster.Stats.GetSkill("SmileMachine_FireShoot");
            SmileMachine_FireShoot = new SmileMachine_FireShoot(this, fireShootSkill);
        }
        else if (monster is SmileMachine_UseMissile)
        {
            var slamSkill = monster.Stats.GetSkill("SmileMachine_Slam");
            SmileToiletSlamState = new SmileToiletSlamState(this, slamSkill);
            var smashSkill = monster.Stats.GetSkill("SmileMachine_Smash");
            SmileToiletSmashState = new SmileToiletSmashState(this, smashSkill);
            var chargeSkill = monster.Stats.GetSkill("SmileMachine_Charge");
            SmileToiletChargeState = new SmileToiletChargeState(this, chargeSkill);
            var missileSkill = monster.Stats.GetSkill("SmileMachine_Missile");
            SmileMachine_Missile = new SmileMachine_Missile(this, missileSkill);
            var gernadeSkill = monster.Stats.GetSkill("SmileMachine_Grenade");
            SmileMachine_Gernade = new SmileMachine_Gernade(this, gernadeSkill);
        }
        else if (monster is SpiderTractor_UseGrenade)
        {
            var stampSkill = monster.Stats.GetSkill("SpiderMachine_AttackStamp");
            SpiderMachine_AttackStamp = new SpiderMachine_AttackStamp(this, stampSkill);
            var turnLeftSkill = monster.Stats.GetSkill("SpiderMachine_TurnLeft");
            SpiderMachine_TurnLeft = new SpiderMachine_TurnLeft(this, turnLeftSkill);
            var fangFallSkill = monster.Stats.GetSkill("SpiderMachine_Fangfall");
            SpiderMachine_FangFall = new SpiderMachine_FangFall(this, fangFallSkill);
        }

        aiEvents = monster.GetComponent<MonsterAIEvents>() ?? monster.gameObject.AddComponent<MonsterAIEvents>();
        aiEvents.SetStateMachine(this);

        EnableAIEvents();

        ChangeState(MonsterIdleState);
    }

    public new void ChangeState(Istate newState)
    {
        if (Monster.IsDead && !(newState is MonsterDeathState))
            return;
        base.ChangeState(newState);
        currentStateInternal = newState;
    }

    public void EnableAIEvents()
    {
        aiEvents.OnPlayerDetected += HandleChase;
        aiEvents.RestingPhase += HandleIdle;
        aiEvents.OnInAttackRange += HandlePlayerInAttackRange;
    }

    public void DisableAIEvents()
    {
        aiEvents.OnPlayerDetected -= HandleChase;
        aiEvents.RestingPhase -= HandleIdle;
        aiEvents.OnInAttackRange -= HandlePlayerInAttackRange;
    }

    private void HandleChase()
    {
        if (Monster.IsDead || isAttacking) 
            return;

        if (Monster.PlayerTarget != null)
        {
            float distance = Vector3.Distance(Monster.transform.position, Monster.PlayerTarget.position);

            if (!Monster.hasDetectedPlayer && distance <= Monster.Stats.AttackRange * 3f)
            {
                Monster.hasDetectedPlayer = true;
                Monster.Stats.DetectRange = 100;
            }

            if (Monster.hasDetectedPlayer)
            {
                ChangeState(MonsterChaseState);
            }
        }
    }


    private void HandleIdle()
    {
        if (!isAttacking)
        {
            ChangeState(MonsterIdleState);
        }
    }



    private void HandlePlayerInAttackRange()
    {
        if (!isAttacking && CurrentState == MonsterIdleState)
        {
            isAttacking = true;
            if (Monster is ToiletMonster toilet)
            {
                toilet.PickPatternByCondition();
            }
        }
    }
    //Enable or Disable the AI events
    public void DisableAIProcessing()
    {
        aiEvents?.Disable();
    }

    public void EnableAIProcessing()
    {
        aiEvents?.Enable();
    }
    void FixedUpdate()
    {
        Physicsupdate();
    }

    public float MovementSpeed => Monster.Stats.MoveSpeed * MovementSpeedModifier;
}
