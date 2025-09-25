using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Profiling.HierarchyFrameDataView;

// 아이템 슬롯 하나하나의 상태를 시각적으로 보여주는 View 계층의 클래스 (아이콘, 수량숫자 넣고 빼기 등)
public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Outline outline;
    [SerializeField] private TextMeshProUGUI emptyText;
    [SerializeField] private Sprite emptyImage;

    private InventoryViewModel inventoryViewModel;

    public ItemData itemData;
    public string itemName;
    public string itemType;
    public string itemDescription;
    private ItemInformationUI itemInformationUI;

    // 기본 컬러값 설정
    private Color defaultColor = new Color(210 / 255f, 210 / 255f, 210 / 255f, 255 / 255f);

    public void OnEnable()
    {
        if (inventoryViewModel == null)
        {
            inventoryViewModel = InventoryManager.Instance.inventoryViewModel;
        }
    }

    public void SetData(ItemData data,  int count = default)
    {
        itemData = data;
        iconImage.sprite = data.sprite;
        iconImage.color = new Color(data.colors[0] / 255.0f, data.colors[1] / 255.0f, data.colors[2] / 255.0f); // data의 color 리스트의 0,1,2 번째 수를 가져와서 할당
        itemName = data.inGameName;
        itemType = data.itemType.ToString();
        itemDescription = data.scriptText;

        if (count > 0)
        {
            countText.text = count.ToString();
        }
    }

    public void ClearSlot()
    {
        countText.text = null;
        itemData = null;
        emptyText.text = "Empty";
        iconImage.sprite = emptyImage;
        iconImage.color = defaultColor; // 이미지 컬러를 다시 기본으로 되돌림
    }

    // 버튼 클릭시 효과 함수
    public void OnClickButton()
    {
        switch (InventoryManager.Instance.currentDecisionState)
        {
            // 아이템 사용 상황
            case DecisionState.UseItem:

                switch (itemData.itemType)
                {
                    // 소비아이템이라면
                    case ItemData.ItemType.Consumable:
                        // ViewModel의 함수 호출
                        inventoryViewModel.SelectItem(itemData);
                        break;

                    // 코어라면 장착 효과 ( 플레이어의 함수를 호출 (장비효과를 On하는 함수) )
                    case ItemData.ItemType.Core:
                        // 아웃라인 컴포넌트 On / Off
                        // !ountline.enabled <- 현재 상태의 반대값 : !(반대) + outline.enabled(현재상태)
                        outline.enabled = !outline.enabled;
                        break;

                    // 스킬이라면 장착 효과 ( 플레이어의 함수를 호출 (장비효과를 On하는 함수) )
                    case ItemData.ItemType.SkillCard:
                        // 아웃라인 컴포넌트 On / Off
                        // !ountline.enabled <- 현재 상태의 반대값 : !(반대) + outline.enabled(현재상태)
                        outline.enabled = !outline.enabled;
                        break;
                }    
                break;

            // 능력 선택 상황
            case DecisionState.SelectAbility:
                // ViewModel의 함수 호출
                inventoryViewModel.SelectItem(itemData);
                break;
        }
    }

    // 마우스 커서가 올라왔을때 효과
    public async void OnPointerEnter(PointerEventData eventData)
    {
        // 아이템 정보가 있다면
        if (itemData != null)
        {
            AudioManager.Instance.PlaySFX("ButtonSoundEffect3");

            // 아이템정보UI 켜기
            await UIManager.Instance.Show<ItemInformationUI>();

            // 아직 변수가 없다면 가져와서 할당
            if ( itemInformationUI == null )
            {
                itemInformationUI = UIManager.Instance.Get<ItemInformationUI>();
            }

            itemInformationUI.SetItemSlotData(this);

            // 정보창을 마우스 커서 위치로 옮기기
            RectTransform rectTransform = itemInformationUI.GetComponent<RectTransform>();
            Vector3 offset = new Vector3(rectTransform.sizeDelta.x/2f, rectTransform.sizeDelta.y/2f, 0);
            itemInformationUI.transform.position = (Vector3)eventData.position - offset;
        }
    }

    // 마우스 커서가 나갔을때 효과
    public void OnPointerExit(PointerEventData eventData)
    {
        // itemDescriptionUI 변수가 null이 아닐 때만 실행
        if (itemInformationUI != null && itemInformationUI.isActiveAndEnabled)
        {
            // 아이템정보UI 끄기
            itemInformationUI.Hide();
        }
    }
}