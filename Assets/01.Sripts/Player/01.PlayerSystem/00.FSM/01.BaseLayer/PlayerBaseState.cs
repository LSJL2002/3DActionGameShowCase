using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;


public abstract class PlayerBaseState : Istate
{
    protected PlayerStateMachine sm;

    public PlayerBaseState(PlayerStateMachine sm)
    {
        this.sm = sm;
    }

    // 상태 완료 이벤트
    public event Action<PlayerBaseState> OnStateComplete;

    protected void StateComplete()
    {
        OnStateComplete?.Invoke(this);
    }


    // ============ 공통 설정 =============
    public virtual bool AllowRotation => true;
    public virtual bool AllowMovement => true;

    // ============ 상태 진입 / 종료 =============
    public virtual void Enter() => AddInputActionCallbacks();
    public virtual void Exit() => RemoveInputActionCallbacks();
    protected void StartAnimation(int animatorHash) => sm.Player.Animator.SetBool(animatorHash, true);
    protected void StopAnimation(int animatorHash) => sm.Player.Animator.SetBool(animatorHash, false);

    // ============ 입력 콜백 등록 =============
    protected virtual void AddInputActionCallbacks()
    {
        PlayerController input = sm.Player.Input;
        input.PlayerActions.Move.canceled += OnMoveCanceled;
        input.PlayerActions.Dodge.started += OnDodgeStarted;
        input.PlayerActions.Attack.started += OnAttackStarted;
        input.PlayerActions.Attack.canceled += OnAttackCanceled;
        input.PlayerActions.HeavyAttack.started += OnSkillStarted;
        input.PlayerActions.HeavyAttack.canceled += OnSkillCanceled;

        input.PlayerActions.SwapNext.started += OnSwapNextStarted;
        input.PlayerActions.SwapPrev.started += OnSwapPrevStarted;

        input.PlayerActions.Menu.performed += OnMenuToggle;
        input.PlayerActions.Camera.started += OnLockOnToggle;
        input.PlayerActions.Inventory.started += OnInvenToggle;
    }

    protected virtual void RemoveInputActionCallbacks()
    {
        PlayerController input = sm.Player.Input;
        input.PlayerActions.Move.canceled -= OnMoveCanceled;
        input.PlayerActions.Dodge.started -= OnDodgeStarted;
        input.PlayerActions.Attack.started -= OnAttackStarted;
        input.PlayerActions.Attack.canceled -= OnAttackCanceled;
        input.PlayerActions.HeavyAttack.started -= OnSkillStarted;
        input.PlayerActions.HeavyAttack.canceled -= OnSkillCanceled;

        input.PlayerActions.SwapNext.started -= OnSwapNextStarted;
        input.PlayerActions.SwapPrev.started -= OnSwapPrevStarted;

        input.PlayerActions.Menu.performed -= OnMenuToggle;
        input.PlayerActions.Camera.started -= OnLockOnToggle;
        input.PlayerActions.Inventory.started -= OnInvenToggle;
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
        ReadZoomInput(); // 줌 값 읽기 추가

        // Animator에 입력값 전달 (공통 처리)
        sm.Player.Animator.SetFloat(
            sm.Player.AnimationData.HorizontalHash,
            sm.MovementInput.x
        );
        sm.Player.Animator.SetFloat(
            sm.Player.AnimationData.VerticalHash,
            sm.MovementInput.y
        );
    }
    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() => MoveCharacter();


    protected virtual void OnMoveCanceled(InputAction.CallbackContext context) { }
    protected virtual void OnDodgeStarted(InputAction.CallbackContext context) { }
    protected virtual void OnAttackStarted(InputAction.CallbackContext context)
    {
        //내부적으로 어떤 모듈이 연결되어 있든, FSM이 알아서 처리하도록 맡김
        //지금 누가 연결되어있는지 모름
        if (sm.IsSkill) return;
        sm.HandleAttackInput(); // 기본 공격 입력 → 콤보 모듈로 전달
    }
    protected virtual void OnAttackCanceled(InputAction.CallbackContext context)
    {
        // BattleModule에 취소 알림 전달
        sm.CurrentBattleModule?.OnAttackCanceled();
    }
    protected virtual void OnSkillStarted(InputAction.CallbackContext context)
    {
        // 스킬 입력 → 스킬 서브모듈 실행
        if (sm.IsAttacking) return;
        sm.HandleSkillInput();
    }
    protected virtual void OnSkillCanceled(InputAction.CallbackContext context)
    {
        sm.CurrentBattleModule?.OnSkillCanceled();
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context) { }

    protected virtual void OnSwapNextStarted(InputAction.CallbackContext context)
    {
        sm.Player.PlayerManager.SwapNext();
    }

    protected virtual void OnSwapPrevStarted(InputAction.CallbackContext context)
    {
        sm.Player.PlayerManager.SwapPrev();
    }

    protected virtual void OnLockOnToggle(InputAction.CallbackContext context)
    {
        var cam = sm.Player._camera;
        cam.ToggleLockOnTarget(null); // 무조건 락온 해제
    }

