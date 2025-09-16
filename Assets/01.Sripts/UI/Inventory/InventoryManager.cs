// 파일명: InventoryManager.cs
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] private InventoryUI inventoryUI;

    private Inventory inventoryModel;

    protected override void Awake()
    {
        base.Awake();

        inventoryModel = new Inventory();
        InventoryViewModel inventoryViewModel = new InventoryViewModel(inventoryModel);
        inventoryUI.Setup(inventoryViewModel);

        
    }

    protected override void Start()
    {
        base.Start();

        LoadTestData(); // 아이템 추가 테스트용
    }

    // 아이템 추가 테스트용
    private async void LoadTestData()
    {
        // 어드레서블로 아이템 데이터 로드
        AsyncOperationHandle<ItemData> loadHandle = Addressables.LoadAssetAsync<ItemData>("20000000");

        // 로딩 완료까지 대기
        await loadHandle.Task;

        if (loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            ItemData loadedItem = loadHandle.Result;
            inventoryModel.AddItem(loadedItem, 1);
        }
        else
        {
            Debug.LogError("Failed to load ItemData");
        }
    }
}