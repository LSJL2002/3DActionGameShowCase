using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;


public abstract class PlayerBaseState : Istate
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    // 상태별 행동 훅
    public virtual bool AllowRotation => true;
    public virtual bool AllowMovement => true;


    public virtual void Enter() => AddInputActionCallbacks();
    public virtual void Exit() => RemoveInputActionCallbacks();
    protected void StartAnimation(int animatorHash) => stateMachine.Player.Animator.SetBool(animatorHash, true);
    protected void StopAnimation(int animatorHash) => stateMachine.Player.Animator.SetBool(animatorHash, false);


    protected virtual void AddInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.PlayerActions.Move.canceled += OnMoveCanceled;
        input.PlayerActions.Dodge.started += OnDodgeStarted;
        input.PlayerActions.Attack.started += OnAttackStarted;
        input.PlayerActions.Attack.canceled += OnAttackCanceled;
        input.PlayerActions.HeavyAttack.started += OnHeavyAttackStarted;
        input.PlayerActions.HeavyAttack.canceled += OnHeavyAttackCanceled;

        input.PlayerActions.Menu.performed += OnMenuToggle;
        input.PlayerActions.Camera.started += OnLockOnToggle;
        input.PlayerActions.Inventory.started += OnInvenToggle;
    }

    protected virtual void RemoveInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.PlayerActions.Move.canceled -= OnMoveCanceled;
        input.PlayerActions.Dodge.started -= OnDodgeStarted;
        input.PlayerActions.Attack.started -= OnAttackStarted;
        input.PlayerActions.Attack.canceled -= OnAttackCanceled;
        input.PlayerActions.HeavyAttack.started -= OnHeavyAttackStarted;
        input.PlayerActions.HeavyAttack.canceled -= OnHeavyAttackCanceled;

        input.PlayerActions.Menu.performed -= OnMenuToggle;
        input.PlayerActions.Camera.started -= OnLockOnToggle;
        input.PlayerActions.Inventory.started -= OnInvenToggle;
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
        ReadZoomInput(); // 줌 값 읽기 추가

        // Animator에 입력값 전달 (공통 처리)
        stateMachine.Player.Animator.SetFloat(
            stateMachine.Player.AnimationData.HorizontalHash,
            stateMachine.MovementInput.x
        );
        stateMachine.Player.Animator.SetFloat(
            stateMachine.Player.AnimationData.VerticalHash,
            stateMachine.MovementInput.y
        );
    }
    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() => MoveCharacter();


    protected virtual void OnMoveCanceled(InputAction.CallbackContext context) { }
    protected virtual void OnDodgeStarted(InputAction.CallbackContext context) { }
    protected virtual void OnAttackStarted(InputAction.CallbackContext context)
        => stateMachine.IsAttacking = true;
    protected virtual void OnAttackCanceled(InputAction.CallbackContext context)
        => stateMachine.IsAttacking = false;
    protected virtual void OnHeavyAttackStarted(InputAction.CallbackContext context) { }
    protected virtual void OnHeavyAttackCanceled(InputAction.CallbackContext context) { }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context) { }

    protected virtual void OnLockOnToggle(InputAction.CallbackContext context)
    {
        var cam = stateMachine.Player.camera;

        if (cam.HasTarget())
        {
            cam.ToggleLockOnTarget(null); // 해제
        }
        else
        {
            // 가까운 몬스터 자동 탐색 후 락온
            //var nearest = FindNearestMonster(stateMachine.Player.InfoData.AttackData.AttackRange);
            //cam.SetLockOnTarget(nearest);
        }
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
            stateMachine.Player.camera.Volume.enabled = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            stateMachine.Player.camera.Volume.enabled = false;
        }
    }

    protected virtual void OnInvenToggle(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (stateMachine.Player.direction.inventory == null) return;

        var inven = stateMachine.Player.direction.inventory;
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
        stateMachine.MovementInput =
            stateMachine.Player.Input.PlayerActions.Move.ReadValue<Vector2>();
    }

    private void ReadZoomInput()
    {
        float zoomDelta =
            stateMachine.Player.Input.PlayerActions.Zoom.ReadValue<float>();

        if (Mathf.Abs(zoomDelta) > 0.01f)
            OnZoom(zoomDelta);
    }

    // ========== Zoom 처리 ==========
    private void OnZoom(float zoomDelta)
    {
        var vcam = stateMachine.Player.camera.FreeLookCam;
        if (vcam == null) return;

        float fov = vcam.m_Lens.FieldOfView;
        float zoomSpeed = 3f;
        float zoomAmount = Mathf.Sign(zoomDelta) * zoomSpeed; // 방향만 가져오기
        fov -= zoomAmount; 
        fov = Mathf.Clamp(fov, 10f, 70f);
        vcam.m_Lens.FieldOfView = fov;
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
            stateMachine.Player.transform.rotation = Quaternion.Slerp(
                stateMachine.Player.transform.rotation,
                targetRot,
                Time.deltaTime * stateMachine.RotationDamping
            );
        }

        // 이동 처리
        if (AllowMovement)
        {
            Vector3 move = Vector3.zero;

            if (stateMachine.Player.Animator.applyRootMotion)
            {
                move = stateMachine.Player.Animator.deltaPosition + stateMachine.Player.ForceReceiver.Movement * Time.deltaTime;
            }
            else
            {
                float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
                move = moveDir * moveSpeed + stateMachine.Player.ForceReceiver.Movement;
            }

            stateMachine.Player.Controller.Move(move * Time.deltaTime);
        }
    }


    protected Vector3 GetMovementDir()
    {
        Vector3 forward = stateMachine.Player.camera.MainCamera.forward;
        Vector3 right = stateMachine.Player.camera.MainCamera.right;
        
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        
        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }


    // ==========ForceReceiver에 쌓인 힘을 실제 캐릭터에 적용하는 역할===============
    protected void ForceMove()
    {
        stateMachine.Player.Controller.Move(stateMachine.Player.ForceReceiver.Movement * Time.deltaTime);
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
            stateMachine.Player.transform.position,
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
                stateMachine.Player.transform.position,
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
            Vector3 dir = (nearest.position - stateMachine.Player.transform.position).normalized;
            dir.y = 0;
            stateMachine.Player.transform.forward = dir;
        }
        return nearest;
    }
}