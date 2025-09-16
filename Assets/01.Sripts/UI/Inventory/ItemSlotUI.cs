using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 아이템 슬롯 하나하나의 상태를 시각적으로 보여주는 View 계층의 클래스 (아이콘, 수량숫자 넣고 빼기 등)
public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;

    public void SetData(ItemData data, int count)
    {
        iconImage.sprite = data.itemIcon;
        countText.text = count.ToString();
    }

    public void ClearSlot()
    {
        iconImage.sprite = null;
        countText.text = null;
    }
}