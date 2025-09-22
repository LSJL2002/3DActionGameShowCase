using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;

public class CompanionTalkState : ICompanionState
{
    readonly CompanionStateMachine sm;
    CompanionController Ctx => sm.Ctx;

    Coroutine routine;
    bool exited;

    public CompanionTalkState(CompanionStateMachine sm) { this.sm = sm; }

    public void Enter()
    {
        // 위치 오프셋 + 입력 잠금
        if (Ctx.targetObject)
        {
            Vector3 offset = new Vector3(1.03f, 0f, 1.2f);
            Ctx.targetObject.localPosition = Ctx.cachedAnchorLocalPos + offset;
        }
        PlayerManager.Instance?.EnableInput(false);

        // 커서 상태 캐시
        Ctx.cachedLockMode = Cursor.lockState;
        Ctx.cachedCursorVisible = Cursor.visible;

        routine = Ctx.StartCoroutine(ShowTalkAfterDelay_Coroutine(1.2f));
    }

    IEnumerator ShowTalkAfterDelay_Coroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        OpenTalkUIAsync().Forget();
    }

    async UniTask OpenTalkUIAsync()
    {
        // 상태가 이미 종료됐다면 중단
        if (exited) return;

        Ctx.ui = await UIManager.Instance.Show<CompanionUI>();
        if (exited || Ctx.ui == null) return;

        if (Ctx.chatUI) Ctx.chatUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Ctx.isTalkMode = true;
    }

    public void Exit()
    {
        if (routine != null) { Ctx.StopCoroutine(routine); routine = null; }
        // 실제 닫기는 UI 버튼에서 Ctx.ExitTalkMode() 호출
    }

    public void HandleInput() { /* 대화 중엔 입력 전환 없음 (버튼으로 종료) */ }
    public void Update() { }
    public void PhysicsUpdate() { }
}
