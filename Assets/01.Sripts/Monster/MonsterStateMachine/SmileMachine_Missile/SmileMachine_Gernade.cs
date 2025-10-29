using System.Collections;
using UnityEngine;

public class SmileMachine_Gernade : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private Coroutine grenadeRoutine;
    private Transform firepointGernade;
    private GameObject aoeInstance;
    private AreaEffectController aoeController;

    public SmileMachine_Gernade(MonsterStateMachine ms, MonsterSkillSO grenadeSkill) : base(ms)
    {
        skillData = grenadeSkill;
        var monster = ms.Monster as SmileMachine_UseMissile;

        if (monster != null)
            firepointGernade = monster.firepointGernade;
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.isAttacking = true;
        StopMoving();
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));

        grenadeRoutine = stateMachine.Monster.StartCoroutine(GrenadeRoutine());
    }

    private IEnumerator GrenadeRoutine()
    {
        Transform playerTarget = stateMachine.Monster.PlayerTarget;
        Transform firepoint = firepointGernade;

        if (playerTarget == null || firepoint == null)
        {
            stateMachine.isAttacking = false;
            yield break;
        }

        Vector3 targetPos = stateMachine.Monster.PlayerTarget.position;
        if (Physics.Raycast(targetPos + Vector3.up, Vector3.down, out RaycastHit hit, 10f, LayerMask.GetMask("Ground")))
            targetPos = hit.point;

        if (stateMachine.Monster.AreaEffectPoint != null)
            targetPos.y = stateMachine.Monster.AreaEffectPoint.transform.position.y;

        aoeInstance = Object.Instantiate(skillData.areaEffectPrefab, targetPos, Quaternion.identity);
        stateMachine.Monster.RegisterAOE(aoeInstance);

        aoeController = aoeInstance.GetComponent<AreaEffectController>();
        if (aoeController == null)
        {
            Debug.LogError("SmileMachine_Gernade: AOE prefab missing AreaEffectController!");
            stateMachine.isAttacking = false;
            yield break;
        }

        bool telegraphFinished = false;
        System.Action telegraphHandler = () => telegraphFinished = true;
        aoeController.OnTelegraphFinished += telegraphHandler;

        aoeController.CircleInitialize(
            skillData.preCastTime * stateMachine.PreCastTimeMultiplier,
            skillData.range * stateMachine.RangeMultiplier,
            (int)(stateMachine.Monster.Stats.AttackPower * stateMachine.EffectValueMultiplier),
            skillData
        );

        while (!telegraphFinished)
            yield return null;

        Vector3 latestPos = stateMachine.Monster.PlayerTarget.position;
        LaunchGrenade(latestPos, (int)(stateMachine.Monster.Stats.AttackPower));

        yield return new WaitForSeconds(1f);

        aoeController.OnTelegraphFinished -= telegraphHandler;

        stateMachine.isAttacking = false;
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }


    private void LaunchGrenade(Vector3 targetPos, int damage)
    {
        if (!(stateMachine.Monster is SmileMachine_UseMissile monster) || monster.gernade == null)
        {
            Debug.LogWarning("SmileMachine_Gernade: Missing grenade prefab!");
            return;
        }
        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill5));

        Vector3 spawnPos = firepointGernade.position + firepointGernade.forward * 0.3f;
        GameObject grenade = Object.Instantiate(monster.gernade, spawnPos, Quaternion.identity);

        if (grenade.TryGetComponent<Collider>(out var grenadeCol) &&
            stateMachine.Monster.TryGetComponent<Collider>(out var monsterCol))
        {
            Physics.IgnoreCollision(grenadeCol, monsterCol);
        }

        if (grenade.TryGetComponent<Rigidbody>(out var rb))
        {
            Vector3 start = firepointGernade.position;
            Vector3 end = targetPos;
            float height = 5f;

            Vector3 velocity = CalculateLaunchVelocity(start, end, height);
            rb.linearVelocity = velocity;
        }

        if (grenade.TryGetComponent<Gernade>(out var grenadeScript))
            grenadeScript.Initialize(damage, stateMachine.Monster.transform);
    }


    private Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 end, float height)
    {
        float gravity = Mathf.Abs(Physics.gravity.y);

        //시작 지점 고려해서 계산
        Vector3 displacementXZ = new Vector3(end.x - start.x, 0, end.z - start.z);
        float displacementY = end.y - start.y;

        float timeUp = Mathf.Sqrt(2 * height / gravity);

        float timeDown = Mathf.Sqrt(2 * (height - displacementY) / gravity);

        float totalTime = timeUp + timeDown;

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / totalTime;

        return velocityXZ + velocityY;
    }


    public override void Exit()
    {
        base.Exit();

        if (grenadeRoutine != null)
        {
            stateMachine.Monster.StopCoroutine(grenadeRoutine);
            grenadeRoutine = null;
        }

        if (aoeInstance != null)
        {
            stateMachine.Monster.UnregisterAOE(aoeInstance);
            Object.Destroy(aoeInstance);
        }

        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill5));
    }
}
