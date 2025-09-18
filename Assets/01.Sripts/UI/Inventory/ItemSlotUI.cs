using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 아이템 슬롯 하나하나의 상태를 시각적으로 보여주는 View 계층의 클래스 (아이콘, 수량숫자 넣고 빼기 등)
public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Outline outline;
    private ItemDescriptionUI itemDescriptionUI;

    public void SetData(ItemData data, int count)
    {
        iconImage.sprite = data.itemIcon;

        if (count > 0)
        {
            countText.text = count.ToString();
        }
    }

    public void ClearSlot()
    {
        iconImage.sprite = null;
        countText.text = null;
    }

    // 버튼 클릭시 효과 함수
    public void OnClickButton()
    {
        // 아웃라인 컴포넌트 On / Off
        if (outline != null)
        {
            // !ountline.enabled <- 현재 상태의 반대값 : !(반대) + outline.enabled(현재상태)
            outline.enabled = !outline.enabled;
        }
    }

    // 마우스 커서가 올라왔을때 효과
    public async void OnPointerEnter(PointerEventData eventData)
    {
        // 아이템 정보가 있다면
        if (iconImage.sprite != null)
        {
            // 아이템정보UI 켜기
            await UIManager.Instance.Show<ItemDescriptionUI>();

            // 아직 변수가 없다면 가져와서 할당
            if ( itemDescriptionUI == null )
            {
                itemDescriptionUI = UIManager.Instance.Get<ItemDescriptionUI>();
            }

            // 마우스 커서 위치로 옮기기
            RectTransform rectTransform = itemDescriptionUI.GetComponent<RectTransform>();
            Vector3 offset = new Vector3(rectTransform.sizeDelta.x / 3, rectTransform.sizeDelta.y / 3, 0);
            itemDescriptionUI.transform.position = (Vector3)eventData.position + offset;
        }
    }

    // 마우스 커서가 나갔을때 효과
    public void OnPointerExit(PointerEventData eventData)
    {
        // itemDescriptionUI 변수가 null이 아닐 때만 실행
        if (itemDescriptionUI != null && itemDescriptionUI.isActiveAndEnabled)
        {
            // 아이템정보UI 끄기
            itemDescriptionUI.Hide();
        }
    }
}