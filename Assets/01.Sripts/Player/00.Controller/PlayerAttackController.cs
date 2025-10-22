using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class PlayerAttackController : MonoBehaviour
{
    private PlayerCharacter player;
    private SkillManagers skill;
    private HitboxOverlap hit;
    private CameraManager _camera;
    private HitStopManager hitStop;

    private Transform spawnPoint;

    [Header("Debug / Gizmos")]
    private PlayerInfo playerInfo;
    [SerializeField] private Transform debugAttackTarget;
    public Transform CurrentAttackTarget
    {
        get
        {
            if (_camera != null) debugAttackTarget = _camera.GetLockOnTarget();
            return debugAttackTarget;
        }
    }

    public void Inject(PlayerCharacter player)
    {
        this.player = player;

        skill = player.skill;
        _camera = player._camera;
        hitStop = player.hitStop;
        hit = player.Hitbox;
        spawnPoint = player.Body;
        playerInfo = player.InfoData;

        hit.OnHit += (target, hitPoint) => HandleHit(target, hitPoint, 1f);
    }

    /// <summary>
    /// 단일 공격 호출 (근접 + 원거리 공용)
    /// </summary>
    public void OnAttackDefalt(string skillName)
    {
        if (string.IsNullOrEmpty(skillName)) return;

        // 현재 스킬/캐릭터에 맞는 공격 타입 결정
        AttackType currentType = hit.attackType;

        switch (currentType)
        {
            case AttackType.Melee:
            case AttackType.InstantRange:
                // HitboxOverlap 호출
                hit.FireSkill();
                break;

            case AttackType.Projectile:
                // ProjectileHitbox 생성
                Vector3 dir = (CurrentAttackTarget != null)
                    ? (CurrentAttackTarget.position - spawnPoint.position).normalized
                    : spawnPoint.forward;

                skill.SpawnSkill(skillName, spawnPoint.position, Quaternion.LookRotation(dir),
                    (target, hitPoint) => HandleHit(target, hitPoint, 1f))
                    ?.GetComponentInChildren<ProjectileHitbox>()?.Launch(spawnPoint.position, dir);
                break;
        }
    }

    /// <summary>
    /// 다단히트 공격: 타수(hitCount)와 간격(interval) 조절 가능
    /// </summary>
    public void OnAttack(string skillName, int hitCount = 1, float interval = 0.1f, float damageMultiplier = 1f)
    {
        ComboAttackAsync(skillName, hitCount, interval, damageMultiplier).Forget();
    }

    private async UniTaskVoid ComboAttackAsync(string skillName, int hitCount, float interval, float damageMultiplier)
    {
        // 파티클 위치 & 회전 결정
        Vector3 particlePos;
        Quaternion particleRot;

        switch (hit.attackType)
        {
            case AttackType.Melee:
                // 근접은 spawnPoint 기준
                particlePos = spawnPoint.position;
                particleRot = spawnPoint.rotation;
                break;

            case AttackType.InstantRange:
                // 즉발형: 타겟 있으면 타겟 위치, 없으면 캐릭터 앞쪽으로 offset
                particlePos = (CurrentAttackTarget != null)
                    ? CurrentAttackTarget.position
                    : spawnPoint.position + spawnPoint.forward * 5f; // 앞쪽 offset
                particleRot = (CurrentAttackTarget != null)
                    ? Quaternion.LookRotation((CurrentAttackTarget.position - spawnPoint.position).normalized)
                    : spawnPoint.rotation;
                break;

            case AttackType.Projectile:
                // 투사체형: spawnPoint에서 발사, 방향 캐릭터 -> 타겟 또는 정면
                particlePos = spawnPoint.position;
                particleRot = (CurrentAttackTarget != null)
                    ? Quaternion.LookRotation((CurrentAttackTarget.position - spawnPoint.position).normalized)
                    : spawnPoint.rotation;
                break;

            default:
                particlePos = spawnPoint.position;
                particleRot = spawnPoint.rotation;
                break;
        }

        // 파티클 1회 재생
        GameObject skillObj = skill.SpawnSkill(skillName, particlePos, particleRot,
            (target, hitPoint) => HandleHit(target, hitPoint, damageMultiplier));

        // 투사체형이면 Launch 호출
        Vector3 dir = (CurrentAttackTarget != null)
            ? (CurrentAttackTarget.position - spawnPoint.position).normalized
            : spawnPoint.forward;
        skillObj?.GetComponentInChildren<ProjectileHitbox>()?.Launch(spawnPoint.position, dir);

        // 타수만큼 HitboxOverlap 반복 (근접 + 즉발형 모두 적용)
        for (int i = 0; i < hitCount; i++)
        {
            if (hit.attackType != AttackType.Projectile)
            {
                hit.FireSkill();
            }

            await UniTask.Delay(TimeSpan.FromSeconds(interval));
        }
    }

    public void HandleHit(IDamageable target, Vector3 hitPoint, float damageMultiplier = 1f)
    {
        int damage = Mathf.RoundToInt(player.Attr.Attack.Value * damageMultiplier);
        target.OnTakeDamage(damage);

        // 타격 효과 & 카메라 흔들림
        string fxName = player.CharacterType switch
        {
            CharacterType.Yuki => "Hit1",
            CharacterType.Aoi => "Hit11",
            CharacterType.Mika => "Hit21",
            _ => "Hit1"
        };
        // FX 재생
        skill.SpawnSkill(fxName, hitPoint);

        _camera?.Shake(2f, 0.2f);
        hitStop.DoHitStop();

        // 맞았다는 걸 BattleModule에 알림
        player.StateMachine.CurrentBattleModule?.OnEnemyHit(target, hitPoint, damageMultiplier);
    }



    // ========기즈모 시각화 (씬뷰에서 공격 범위/Force 확인용)==============
    private void OnDrawGizmosSelected()
    {
        player = GetComponent<PlayerCharacter>();
        playerInfo = player.InfoData;


        if (playerInfo == null || playerInfo.AttackData == null)
            return;

        var attackData = playerInfo.AttackData;

        // 공격 범위 (공용)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackData.AttackRange);

        // 돌진 멈춤 거리 (공용)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackData.StopDistance);

        // Force 방향 표시 (공용 기본값만 사용)
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.5f);

        // 타겟 연결선
        Transform target = CurrentAttackTarget; // 편집 모드에서 연결한 필드 사용
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position);
            Gizmos.DrawWireSphere(target.position, 0.5f);
        }
    }
}