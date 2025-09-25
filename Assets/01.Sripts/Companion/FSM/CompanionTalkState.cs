using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;

public class CompanionTalkState : ICompanionState
{
    readonly CompanionStateMachine sm;
    CompanionController Ctx => sm.Ctx;

    Coroutine routine;
    bool exited;

    // UI가 나오는 동안 이동을 허용하는 변수
    float delay = 1.2f;
    float timer;

    public CompanionTalkState(CompanionStateMachine sm) { this.sm = sm; }

    public void Enter()
    {
        exited = false;
        timer = delay;

        // 오프셋 적용 (조력자를 플레이어 옆으로 이동시키기 위해)
        if (Ctx.targetObject != null)
        {
            Vector3 offset = new Vector3(1.03f, 0f, 1.2f);
            Ctx.targetObject.localPosition = Ctx.cachedAnchorLocalPos + offset;
        }

        // 입력 잠금
        PlayerManager.Instance?.EnableInput(false);

        // 커서 상태 캐시
        Ctx.cachedLockMode = Cursor.lockState;
        Ctx.cachedCursorVisible = Cursor.visible;

        // UI 지연 호출
        routine = Ctx.StartCoroutine(ShowTalkAfterDelay(delay));
    }

    IEnumerator ShowTalkAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (exited) yield break;

        // UI 열기
        _ = OpenTalkUIAsync();
    }

    private async UniTask OpenTalkUIAsync()
    {
        Ctx.ui = await UIManager.Instance.Show<CompanionUI>();
        if (exited || Ctx.ui == null) return;

        if (Ctx.chatUI) Ctx.chatUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Ctx.isTalkMode = true;
    }

    public void Exit()
    {
        exited = true;
        if (routine != null)
        {
            Ctx.StopCoroutine(routine);
            routine = null;
        }
    }

    public void HandleInput() { }

    public void Update()
    {
        // ★ 추가: UI가 뜨기 전에는 Follow처럼 이동 로직 수행
        if (timer > 0f && !exited)
        {
            timer -= Time.deltaTime;

            if (Ctx.targetObject != null && Ctx.rb != null)
            {
                // FollowState의 이동 로직을 그대로 가져옴
                Vector3 nextMove = Vector3.MoveTowards(
                    Ctx.rb.position,
                    Ctx.targetObject.position,
                    Ctx.moveSpeed * Time.deltaTime);

                Ctx.rb.MovePosition(nextMove);

                Vector3 dir = (Ctx.lookObject.position - Ctx.rb.position).normalized;
                dir = Vector3.ProjectOnPlane(dir, Vector3.up);
                if (dir.sqrMagnitude > 0.0001f)
                {
                    Quaternion look = Quaternion.LookRotation(dir, Vector3.up);
                    Quaternion nextRot = Quaternion.RotateTowards(
                        Ctx.rb.rotation, look, Ctx.rotationSpeed * Time.deltaTime);
                    Ctx.rb.MoveRotation(nextRot);
                }
            }
        }
    }

    public void PhysicsUpdate() { }
}
