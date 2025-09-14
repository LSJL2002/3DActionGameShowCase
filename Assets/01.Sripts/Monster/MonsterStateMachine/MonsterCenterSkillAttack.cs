using System.Collections;
using UnityEngine;

public class MonsterCenterSkillAttack : MonsterBaseState
{
    private MonsterSkillSO skill;
    private GameObject aoeInstance;
    private AreaEffectController aoeController;

    public MonsterCenterSkillAttack(MonsterStateMachine stateMachine, MonsterSkillSO skillData) 
        : base(stateMachine)
    {
        skill = skillData;
    }

    public override void Enter()
    {

        //Debug Logs of Using Skill
        Debug.Log($"[CenterSkillAttack] Entering with skill: {skill?.skillName}");
        if (skill == null)
        {
            Debug.LogError("Skill is NULL!");
            return;
        }
        if (skill.areaEffectPrefab == null)
        {
            Debug.LogError($"Skill {skill.skillName} has NO areaEffectPrefab assigned!");
            return;
        }

        // Find the monster's AreaEffect spawn point
        Transform areaEffectPoint = stateMachine.Monster.transform.Find("AreaEffect");
        if (areaEffectPoint == null)
        {
            Debug.LogError("Monster has no child named 'AreaEffect'!");
            return;
        }

        // Instantiate at the position but NOT as a child
        aoeInstance = Object.Instantiate(skill.areaEffectPrefab, areaEffectPoint.position, areaEffectPoint.rotation);

        aoeController = aoeInstance.GetComponent<AreaEffectController>();
        if (aoeController == null)
        {
            Debug.LogError("Prefab has no AreaEffectController attached!");
            return;
        }
        //End of Debug Log

        // Initialize with telegraph cast time & range
        aoeController.Initialize(skill.preCastTime, skill.range, stateMachine.Monster.Stats.AttackPower);

        // Subscribe to telegraph finished event
        aoeController.OnTelegraphFinished += OnTelegraphComplete;
    }

    private void OnTelegraphComplete()
    {
        // Play monster's attack animation
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Attack));
    }

    // Called by Animation Event
    public void EnableDamage()
    {
        aoeController?.EnableDamage();

        // Destroy prefab right after damage triggers
        if (aoeInstance != null)
        {
            Object.Destroy(aoeInstance, 0.5f); // small delay so OnTriggerEnter can fire
        }
    }

    // Called by Animation Event
    public void DisableDamage()
    {
        aoeController?.DisableDamage();
    }

    public override void Exit()
    {
        if (aoeController != null)
            aoeController.OnTelegraphFinished -= OnTelegraphComplete;

        if (aoeInstance != null)
            Object.Destroy(aoeInstance);
    }
}