    private bool isPaused = false;
    protected virtual void OnMenuToggle(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        isPaused = !isPaused;
        GameManager.Instance.PauseGame(isPaused);
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            sm.Player._camera.Volume_Blur.enabled = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            sm.Player._camera.Volume_Blur.enabled = false;
        }
    }

    protected virtual void OnInvenToggle(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (sm.Player.direction.inventory == null) return;

        var inven = sm.Player.direction.inventory;
        bool isActive = !inven.activeSelf;
        inven.SetActive(isActive);

        // 인벤토리 켜면 시간 멈추기, 끄면 다시 정상
        Time.timeScale = isActive ? 0f : 1f;

        // 옵션: 커서 락 해제
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isActive;
    }

    // ========== 개별 입력 읽기 ==========
    private void ReadMovementInput()
    {
        var input = sm.Player.Input.PlayerActions.Move.ReadValue<Vector2>();
        sm.MovementInput = input;
    }

    private void ReadZoomInput()
    {
        float zoomDelta =
            sm.Player.Input.PlayerActions.Zoom.ReadValue<float>();

        if (Mathf.Abs(zoomDelta) > 0.01f)
            OnZoom(zoomDelta);
    }

    // ========== Zoom 처리 ==========
    private void OnZoom(float zoomDelta)
    {
        var vcam = sm.Player._camera.FreeLookCam;
        if (vcam == null) return;

        float fov = vcam.Lens.FieldOfView;
        float zoomSpeed = 3f;
        float zoomAmount = Mathf.Sign(zoomDelta) * zoomSpeed; // 방향만 가져오기
        fov -= zoomAmount; 
        fov = Mathf.Clamp(fov, 10f, 70f);
        vcam.Lens.FieldOfView = fov;
    }


    private void MoveCharacter()
    {
        if (!AllowRotation && !AllowMovement)
            return; // 회전/이동 모두 막기

        Vector3 moveDir = GetMovementDir();

        // 캐릭터 회전
        if (AllowRotation && moveDir.sqrMagnitude > 0.01f) // 회전 제어
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            sm.Player.transform.rotation = Quaternion.Slerp(
                sm.Player.transform.rotation,
                targetRot,
                Time.deltaTime * sm.RotationDamping
            );
        }

        // 이동 처리
        if (AllowMovement)
        {
            Vector3 move = Vector3.zero;

            if (sm.Player.Animator.applyRootMotion)
            {
                move = sm.Player.Animator.deltaPosition + sm.Player.ForceReceiver.Movement * Time.deltaTime;
            }
            else
            {
                float moveSpeed = sm.MovementSpeed * sm.MovementSpeedModifier;
                move = moveDir * moveSpeed + sm.Player.ForceReceiver.Movement;
            }

            sm.Player.Controller.Move(move * Time.deltaTime);
        }
    }


    protected Vector3 GetMovementDir()
    {
        Vector3 forward = sm.Player._camera.MainCamera.forward;
        Vector3 right = sm.Player._camera.MainCamera.right;
        
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        
        return forward * sm.MovementInput.y + right * sm.MovementInput.x;
    }


    // ====================== 에니메이션 특정 태그 =============================
    protected float GetNormalizeTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        // 애니메이션 전환(Transition) 중이고, 다음 상태가 지정한 tag면 → 다음 애니메이션의 진행도 반환
        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
            return nextInfo.normalizedTime;
        // 전환 중이 아니고, 현재 상태가 지정한 tag면 → 현재 애니메이션의 진행도 반환
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
            return currentInfo.normalizedTime;
        // 태그랑 맞는 애니메이션이 없다면 0 반환
        else
            return 0f;
    }

    // ===================== 타겟 탐지 (공통) =====================
    /// <summary>
    /// 반경 내 가장 가까운 Enemy 탐색
    /// </summary>
    /// <param name="radius">탐색 반경</param>
    /// <param name="faceTarget">찾은 타겟을 바라볼지 여부</param>
    /// <returns>가장 가까운 Enemy Transform</returns>
    protected Transform FindNearestMonster(float radius, bool faceTarget = false)
    {
        Collider[] hits = Physics.OverlapSphere(
            sm.Player.transform.position,
            radius,
            LayerMask.GetMask("Enemy")
        );
        Transform nearest = null;
        float minDist = float.MaxValue;
        foreach (var hit in hits)
        {
            // 한 단계 위 부모 가져오기
            Transform target = hit.transform.parent ?? hit.transform;

            float dist = Vector3.Distance(
                sm.Player.transform.position,
                target.position
            );

            if (dist < minDist)
            {
                minDist = dist;
                nearest = target;
            }
        }
        if (faceTarget && nearest != null)
        {
            Vector3 dir = (nearest.position - sm.Player.transform.position).normalized;
            dir.y = 0;
            sm.Player.transform.forward = dir;
        }
        return nearest;
    }
}