using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static CartoonFX.CFXR_Effect;


// 공격 처리 + 전투 대상 관리 + 기즈모 시각화
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

    private void Awake()
    {
        player = GetComponent<PlayerCharacter>();
    }

    private void Start()
    {
        skill = player.skill;
        _camera = player._camera;
        hitStop = player.hitStop;
        hit = player.Hit;
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

        // 근접 스킬 발동
        hit.FireSkill();

        if (CurrentAttackTarget != null)
        {
            Vector3 targetPos = CurrentAttackTarget.position;

            // 타격 위치에 스킬 생성
            skill.SpawnSkill(skillName, targetPos, Quaternion.identity,
                (target, hitPoint) => HandleHit(target, hitPoint, 1f));
        }
        else
        {
            // 타겟 없으면 기존 방식대로
            Vector3 dir = spawnPoint.forward;

            skill.SpawnSkill(skillName, spawnPoint.position, spawnPoint.rotation,
                (target, hitPoint) => HandleHit(target, hitPoint, 1f))
                ?.GetComponentInChildren<ProjectileHitbox>()?.Launch(spawnPoint.position, dir);
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
        for (int i = 0; i < hitCount; i++)
        {
            if (i == 0)
            {
                // 첫 히트만 파티클 재생
                hit.FireSkill();

                Vector3 dir = (CurrentAttackTarget != null)
                                ? (CurrentAttackTarget.position - spawnPoint.position).normalized
                                : spawnPoint.forward;

                skill.SpawnSkill(skillName, spawnPoint.position, spawnPoint.rotation,
                    (target, hitPoint) => HandleHit(target, hitPoint, damageMultiplier))
                    ?.GetComponentInChildren<ProjectileHitbox>()?.Launch(spawnPoint.position, dir);
            }
            else
            {
                // 매 히트마다 근접 히트박스 발동
                hit.FireSkill();
            }

            await UniTask.Delay(TimeSpan.FromSeconds(interval)); // 논블로킹 대기
        }
    }

    private void HandleHit(IDamageable target, Vector3 hitPoint, float damageMultiplier = 1f)
    {
        int damage = Mathf.RoundToInt(player.Stats.Attack.Value * damageMultiplier);
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