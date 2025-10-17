using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public PlayerCharacter ActiveCharacter => characters[currentIndex];
    public PlayerStateMachine StateMachine => ActiveCharacter?.StateMachine;
    public PlayerAttribute Stats => ActiveCharacter?.Stats;
    public PlayerController Input => ActiveCharacter?.Input;

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
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 120;

        // 각 캐릭터에 매니저 연결
        foreach (var character in characters)
        {
            character.InitManagers(
                        playerManager: this,
                        cam: _camera,
                        skl: skill,
                        evt: _event,
                        vfx: vFX,
                        hit: hitStop,
                        dir: direction
                    );
        }
    }

    void Start()
    {
        // 모든 캐릭터 활성화 상태 초기화
        for (int i = 0; i < characters.Length; i++)
            characters[i].gameObject.SetActive(i == currentIndex);

        // 시작 캐릭터 Idle 상태
        var active = ActiveCharacter;
        active.StateMachine.ChangeState(active.StateMachine.IdleState);

        // 카메라 타겟 초기화
        _camera?.SetPlayerTarget(active.transform, active.Face);
    }

    // ================ 플레이어 입력 제어 ====================
    public void EnableInput(bool active)
    {
        ActiveCharacter?.EnableCharacterInput(active);
        _camera?.SetCameraInputEnabled(active);
        Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.None;
    }

    // ================ 플레이어 스왑 기능 ====================
    private void SwapTo(int index)
    {
        if (index == currentIndex || index < 0 || index >= characters.Length) return;

        var oldChar = ActiveCharacter;
        var newChar = characters[index];

        // 기존 캐릭터 입력 비활성화
        oldChar.EnableCharacterInput(false);

        // 방향 동기화 (예: 오른쪽/왼쪽 바라보는 방향)
        Vector3 spawnOffset = oldChar.transform.right * 0.8f;
        newChar.transform.position = oldChar.transform.position + spawnOffset;
        newChar.transform.rotation = oldChar.transform.rotation;

        // 새 캐릭터 즉시 등장 + 카메라 전환
        newChar.gameObject.SetActive(true);
        newChar.EnableCharacterInput(true);
        _camera?.SetPlayerTarget(newChar.transform, newChar.Face);

        // 새 캐릭터 SwapInState 진입
        newChar.StateMachine.ChangeState(newChar.StateMachine.SwapInState);

        // SwapOut 애니메이션 재생 후 기존 캐릭터 비활성화
        void OnOldSwapOutComplete(PlayerBaseState state)
        {
            oldChar.StateMachine.SwapOutState.OnStateComplete -= OnOldSwapOutComplete;
            oldChar.gameObject.SetActive(false);
        }

        oldChar.StateMachine.SwapOutState.OnStateComplete += OnOldSwapOutComplete;
        oldChar.StateMachine.ChangeState(oldChar.StateMachine.SwapOutState);

        // 인덱스 갱신
        currentIndex = index;

        OnActiveCharacterChanged?.Invoke(newChar);
    }

    public void SwapNext() => SwapTo((currentIndex + 1) % characters.Length);
    public void SwapPrev() => SwapTo((currentIndex - 1 + characters.Length) % characters.Length);
}