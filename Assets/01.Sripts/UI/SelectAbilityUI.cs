using System;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class SelectAbilityUI : UIBase
{
    public Image statImage;
    public Image skillImage;
    public Image coreImage;

    public TextMeshProUGUI statDescriptionText;
    public TextMeshProUGUI skillDescriptionText;
    public TextMeshProUGUI coreDescriptionText;

    // 배틀매니저가 가지고 있는 아이템을 저장할 변수
    private string skillItemAdress;
    private string coreItemAdress;
    private float statPoint_MaxHP;
    private float statPoint_MaxMP;
    private int statPoint_ATK;
    private int statPoint_DEF;
    private float statPoint_AttackSPD;
    private float statPoint_MoveSPD;

    // AsyncOperationHandle을 저장할 변수 추가
    private AsyncOperationHandle<ItemData> skillLoadHandle;
    private AsyncOperationHandle<ItemData> coreLoadHandle;

    protected override void OnEnable()
    {
        base.OnEnable();

        // 마우스 커서 보이게
        PlayerManager.Instance.EnableInput(false);

        BattleManager battleManager = BattleManager.Instance;

        // 변수 초기화
        skillItemAdress = battleManager.GetItemInfo(0); // 0번째정보 : 스킬아이템ID
        coreItemAdress = battleManager.GetItemInfo(1); // 1번째정보 : 코어아이템ID
        statPoint_MaxHP = battleManager.currentZone.extraHP;
        statPoint_MaxMP = battleManager.currentZone.extraMP;
        statPoint_ATK = battleManager.currentZone.extraATK;
        statPoint_DEF = battleManager.currentZone.extraDEF;
        statPoint_AttackSPD = battleManager.currentZone.extraAtkSpeed;
        statPoint_MoveSPD = battleManager.currentZone.extraSpeed;

        statDescriptionText.text = $"최대체력 : {statPoint_MaxHP}\n최대마력 : {statPoint_MaxMP}\n공격력 : {statPoint_ATK}\n방어력 : {statPoint_DEF}\n공격속도 : {statPoint_AttackSPD}\n이동속도 : {statPoint_MoveSPD}만큼 증가합니다.";
        GetItemInfo(skillItemAdress, "Skill"); // 스킬 아이템 정보 가져오고 UI에 세팅
        GetItemInfo(coreItemAdress, "Core");   // 코어 아이템 정보 가져오고 UI에 세팅
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        // 로드했던 데이터 해제
        ReleaseHandles();
    }

    public async void GetItemInfo(string adress, string type)
    {
        // 타입 확인 / 이미 핸들이 유효한지 확인
        if (type == "Skill" && skillLoadHandle.IsValid())
        {
            Addressables.Release(skillLoadHandle);
        }
        else if (type == "Core" && coreLoadHandle.IsValid())
        {
            Addressables.Release(coreLoadHandle);
        }

        // 어드레서블로 아이템 데이터 로드
        AsyncOperationHandle<ItemData> loadHandle = Addressables.LoadAssetAsync<ItemData>(adress);

        if (type == "Skill")
        {
            skillLoadHandle = loadHandle;
        }
        else if (type == "Core")
        {
            coreLoadHandle = loadHandle;
        }

        // 로딩 완료까지 대기
        await loadHandle.Task;

        // 로딩이 완료된 시점에 핸들 유효성 확인
        if (loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            ItemData loadedItem = loadHandle.Result;

            if (type == "Skill")
            {
                skillDescriptionText.text = loadedItem.scriptText;
                skillImage.sprite = loadedItem.sprite;
            }
            else if (type == "Core")
            {
                coreDescriptionText.text = loadedItem.scriptText;
                coreImage.sprite = loadedItem.sprite;
            }
        }
        else
        {
            Debug.LogError($"{adress} Fail");
        }
    }

    // 모든 핸들을 해제하는 함수
    private void ReleaseHandles()
    {
        if (skillLoadHandle.IsValid())
        {
            Addressables.Release(skillLoadHandle);
            Debug.Log("스킬 데이터 해제 완료");
        }
        if (coreLoadHandle.IsValid())
        {
            Addressables.Release(coreLoadHandle);
            Debug.Log("코어 데이터 해제 완료");
        }
    }

    public void OnClickButton(string str)
    {
        InventoryManager.Instance.LoadData_Addressables("20000000");
        Debug.Log("회복약 획득");

        switch (str)
        {
            case "Stat":
                // 플레이어 스탯 증가 함수 호출
                PlayerManager.Instance.Stats.AddModifier(StatType.MaxHealth, statPoint_MaxHP);
                PlayerManager.Instance.Stats.AddModifier(StatType.MaxEnergy, statPoint_MaxMP);
                PlayerManager.Instance.Stats.AddModifier(StatType.Attack, statPoint_ATK);
                PlayerManager.Instance.Stats.AddModifier(StatType.Defense, statPoint_DEF);
                PlayerManager.Instance.Stats.AddModifier(StatType.MoveSpeed, statPoint_AttackSPD);
                PlayerManager.Instance.Stats.AddModifier(StatType.AttackSpeed, statPoint_MoveSPD);

                Debug.Log($"플레이어 스탯증가");
                break;

            case "Skill":
                // 아이템을 인벤토리에 추가
                InventoryManager.Instance.LoadData_Addressables(skillItemAdress);
                Debug.Log($"{skillItemAdress} 획득");
                break;

            case "Core":
                // 아이템을 인벤토리에 추가
                InventoryManager.Instance.LoadData_Addressables(coreItemAdress);
                Debug.Log($"{coreItemAdress} 획득");

                break;

        }
        BattleManager.Instance.ClearBattle();

        // 마우스 커서 다시 락
        PlayerManager.Instance.EnableInput(true);
        Hide();
    }
}
