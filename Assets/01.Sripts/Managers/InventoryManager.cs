using System.Collections;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class InventoryManager : Singleton<InventoryManager>
{
    private Inventory inventoryModel;
    private InventoryViewModel inventoryViewModel;
    public CharacterInventoryUI CharacterInventoryUI;
    public CharacterSkillUI CharacterSkillUI;
    public CharacterCoreUI CharacterCoreUI;

    protected override void Start()
    {
        base.Start();

        inventoryModel = new Inventory();
        inventoryViewModel = new InventoryViewModel(inventoryModel);
    }

    // 인벤토리 시스템 최초 초기화시 호출될 함수 (각 UI에서 호출)
    public void SetInventoryUI()
    {
        CharacterInventoryUI.Setup(inventoryViewModel);
    }

    public void SetSkillUI()
    {
        CharacterSkillUI.Setup(inventoryViewModel);
    }

    public void SetCoreUI()
    {
        CharacterCoreUI.Setup(inventoryViewModel);
    }

    // 아이템 추가 함수
    public async void LoadTestData_Addressables(string adress, int stack = default)
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
        // Model에게 아이템 수량 감소를 요청
        inventoryModel.DecreaseItemCount(itemData, 1);
    }
}