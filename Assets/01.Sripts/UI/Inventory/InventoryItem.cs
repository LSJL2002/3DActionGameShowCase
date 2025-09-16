[System.Serializable]
public class InventoryItem // 아이템의 수량 등 '가변'적인 정보를 담고있는 클래스 (Model 계층)
{
    public ItemData data; // 아이템의 템플릿 정보
    public int stackCount; // 현재 보유한 아이템 개수

    public InventoryItem(ItemData itemData, int count)
    {
        data = itemData;
        stackCount = count;
    }
}