using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletMonster : BaseMonster
{
    private MonsterPatternSO.PatternEntry currentPattern;
    private int currentStepIndex = 0;
    private bool isRunningPattern = false;

    //Colliders for BaseAttacks
    public Collider baseAttackCollider;

    private IEnumerator RunPattern()
    {
        isRunningPattern = true;
        stateMachine.isAttacking = true;

        while (currentPattern != null && currentStepIndex < currentPattern.states.Count)
        {
            if (IsDead)
            {
                Debug.Log($"{name} - Pattern stopped: Monster is dead.");
                yield break;
            }

            var stateEnum = currentPattern.states[currentStepIndex];
            var state = GetStateFromEnum(stateEnum);

            if (state != null)
            {
                float skillRange = GetSkillRangeFromState(state);
                float startTime = Time.time;

                // keep chasing until safely inside range
                while (Vector3.Distance(transform.position, PlayerTarget.position) > skillRange * 0.8f)
                {
                    if (IsDead) yield break;

                    stateMachine.ChangeState(stateMachine.MonsterChaseState);

                    if (Time.time - startTime >= 5f)
                    {
                        Debug.Log($"{name} - Abandoning pattern: player out of range.");
                        stateMachine.isAttacking = false;
                        currentPattern = null;
                        isRunningPattern = false;
                        yield break;
                    }
                    yield return null;
                }

                // lock in idle before cast
                stateMachine.ChangeState(stateMachine.MonsterIdleState);
                yield return new WaitForSeconds(0.5f);

                // perform attack
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

    public void PickPatternByCondition()
    {
        if (patternConfig == null || isRunningPattern)
        {
            return;
        }

        float hpPercent = (Stats.CurrentHP / Stats.maxHp) * 100f;
        float distance = PlayerTarget != null ? Vector3.Distance(transform.position, PlayerTarget.position) : Mathf.Infinity;

        var validConditions = patternConfig.GetValidConditions(hpPercent, distance);
        if (validConditions == null || validConditions.Count == 0)
        {
            return;
        }

        var chosenCondition = validConditions[0]; //제일 높은 우선순위 (1->2->3)

        if (chosenCondition.possiblePatternIds.Count == 0)
        {
            return;
        }

        int patternId = chosenCondition.possiblePatternIds[Random.Range(0, chosenCondition.possiblePatternIds.Count)];

        currentPattern = patternConfig.GetPatternById(patternId);
        if (currentPattern == null) return;

        //Apply multipliers

        currentStepIndex = 0;
        StartCoroutine(RunPattern());
    }

    private MonsterBaseState GetStateFromEnum(States stateEnum)
    {
        switch (stateEnum)
        {
            case States.Skill1: return stateMachine.SmileToiletSmashState;
            case States.Skill2: return stateMachine.SmileToiletSlamState;
            case States.Skill3: return stateMachine.SmileToiletChargeState;
            case States.BaseAttack: return stateMachine.MonsterBaseAttack;
            case States.BaseAttack2: return stateMachine.MonsterBaseAttackAlt;
            default:
                Debug.LogWarning($"{name} - Unknown state enum: {stateEnum}");
                return null;
        }
    }

    private float GetSkillRangeFromState(MonsterBaseState state)
    {
        switch (state)
        {
            case SmileToiletSlamState:
                return Stats.GetSkill("SmileMachine_Slam").range / 2f;
            case SmileToiletSmashState:
                return Stats.GetSkill("SmileMachine_Smash").range / 2f;
            case SmileToiletChargeState:
                return Stats.GetSkill("SmileMachine_Charge").range;
            case MonsterBaseAttack:
            case MonsterBaseAttackAlt:
                return Stats.AttackRange;
            default:
                return 0f;
        }
    }
}
