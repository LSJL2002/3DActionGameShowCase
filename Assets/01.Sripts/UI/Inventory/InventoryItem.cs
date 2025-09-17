using UnityEngine;

[System.Serializable]
public class InventoryItem // 인벤토리의 아이템 '하나'에 대한 정보를 가진 클래스 (Model 계층)
{
    public ItemData data; // 아이템의 템플릿 정보
    public int stackCount; // 현재 보유한 아이템 개수

    public InventoryItem(ItemData itemData, int count)
    {
        data = itemData;
        stackCount = count;
    }
}