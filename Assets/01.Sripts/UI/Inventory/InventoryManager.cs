using System.Collections;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class InventoryManager : Singleton<InventoryManager>
{
    private Inventory inventoryModel;
    public InventoryUI inventoryUI;

    // 인벤토리 시스템 최초 초기화시 호출될 함수 (InventoryUI에서 호출)
    public void SetInventory()
    {
        inventoryModel = new Inventory();
        InventoryViewModel inventoryViewModel = new InventoryViewModel(inventoryModel);
        inventoryUI.Setup(inventoryViewModel);
    }

    // 아이템 추가 함수
    public async void LoadTestData_Addressables(string adress)
    {
        // 어드레서블로 아이템 데이터 로드
        AsyncOperationHandle<ItemData> loadHandle = Addressables.LoadAssetAsync<ItemData>(adress);

        // 로딩 완료까지 대기
        await loadHandle.Task;

        if (loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            ItemData loadedItem = loadHandle.Result;
            inventoryModel.AddItem(loadedItem, 1);
        }
        else
        {
            Debug.LogError("Fail");
        }
    }
}