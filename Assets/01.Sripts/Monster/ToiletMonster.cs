using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToiletMonster : BaseMonster
{
    [Header("Pattern SO")]
    public MonsterAllPatternsSO allPatternsSO; // assign this in Inspector

    // Current pattern tracking
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
            string stateName = currentPattern.states[currentStepIndex];
            var state = ChangeStateFromString(stateName);

            if (state != null)
            {
                stateMachine.isAttacking = true;  // mark attack as started

                // wait until attack animation finishes
                yield return new WaitUntil(() => !stateMachine.isAttacking);

                // go back to idle briefly
                stateMachine.ChangeState(stateMachine.MonsterIdleState);
                yield return new WaitForSeconds(0.5f);
            }

            currentStepIndex++;
        }

        stateMachine.isAttacking = false; // pattern finished
        currentPattern = null;
    }


    private MonsterBaseState ChangeStateFromString(string stateName)
    {
        MonsterBaseState state = null;

        switch (stateName)
        {
            case "Slam":
                state = stateMachine.SmileToiletSlamState;
                break;
            case "Smash":
                state = stateMachine.SmileToiletSmashState;
                break;
            case "Charge":
                state = stateMachine.SmileToiletChargeState;
                break;
            case "BaseAttack":
                state = stateMachine.MonsterBaseAttack;
                break;
            default:
                Debug.LogWarning("Unknown state: " + stateName);
                break;
        }

        if (state != null)
            stateMachine.ChangeState(state);

        return state; // ✅ return the state
    }
}
