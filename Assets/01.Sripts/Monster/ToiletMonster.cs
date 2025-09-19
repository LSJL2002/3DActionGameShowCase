using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletMonster : BaseMonster
{
    [Header("Pattern SO")]
    public MonsterAllPatternsSO allPatternsSO;
    private MonsterAllPatternsSO.PatternEntry currentPattern;
    private int currentStepIndex = 0;
    private bool isRunningPattern = false;


    public void PickPatternById(int id)
    {
        if (allPatternsSO == null || isRunningPattern) return;

        currentPattern = allPatternsSO.GetPatternById(id);
        if (currentPattern == null)
        {
            Debug.LogWarning("Pattern ID not found: " + id);
            return;
        }

        currentStepIndex = 0;
        StartCoroutine(RunPattern());
    }


    private IEnumerator RunPattern()
    {
        isRunningPattern = true;
        stateMachine.isAttacking = true;

        while (currentPattern != null && currentStepIndex < currentPattern.states.Count)
        {
            // 🚫 if dead, stop immediately
            if (stateMachine.Monster.IsDead)
            {
                Debug.Log("Pattern stopped: Monster is dead.");
                yield break;
            }

            States stateEnum = currentPattern.states[currentStepIndex];
            var state = GetStateFromEnum(stateEnum);

            if (state != null)
            {
                float skillRange = GetSkillRangeFromState(state);
                float startTime = Time.time;

                while (Vector3.Distance(transform.position, stateMachine.Monster.PlayerTarget.position) > skillRange)
                {
                    // 🚫 if dead, stop immediately
                    if (stateMachine.Monster.IsDead)
                    {
                        Debug.Log("Pattern stopped while chasing: Monster is dead.");
                        yield break;
                    }

                    stateMachine.ChangeState(stateMachine.MonsterChaseState);

                    if (Time.time - startTime >= 5f)
                    {
                        Debug.Log("Player out of range, abandoning pattern.");
                        stateMachine.isAttacking = false;
                        currentPattern = null;
                        isRunningPattern = false;
                        yield break;
                    }

                    yield return null;
                }

                stateMachine.ChangeState(stateMachine.MonsterIdleState);
                yield return new WaitForSeconds(1f);

                stateMachine.ChangeState(state);
                yield return new WaitUntil(() => !stateMachine.isAttacking);

                stateMachine.ChangeState(stateMachine.MonsterIdleState);
                yield return new WaitForSeconds(0.2f);
            }

            currentStepIndex++;
        }

        stateMachine.isAttacking = false;
        currentPattern = null;
        isRunningPattern = false;
    }


    private MonsterBaseState GetStateFromEnum(States stateEnum)
    {
        switch (stateEnum)
        {
            case States.Skill1:
                return stateMachine.SmileToiletSmashState;
            case States.Skill2:
                return stateMachine.SmileToiletSlamState;
            case States.Skill3:
                return stateMachine.SmileToiletChargeState;
            case States.BaseAttack:
                return stateMachine.MonsterBaseAttack;
            case States.BaseAttack2:
                return stateMachine.MonsterBaseAttackAlt;
            
            default:
                Debug.LogWarning("Unknown state enum: " + stateEnum);
                return null;
        }
    }

    private float GetSkillRangeFromState(MonsterBaseState state)
    {
        switch (state)
        {
            case SmileToiletSlamState:
                return stateMachine.Monster.Stats.GetSkill("SmileMachine_Slam").range / 2;
            case SmileToiletSmashState:
                return stateMachine.Monster.Stats.GetSkill("SmileMachine_Smash").range / 2;
            case SmileToiletChargeState:
                return stateMachine.Monster.Stats.GetSkill("SmileMachine_Charge").range;
            case MonsterBaseAttack:
                return stateMachine.Monster.Stats.AttackRange;
            case MonsterBaseAttackAlt:
                return stateMachine.Monster.Stats.AttackRange;
            default:
                return 0f;
        }
    }
}
