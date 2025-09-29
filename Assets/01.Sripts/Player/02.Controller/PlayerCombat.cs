using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.VFX;
using Zenject.SpaceFighter;
using static SkillSO;
using static UnityEngine.Rendering.DebugUI;

//전투관련 로직만
public class PlayerCombat : MonoBehaviour, IDamageable
{
    private PlayerManager player;
    private SkillManagers skillManager;
    private ForceReceiver forceReceiver;


    [Header("Debug / Gizmos")]
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private int attackIndex = 0; // 인스펙터에서 공격 선택

    [SerializeField] private Transform currentAttackTarget; // 인스펙터용
    public Transform CurrentAttackTarget
    {
        get => currentAttackTarget;
        private set => currentAttackTarget = value;
    }

    private void Awake()
    {
        player ??= GetComponent<PlayerManager>();
        skillManager = player.skill;
        playerInfo = player.InfoData;

        forceReceiver = GetComponent<ForceReceiver>();
    }


    /// 공격 입력 시 호출 에니메이션 이벤트로 조작
    public void OnAttack(string skillName)
    {
        var skillObj = skillManager.SpawnSkill(skillName);

        // Hitbox 연결
        var skillHitbox = skillObj.GetComponentInChildren<Hitbox>();
        if (skillHitbox != null)
            skillHitbox.OnHit += HandleHit;

        // ParticleSystem 재생
        var ps = skillObj.GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();

        // 일정 시간 후 반환 처리
        float duration = ps != null ? ps.main.duration : 1f;

        // DOTween Sequence로 통합 처리
        Sequence seq = DOTween.Sequence();

        // duration 시간 동안 대기
        seq.AppendInterval(duration);

        // Hitbox 이벤트 해제 + VFX 종료
        seq.AppendCallback(() =>
        {
            if (skillHitbox != null)
                skillHitbox.OnHit -= HandleHit;


            // 필요하면 카메라 셰이크도 여기서 호출 가능
            // CameraShake.Instance?.Shake(0.2f, 1f);
        });

        // Sequence 재생
        seq.Play();
    }


    public void SetAttackTarget(Transform target)
    {
        CurrentAttackTarget = target;
    }


    private void HandleHit(IDamageable target)
    {
        int damage = Mathf.RoundToInt(player.Stats.Attack.Value);
        target.OnTakeDamage(damage);

        // 디버그: 몇 번 호출됐는지
        Debug.Log($"Hit! 대상: {target}, 피해량: {damage}");
    }


    // IDamageable 구현 예시 (플레이어가 맞았을 때)
    public void OnTakeDamage(int amount)
    {
        player?.Stats.TakeDamage(amount); // HP 변경은 Stats에서만
        // 피격 애니메이션, 넉백 등도 여기서 처리 가능
    }

    public void ApplyEffect(MonsterEffectType Type, Vector3 sourcePos, float value = 0, float duration = 0)
    {
        switch (Type)
        {
            case MonsterEffectType.Knockback:
                var dir = (transform.position - sourcePos).normalized;
                player.stateMachine.KnockbackState.Setup(dir, value, duration);
                player.stateMachine.ChangeState(player.stateMachine.KnockbackState);
                break;

            case MonsterEffectType.Groggy:
                player.stateMachine.StunState.Setup(duration);
                player.stateMachine.ChangeState(player.stateMachine.StunState);
                break;
        }
    }
    
