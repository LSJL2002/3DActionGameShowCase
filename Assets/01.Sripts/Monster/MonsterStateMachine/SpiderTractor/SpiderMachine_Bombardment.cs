using System.Collections;
using UnityEngine;

public class SpiderMachine_Bombardment : MonsterBaseState
{
    private MonsterSkillSO skillData;
    private Transform firePointMissile;
    private GameObject aoeInstance;

    public SpiderMachine_Bombardment(MonsterStateMachine ms, MonsterSkillSO bombardmentSkill) : base(ms)
    {
        skillData = bombardmentSkill;
        if (ms.Monster is SpiderTractor_UseGrenade monster)
            firePointMissile = monster.firePointMissile;
    }

    public override void Enter()
    {
        base.Enter();
        if (skillData == null || firePointMissile == null)
        {
            stateMachine.isAttacking = false;
            return;
        }

        stateMachine.isAttacking = true;
        StopMoving();
        StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Idle));

        if (stateMachine.Monster is SpiderTractor_UseGrenade monster)
        {
            // Setup AOE and spawn multiple circles
            var aoeController = stateMachine.Monster.AreaEffectPoint.GetComponent<AreaEffectController>();
            if (aoeController == null)
                aoeController = stateMachine.Monster.AreaEffectPoint.gameObject.AddComponent<AreaEffectController>();

            // Start coroutine to handle missiles and animation cleanup
            stateMachine.Monster.StartCoroutine(HandleBombardment(aoeController, monster));
        }
    }

    private IEnumerator HandleBombardment(AreaEffectController aoeController, SpiderTractor_UseGrenade monster)
    {
        bool allDone = false;
        int totalCircles = 10;
        int finishedCount = 0;
        bool startedSkillAnimation = false;

        System.Action<Vector3> onCircleFinish = (Vector3 pos) =>
        {
            LaunchMissile(pos, monster, (int)(stateMachine.Monster.Stats.AttackPower));
            if (!startedSkillAnimation)
            {
                startedSkillAnimation = true;
                StartAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill4));
            }

            finishedCount++;
            if (finishedCount >= totalCircles)
                allDone = true;
        };

        aoeController.MultipleCircleInitialize(
            skillData.preCastTime,
            skillData.range,
            monster.Stats.AttackPower,
            skillData,
            totalCircles,
            5f,
            0.5f,
            monster,
            onCircleFinish
        );

        while (!allDone)
            yield return null;

        StopAnimation(stateMachine.Monster.animationData.GetHash(MonsterAnimationData.MonsterAnimationType.Skill4));
        stateMachine.isAttacking = false;
        stateMachine.ChangeState(stateMachine.MonsterIdleState);
    }


    private void LaunchMissile(Vector3 targetPos, SpiderTractor_UseGrenade monster, int damage)
    {
        if (monster.missile == null) return;

        Vector3 spawnPos = firePointMissile.position + firePointMissile.forward * 0.3f;
        GameObject missile = Object.Instantiate(monster.missile, spawnPos, Quaternion.LookRotation(firePointMissile.forward));

        // Ignore collisions with the monster
        if (missile.TryGetComponent<Collider>(out var missileCol) &&
            stateMachine.Monster.TryGetComponent<Collider>(out var monsterCol))
        {
            Physics.IgnoreCollision(missileCol, monsterCol);
        }

        // Ignore collisions with other missiles
        Missile[] otherMissiles = Object.FindObjectsOfType<Missile>();
        foreach (var other in otherMissiles)
        {
            if (other != null && other.TryGetComponent<Collider>(out var otherCol))
                Physics.IgnoreCollision(missileCol, otherCol);
        }

        if (missile.TryGetComponent<Rigidbody>(out var rb))
        {
            float arcHeight = 5f; // peak height of trajectory
            Vector3 velocity = CalculateLaunchVelocity(firePointMissile.position, targetPos, arcHeight);
            rb.useGravity = true;
            rb.linearVelocity = velocity;
        }

        if (missile.TryGetComponent<Missile>(out var missileScript))
            missileScript.Initialize(damage, stateMachine.Monster.transform);
    }

    private Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 end, float height)
    {
        float gravity = Mathf.Abs(Physics.gravity.y);

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
        stateMachine.isAttacking = false;

        if (aoeInstance != null)
        {
            stateMachine.Monster.UnregisterAOE(aoeInstance);
            Object.Destroy(aoeInstance);
        }
    }
}
