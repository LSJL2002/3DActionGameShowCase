using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unity.Burst.Intrinsics.X86.Avx;

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
