using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ItemDescriptionUI : UIBase
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemTypeText;
    public TextMeshProUGUI descriptionText;

    public void SetItemSlotData(ItemSlotUI itemSlotUI)
    {
        itemNameText.text = itemSlotUI.itemName;
        itemTypeText.text = itemSlotUI.itemType;
        descriptionText.text = itemSlotUI.itemDescription;
    }
}
