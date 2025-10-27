using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// GameUI의 Icon Part
public partial class GameUI : UIBase
{
    [Header("[Skill]")]
    [SerializeField] private CanvasGroup skillInfoCanvasGroup;
    [SerializeField] private Image evadeImage; // 회피 이미지
    [SerializeField] private TextMeshProUGUI evadeText; // 회피 스택 텍스트
    [SerializeField] private Image skill1Image; // 스킬1 이미지
    [SerializeField] private TextMeshProUGUI skill1Text; // 스킬1 스택 텍스트
    [SerializeField] private GameObject item1GO; // 소비템1 오브젝트
    [SerializeField] private Image item1Image; // 소비템1 이미지
    [SerializeField] private TextMeshProUGUI item1Text; // 소비템1 스택 텍스트
    [SerializeField] private GameObject item2GO; // 소비템2 오브젝트
    [SerializeField] private Image item2Image; // 소비템2 이미지
    [SerializeField] private TextMeshProUGUI item2Text; // 소비템2 스택 텍스트
    [SerializeField] private GameObject item3GO; // 소비템3 오브젝트
    [SerializeField] private Image item3Image; // 소비템3 이미지
    [SerializeField] private TextMeshProUGUI item3Text; // 소비템3 스택 텍스트

    private Sequence evadeCoolTimeSequence = null;
    private Sequence skillCoolTimeSequence = null;
    private Sequence item1CoolTimeSequence = null;
    private Sequence item2CoolTimeSequence = null;
    private Sequence item3CoolTimeSequence = null;

    private InventoryItem inventoryItemData1 = null;
    private InventoryItem inventoryItemData2 = null;
    private InventoryItem inventoryItemData3 = null;

    private float item1CooltimeDuration = 0;
    private float item2CooltimeDuration = 0;
    private float item3CooltimeDuration = 0;

    public void OnEnableIcon()
    {
        skillInfoCanvasGroup.DOFade(0f, 0f).OnComplete(() => { skillInfoCanvasGroup.DOFade(1f, 1f); });

        PlayerCharacter activeCharacter = PlayerManager.Instance.ActiveCharacter;
        UpdateIconUI(activeCharacter);

        // 이벤트 구독 / 해제
        PlayerManager.Instance.OnActiveCharacterChanged -= UpdateIconUI;
        EventsManager.Instance.StopListening(GameEvent.OnConsumableItemsChanged, SetItemIcon);
        EventsManager.Instance.StopListening(GameEvent.OnUsedItem, SetItemIcon);
        EventsManager.Instance.StopListening(GameEvent.OnUsedItem, ItemCooltimeStart);

        PlayerManager.Instance.OnActiveCharacterChanged += UpdateIconUI;
        EventsManager.Instance.StartListening(GameEvent.OnConsumableItemsChanged, SetItemIcon);
        EventsManager.Instance.StartListening(GameEvent.OnUsedItem, SetItemIcon);
        EventsManager.Instance.StartListening(GameEvent.OnUsedItem, ItemCooltimeStart);

        float evadeCooltimeDuration = PlayerManager.Instance.Attr.EvadeBuffer.Cooldown; // 스킬 Max 쿨타임 가져옴 (초단위)
        float skillCooltimeDuration = PlayerManager.Instance.Attr.SkillBuffer.Cooldown; // 스킬 Max 쿨타임 가져옴 (초단위)

        // 회피 쿨타임 시퀀스 초기 생성
        evadeCoolTimeSequence = DOTween.Sequence()
            .SetAutoKill(false)
            .Append(evadeImage.DOFillAmount(0f, evadeCooltimeDuration))
            .AppendCallback(() => { evadeImage.fillAmount = 1f; })
            .Pause();

        // 스킬 쿨타임 시퀀스 초기 생성
        skillCoolTimeSequence = DOTween.Sequence()
            .SetAutoKill(false)
            .Append(skill1Image.DOFillAmount(0f, skillCooltimeDuration))
            .AppendCallback(() => { skill1Image.fillAmount = 1f; })
            .Pause();

        SetItemIcon();
    }

    // 아이템 소유정보를 인벤토리로부터 가져와서 세팅하는 함수
    public void SetItemIcon()
    {
        // 인벤토리 매니저로부터 아이템 리스트를 받아옴 (읽기전용)
        IReadOnlyList<InventoryItem> inventoryItems = InventoryManager.Instance?.GetConsumableItems();

        // LINQ의 FirstOrDefault()를 사용하여 조건에 맞는 InventoryItem을 찾기
        // InventoryItem.ItemData.id가 targetId와 일치하는 항목을 찾기
        inventoryItemData1 = inventoryItems?.FirstOrDefault(item => item.data != null && item.data.id == 20000000);
        inventoryItemData2 = inventoryItems?.FirstOrDefault(item => item.data != null && item.data.id == 20000002);
        inventoryItemData3 = inventoryItems?.FirstOrDefault(item => item.data != null && item.data.id == 20000003);

        // 찾은 아이템이 인벤토리에 존재하는지 확인
        if (inventoryItemData1 != null)
        {
            item1CooltimeDuration = inventoryItemData1.data.duration;
            item1Text.text = inventoryItemData1.stackCount.ToString();
        }
        else item1Text.text = "0";

        if (inventoryItemData2 != null)
        {
            item2CooltimeDuration = inventoryItemData2.data.duration;
            item2Text.text = inventoryItemData2.stackCount.ToString();
        }
        else item2Text.text = "0";

        if (inventoryItemData3 != null)
        {
            item3CooltimeDuration = inventoryItemData3.data.duration;
            item3Text.text = inventoryItemData3.stackCount.ToString();
        }
        else item3Text.text = "0";
    }

