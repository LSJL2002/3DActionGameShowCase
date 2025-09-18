using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletMonster : BaseMonster
{
    [Header("Pattern SO")]
    public MonsterAllPatternsSO allPatternsSO; // assign in Inspector
    private MonsterAllPatternsSO.PatternEntry currentPattern;
    private int currentStepIndex = 0;

    public void PickPatternById(int id)
    {
        if (allPatternsSO == null) return;

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
        while (currentPattern != null && currentStepIndex < currentPattern.states.Count)
        {
            States stateEnum = currentPattern.states[currentStepIndex];
            var state = GetStateFromEnum(stateEnum);

            if (state != null)
            {
                stateMachine.isAttacking = true;

                // Get the skill's range from the state
                float skillRange = GetSkillRangeFromState(state);
                float startTime = Time.time;

                // ✅ Chase until player is in range or timeout
                while (Vector3.Distance(transform.position, stateMachine.Monster.PlayerTarget.position) > skillRange)
                {
                    stateMachine.ChangeState(stateMachine.MonsterChaseState);

                    if (Time.time - startTime >= 5f)
                    {
                        Debug.Log("Player out of range, abandoning pattern.");
                        stateMachine.isAttacking = false;
                        currentPattern = null;
                        yield break; // exit coroutine
                    }

                    yield return null;
                }

                // ✅ Force Idle before skill
                stateMachine.ChangeState(stateMachine.MonsterIdleState);
                yield return new WaitForEndOfFrame();
                yield return new WaitForSeconds(1f);

                // ✅ Now actually perform the skill
                stateMachine.ChangeState(state);
                yield return new WaitUntil(() => !stateMachine.isAttacking);

                // ✅ Brief idle after skill
                stateMachine.ChangeState(stateMachine.MonsterIdleState);
                yield return new WaitForSeconds(0.2f);
            }

            currentStepIndex++;
        }

        stateMachine.isAttacking = false;
        currentPattern = null;
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
