using System.Collections;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class InventoryManager : Singleton<InventoryManager>
{
    private Inventory inventoryModel;
    public InventoryViewModel inventoryViewModel;
    public CharacterInfomationUI characterInventoryUI;
    public CharacterSkillUI characterSkillUI;
    public CharacterCoreUI characterCoreUI;

    protected override void Start()
    {
        base.Start();

        inventoryModel = new Inventory();
        inventoryViewModel = new InventoryViewModel(inventoryModel);
    }

    // 인벤토리 시스템 최초 초기화시 호출될 함수 (각 UI에서 호출)
    public void SetInventoryUI()
    {
        characterInventoryUI.Setup(inventoryViewModel);
    }

    public void SetSkillUI()
    {
        characterSkillUI.Setup(inventoryViewModel);
    }

    public void SetCoreUI()
    {
        characterCoreUI.Setup(inventoryViewModel);
    }

    // 아이템 추가 함수
    public async void LoadData_Addressables(string adress, int stack = default)
    {
        // 어드레서블로 아이템 데이터 로드
        AsyncOperationHandle<ItemData> loadHandle = Addressables.LoadAssetAsync<ItemData>(adress);

        // 로딩 완료까지 대기
        await loadHandle.Task;

        if (loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            ItemData loadedItem = loadHandle.Result;
            inventoryModel.AddItem(loadedItem, stack);
        }
        else
        {
            Debug.LogError($"{adress} Fail");
        }
    }

    // 아이템 감소 및 삭제 함수
    public void UseConsumableItem(ItemData itemData)
    {
        // 아이템의 효과리스트를 전부 발동
        for (int i = 0; i < itemData.abilities.Count; i++ )
        {
            itemData.abilities[i].Use(itemData);
        }

        // Model에게 아이템 수량 감소를 요청
        inventoryModel.DecreaseItemCount(itemData, 1);
    }

    // 플레이어 스탯 추가 함수 (능력선택시 호출)
    public void StatUPAbility(ItemData itemData)
    {
        // 플레이어 스탯 증가 함수 호출
        PlayerManager.Instance.Stats.AddModifier(StatType.MaxHealth, itemData.MaxHP);
        PlayerManager.Instance.Stats.AddModifier(StatType.MaxEnergy, itemData.MaxMP);
        PlayerManager.Instance.Stats.AddModifier(StatType.Attack, itemData.Attack);
        PlayerManager.Instance.Stats.AddModifier(StatType.Defense, itemData.Defence);
        PlayerManager.Instance.Stats.AddModifier(StatType.MoveSpeed, itemData.MoveSpeed);
        PlayerManager.Instance.Stats.AddModifier(StatType.AttackSpeed, itemData.AttackSpeed);
    }
}