    // -------------------------------
    // 기즈모 시각화 (씬뷰에서 공격 범위/Force 확인용)
    private void OnDrawGizmosSelected()
    {
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
        Transform target = currentAttackTarget; // 편집 모드에서 연결한 필드 사용
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position);
            Gizmos.DrawWireSphere(target.position, 0.5f);
        }
    }


    [Header("타겟 뒤로 이동 설정")]
    private float sideOffset = 1.5f;   // 옆으로 회피 거리
    private float behindOffset = 3f; // 타겟 뒤로 갈 거리
    private float moveDuration = 0.25f;// 이동 속도(짧을수록 빠름)
    private int pathPoints = 5;       // 중간점 개수 (커브 부드러움)

    /// <summary>
    /// 타겟의 뒤쪽으로 우측 횡이동 후 순간이동 (애니메이션 이벤트로 호출)
    /// </summary>
    public void MoveBehindTarget()
    {
        if (CurrentAttackTarget == null || forceReceiver == null) return;

        Vector3 startPos = transform.position;

        // 1️⃣ 타겟 뒤 방향 계산 (XZ 평면)
        Vector3 rawDir = CurrentAttackTarget.position - startPos; // 플레이어 → 타겟
        rawDir.y = 0f;
        Vector3 playerToTarget = rawDir.normalized;

        // 2️⃣ targetBehind 계산 + Y값을 플레이어 지면 높이로 고정
        Vector3 targetBehind = CurrentAttackTarget.position + playerToTarget * behindOffset;
        targetBehind.y = transform.position.y;

        // 3️⃣ 오른쪽 방향 (횡이동)
        Vector3 right = Vector3.Cross(Vector3.up, playerToTarget).normalized;

        // 4️⃣ 경로 중간점 생성 (Catmull-Rom용)
        Vector3[] path = new Vector3[pathPoints + 2]; // 시작점 + 중간점 + 끝점
        path[0] = startPos;

        for (int i = 1; i <= pathPoints; i++)
        {
            float t = (float)i / (pathPoints + 1);
            Vector3 point = Vector3.Lerp(startPos, targetBehind, t);
            point += right * Mathf.Sin(t * Mathf.PI) * sideOffset; // 반원형 곡선
            point.y = transform.position.y; // Y 고정
            path[i] = point;
        }

        path[path.Length - 1] = targetBehind;

        // 5️⃣ DOTween 이동
        float elapsed = 0f;
        DOTween.To(() => elapsed, x => elapsed = x, 1f, moveDuration)
            .SetEase(Ease.OutQuad)
            .OnUpdate(() =>
            {
                float t = elapsed;
                Vector3 newPos = CatmullRomPath(path, t);

                // 6️⃣ 이동
                Vector3 delta = newPos - transform.position;
                forceReceiver.AddForce(delta, horizontalOnly: true);

                // 7️⃣ 타겟 바라보기 (XZ 평면만)
                Vector3 lookDir = CurrentAttackTarget.position - transform.position;
                lookDir.y = 0f; // Y 제거
                if (lookDir != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDir, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);
                }

                // 8️⃣ 실제 캐릭터 이동
                player.Controller.Move(forceReceiver.Movement * Time.deltaTime);
            })
            .OnComplete(() =>
            {
                // 이동 끝나면 타겟 바라보기 최종 회전 고정
                Vector3 lookDir = CurrentAttackTarget.position - transform.position;
                lookDir.y = 0f;
                if (lookDir != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDir, Vector3.up);
                    transform.rotation = targetRotation;
                }
            });

        // 9️⃣ 디버그 시각화
        Debug.DrawLine(CurrentAttackTarget.position, targetBehind, Color.red, 5f);
        float flatDist = Vector3.Distance(
            new Vector3(CurrentAttackTarget.position.x, 0f, CurrentAttackTarget.position.z),
            new Vector3(targetBehind.x, 0f, targetBehind.z)
        );
        Debug.Log($"타겟 뒤 XZ 거리: {flatDist}");
    }

    // Catmull-Rom 보간 (n점)
    private Vector3 CatmullRomPath(Vector3[] points, float t)
    {
        int numSections = points.Length - 3;
        int currPt = Mathf.Min(Mathf.FloorToInt(t * numSections), numSections - 1);
        float u = t * numSections - currPt;

        Vector3 a = points[currPt];
        Vector3 b = points[currPt + 1];
        Vector3 c = points[currPt + 2];
        Vector3 d = points[currPt + 3];

        float u2 = u * u;
        float u3 = u2 * u;

        return 0.5f * ((2f * b) +
                       (-a + c) * u +
                       (2f * a - 5f * b + 4f * c - d) * u2 +
                       (-a + 3f * b - 3f * c + d) * u3);
    }
}