    // 아이템 사용시 쿨타임 연출하는 함수
    public void ItemCooltimeStart()
    {
        if (inventoryItemData1 != default)
        {
            // 스킬 쿨타임 시퀀스 초기 생성
            item1CoolTimeSequence = DOTween.Sequence();
            item1CoolTimeSequence.Append(item1Image.DOFillAmount(0f, item1CooltimeDuration));
            item1CoolTimeSequence.AppendCallback(() => { item1Image.fillAmount = 1f; });
            item1CoolTimeSequence.SetAutoKill(false);
        }

        if (inventoryItemData2 != default)
        {
            // 스킬 쿨타임 시퀀스 초기 생성
            item2CoolTimeSequence = DOTween.Sequence();
            item2CoolTimeSequence.Append(item2Image.DOFillAmount(0f, item2CooltimeDuration));
            item2CoolTimeSequence.AppendCallback(() => { item2Image.fillAmount = 1f; });
            item2CoolTimeSequence.SetAutoKill(false);
        }

        if (inventoryItemData3 != default)
        {
            // 스킬 쿨타임 시퀀스 초기 생성
            item3CoolTimeSequence = DOTween.Sequence();
            item3CoolTimeSequence.Append(item3Image.DOFillAmount(0f, item3CooltimeDuration));
            item3CoolTimeSequence.AppendCallback(() => { item3Image.fillAmount = 1f; });
            item3CoolTimeSequence.SetAutoKill(false);
        }
    }

    public void UpdateIcon()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryItemData1 != null)
        {
            InventoryManager.Instance.UseConsumableItem(inventoryItemData1.data);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryItemData2 != null)
        {
            InventoryManager.Instance.UseConsumableItem(inventoryItemData2.data);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && inventoryItemData3 != null)
        {
            InventoryManager.Instance.UseConsumableItem(inventoryItemData3.data);
        }
    }

    public void OnDisableIcon()
    {
        DOTween.Kill(this);
        evadeCoolTimeSequence = null;
        skillCoolTimeSequence = null;
    }

    // 구독갱신
    public void ResetEventIcon()
    {
        // 구독해제
        PlayerManager.Instance.Attr.EvadeBuffer.OnBufferChanged -= OnCoolTimeEvade;
        PlayerManager.Instance.Attr.SkillBuffer.OnBufferChanged -= OnCoolTimeSkill;

        // 구독
        PlayerManager.Instance.Attr.EvadeBuffer.OnBufferChanged += OnCoolTimeEvade;
        PlayerManager.Instance.Attr.SkillBuffer.OnBufferChanged += OnCoolTimeSkill;
    }

    // SkillUI 업데이트시 호출 (최초, 플레이어 교체시)
    public void UpdateIconUI(PlayerCharacter playerCharacter)
    {
        if (evadeCoolTimeSequence.IsActive())
            evadeCoolTimeSequence.Rewind(); // 처음으로 되감기 + Pause
        if (skillCoolTimeSequence.IsActive())
            skillCoolTimeSequence.Rewind(); // 처음으로 되감기 + Pause

        evadeImage.fillAmount = 1;
        evadeText.text = PlayerManager.Instance.Attr.EvadeBuffer.BufferCurrent.ToString();

        skill1Image.fillAmount = 1;
        skill1Text.text = PlayerManager.Instance.Attr.SkillBuffer.BufferCurrent.ToString();

        ResetEventIcon();
    }

    // 회피 쿨타임
    public void OnCoolTimeEvade()
    {
        BufferModule evadeBuffer = PlayerManager.Instance.Attr.EvadeBuffer;
        evadeText.text = evadeBuffer.BufferCurrent.ToString(); // 스택을 업데이트

        if (evadeBuffer.BufferCurrent == evadeBuffer.BufferMax || evadeCoolTimeSequence.IsPlaying())
            return;

        evadeImage.fillAmount = 1f; // 시작 전 1f로 초기화
        evadeCoolTimeSequence.Restart();
    }

    // 스킬 쿨타임
    public void OnCoolTimeSkill()
    {
        BufferModule skillBuffer = PlayerManager.Instance.Attr.SkillBuffer;
        skill1Text.text = skillBuffer.BufferCurrent.ToString(); // 스택을 업데이트

        if (skillBuffer.BufferCurrent == skillBuffer.BufferMax || skillCoolTimeSequence.IsPlaying())
            return;

        skill1Image.fillAmount = 1f; // 시작 전 1f로 초기화
        skillCoolTimeSequence.Restart();
    }
}
