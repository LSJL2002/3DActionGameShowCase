using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public interface IPlayerManager
{
}

public class PlayerManager : Singleton<PlayerManager>, IPlayerManager
{
    [SerializeField] private PlayerCharacter[] characters; // 등록된 캐릭터들
    [SerializeField] private int currentIndex = 0;

    // ===================== Proxy =====================
    public PlayerCharacter ActiveCharacter =>
        (characters != null && currentIndex >= 0 && currentIndex < characters.Length)
            ? characters[currentIndex]
            : null;
    public PlayerStateMachine StateMachine => ActiveCharacter?.StateMachine;
    public AbilitySystem Ability => ActiveCharacter?.Ability;
    public PlayerAttribute Attr => ActiveCharacter?.Attr;
    public InputSystem Input => ActiveCharacter?.Input;

    // ==================== Managers =====================
    public CameraManager _camera;
    public SkillManagers skill;
    public EventManager _event;
    public VFXManager vFX;
    public HitStopManager hitStop;
    public DirectionManager direction;

    // ==================== Actions =======================
    public event Action<PlayerCharacter> OnActiveCharacterChanged;

    private void Awake()
    {
        Application.targetFrameRate = 120;

        InitializeCharacters();
    }

    void Start()
    {
        ActivateFirstCharacter();
    }

    // =================== Player Data Driven =====================
    private void InitializeCharacters()
    {
        foreach (var character in characters)
        {
            character.InitManagers(this, _camera, skill, _event, vFX, hitStop, direction);
        }
    }

    private void ActivateFirstCharacter()
    {
        // 모든 캐릭터 활성화 상태 초기화
        for (int i = 0; i < characters.Length; i++)
            characters[i].gameObject.SetActive(i == currentIndex);

        var active = ActiveCharacter;
        // 카메라 타겟 초기화 & 첫 시작시 초기화용 이후 상태변환 책임은 AbilitySystem에게 위임함
        _camera?.SetPlayerTarget(active.transform, active.Face);
        StateMachine.ChangeState(StateMachine.IdleState);
        EnableInput(true);
    }

    // ================ 플레이어 입력 제어 ====================
    public void EnableInput(bool active)
    {
        ActiveCharacter?.EnableCharacterInput(active);
        _camera?.SetCameraInputEnabled(active);
        Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.None;
    }

    // ================ 플레이어 스왑 기능 ====================
    private void SwapTo(int newIndex)
    {
        if (newIndex == currentIndex) return;

        var oldChar = ActiveCharacter;
        var newChar = characters[newIndex];

        // 위치/활성화만 담당 (논리적 전환은 FSM 담당)
        PlayerSwapService.PrepareCharacterSwap(oldChar, newChar);

        newChar.Ability.StartSwapIn();
        _camera?.SetPlayerTarget(newChar.transform, newChar.Face);

        currentIndex = newIndex;
        OnActiveCharacterChanged?.Invoke(newChar);
    }

    public void SwapNext() => SwapTo((currentIndex + 1) % characters.Length);
    public void SwapPrev() => SwapTo((currentIndex - 1 + characters.Length) % characters.Length);
}