using System;
using UnityEngine;
using UnityEngine.InputSystem;

// 입력 래퍼(wrapper, 게이트웨이), 초기화, PlayerActions 노출
// 상태/로직(AbilitySystem)이 입력구조(PlayerInputs)에 전혀 의존하지 않음

// UI, AI, 리플레이 시스템 같은 데서도 가짜 입력 이벤트만 쏴주면 재활용 가능
// InputSystem만 교체하면 전체 입력 로직 변경 가능 (패드/키보드/모바일 전환 쉬움)
public class InputSystem
{
    private PlayerInputs _playerInputs;
    public PlayerInputs.PlayerActions PlayerActions { get; private set; }

    public event Action OnAttackStarted;
    public event Action OnAttackCanceled;
    public event Action OnDodge;
    public event Action OnJump;
    public event Action OnSkillStarted;
    public event Action OnSkillCanceled;

    public event Action<Vector2> OnMove;     // 이동 입력 이벤트
    public event Action<float> OnZoom;       // 줌 입력 이벤트
    public event Action OnMenuToggle;
    public event Action OnInventoryToggle;
    public event Action OnCameraLockOn;
    public event Action OnSwapNext;
    public event Action OnSwapPrev;
    public Vector2 MoveInput { get; private set; }


    public InputSystem()
    {
        _playerInputs = new PlayerInputs();
        PlayerActions = _playerInputs.Player;

        // ====== 입력 바인딩 ======
        PlayerActions.Attack.started += ctx => OnAttackStarted?.Invoke();
        PlayerActions.Attack.canceled += ctx => OnAttackCanceled?.Invoke();
        PlayerActions.Dodge.started += ctx => OnDodge?.Invoke();
        PlayerActions.Jump.started += ctx => OnJump?.Invoke();
        PlayerActions.HeavyAttack.started += ctx => OnSkillStarted?.Invoke();
        PlayerActions.HeavyAttack.canceled += ctx => OnSkillCanceled?.Invoke();

        PlayerActions.Move.performed += ctx =>
        {
            MoveInput = ctx.ReadValue<Vector2>();
            OnMove?.Invoke(MoveInput);
        };
        PlayerActions.Move.canceled += _ =>
        {
            MoveInput = Vector2.zero;
            OnMove?.Invoke(MoveInput);
        };

        PlayerActions.Zoom.performed += ctx =>
        {
            float zoomDelta = ctx.ReadValue<float>();
            if (Mathf.Abs(zoomDelta) > 0.01f)
                OnZoom?.Invoke(zoomDelta);
        };

        PlayerActions.SwapNext.started += _ => OnSwapNext?.Invoke();
        PlayerActions.SwapPrev.started += _ => OnSwapPrev?.Invoke();

        PlayerActions.Menu.started += _ => OnMenuToggle?.Invoke();
        PlayerActions.Inventory.started += _ => OnInventoryToggle?.Invoke();
        PlayerActions.Camera.started += _ => OnCameraLockOn?.Invoke();
    }

    public void Enable() => _playerInputs.Enable();
    public void Disable() => _playerInputs.Disable();
}