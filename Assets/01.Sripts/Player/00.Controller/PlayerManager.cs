using System;
using System.Linq;
using UnityEngine;

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
    public int AliveCount => characters.Count(c => !c.Ability.IsDeath);

    // ===================== 외부 API =========================
    public PlayerCharacter[] Characters => characters;
    public int CurrentIndex { get => currentIndex; set => currentIndex = value; }
    public CameraManager Camera => _camera;

    // ==================== Managers =====================
    public CameraManager _camera;
    public SkillManagers skill;
    public EventManager _event;
    public VFXManager vFX;
    public HitStopManager hitStop;
    public DirectionManager direction;

    // ==================== Actions =======================
    public event Action<PlayerCharacter> OnActiveCharacterChanged; // 캐릭터 Swap
    public event Action OnAllCharactersDead;                       // 캐릭터 All Death

    private void Awake()
    {
        Application.targetFrameRate = 120;
        InitializeCharacters();
    }

    void Start()
    {
        ActivateFirstCharacter();
    }

    // =================== Injection + Initialization =====================
    private void InitializeCharacters()
    {
        foreach (var character in characters)
        {
            character.gameObject.SetActive(true);
            character.InitManagers(this, _camera, skill, _event, vFX, hitStop, direction);
        }
    }

    private void ActivateFirstCharacter()
    {
        for (int i = 0; i < characters.Length; i++)
            characters[i].gameObject.SetActive(i == currentIndex);

        _camera?.SetPlayerTarget(ActiveCharacter.transform, ActiveCharacter.Face);
        ActiveCharacter.StateMachine.ChangeState(ActiveCharacter.StateMachine.IdleState);

        //EnableInput(true); // letterbox 연출
    }

    // ================ 플레이어 입력 제어 ====================
    public void EnableInput(bool active)
    {
        ActiveCharacter?.EnableCharacterInput(active);
        _camera?.SetCameraInputEnabled(active);
        Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // ================ 플레이어 스왑 기능 ====================
    public void SwapNext()
    {
        var next = GetNextAliveCharacter();
        if (next != null)
            PlayerSwapService.Swap(this, Array.IndexOf(characters, next));
    }

    public void SwapPrev()
    {
        var prev = GetPrevAliveCharacter();
        if (prev != null)
            PlayerSwapService.Swap(this, Array.IndexOf(characters, prev));
    }

    public PlayerCharacter GetNextAliveCharacter()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            int idx = (currentIndex + 1 + i) % characters.Length;
            if (!characters[idx].Ability.IsDeath)
                return characters[idx];
        }
        return null;
    }
    public PlayerCharacter GetPrevAliveCharacter()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            int idx = (currentIndex - 1 - i + characters.Length) % characters.Length;
            if (!characters[idx].Ability.IsDeath)
                return characters[idx];
        }
        return null;
    }
    public void NotifyActiveCharacterChanged(PlayerCharacter newChar)
    {
        OnActiveCharacterChanged?.Invoke(newChar);
    }

    // ==================== 플레이어 상태 체크 =======================
    public void HandleCharacterDeath(PlayerCharacter deadCharacter)
    {
        var next = GetNextAliveCharacter();
        if (next != null)
        {
            PlayerSwapService.Swap(this, Array.IndexOf(characters, next));
        }

        CheckAllCharactersDead();
    }

    public void CheckAllCharactersDead()
    {
        // 모든 캐릭터가 죽었는지 확인
        if (characters.All(c => c.Ability.IsDeath))
        {
            OnAllCharactersDead?.Invoke();
        }
    }

    // ============== 부활 ============
    public void ReviveCharacter(PlayerCharacter character)
    {
        character.Revive();
    }
    public void ReviveAll()
    {
        foreach (var character in characters)
            character.Revive();

        currentIndex = 0;
        PlayerSwapService.Swap(this, 0);
    }
}