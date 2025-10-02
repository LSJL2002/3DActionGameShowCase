using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 아이템 슬롯 하나하나의 상태를 시각적으로 보여주는 View 계층의 클래스 (아이콘, 수량숫자 넣고 빼기 등)
public class ItemSlotUI : MonoBehaviour
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

    // 기본 컬러값 설정
    private Color defaultColor = new Color(0 / 255f, 0 / 255f, 0 / 255f, 200 / 255f);

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
        if (itemData == null) return;

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

        UIManager.Instance.Hide<ItemInformationUI>();
    }
}