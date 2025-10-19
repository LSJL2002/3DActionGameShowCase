using TMPro;
using UnityEngine;

public class ItemInformationUI : UIBase
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemTypeText;
    public TextMeshProUGUI descriptionText;

    protected override void OnEnable()
    {
        base.OnEnable();

        // 최상위 캔버스의 sortting order 값을 항상 최상위로 설정
        Canvas canvas = GetComponentInParent<Canvas>();
        canvas.sortingOrder = 100;
    }

    public void SetItemSlotData(ItemSlotUI itemSlotUI)
    {
        itemNameText.text = itemSlotUI.itemName;
        itemTypeText.text = itemSlotUI.itemType;
        descriptionText.text = itemSlotUI.itemDescription;
    }
}
