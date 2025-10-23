using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private enum ButtonType
    {
        HomeButton,
        UIButton,
        ItemSlotButton,
    }

    [SerializeField] private ButtonType buttonType = ButtonType.UIButton;
    [SerializeField] private CanvasGroup targetCanvasGroup;
    [SerializeField] private GameObject targetButton;
    [SerializeField] private ItemSlotUI itemSlotUI;
    private ItemInformationUI itemInformationUI;
    private bool isPointerEntering = false;
    public float fadeDuration = 1f;

    public void OnEnable()
    {
        if (targetCanvasGroup != null)
        {
            targetCanvasGroup.DOFade(0f, 0f).OnComplete(() => { targetCanvasGroup.DOFade(0.5f, fadeDuration); });
        }
    }

    // 마우스가 버튼에 진입했을 때 호출
    public async void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX("ButtonSoundEffect2");

        switch (buttonType)
        {
            case ButtonType.HomeButton:
                targetCanvasGroup.DOFade(1f, 0.3f);
                targetButton.transform.DOScale(1.05f, 0.2f);
                break;

            case ButtonType.UIButton:
                targetButton.transform.DOScale(1.05f, 0.2f);
                break;
            
            case ButtonType.ItemSlotButton:
                if (isPointerEntering) return;
                isPointerEntering = true;

                if (itemSlotUI.itemData != null) // 아이템 정보가 있다면
                {
                    AudioManager.Instance.PlaySFX("ButtonSoundEffect3");
                    this.transform.DOScale(1.1f, 0.2f);

                    // 아이템정보UI 켜기
                    await UIManager.Instance.Show<ItemInformationUI>();

                    // 아직 변수가 없다면 가져와서 할당
                    if (itemInformationUI == null)
                    {
                        itemInformationUI = UIManager.Instance.Get<ItemInformationUI>();
                    }

                    itemInformationUI.SetItemSlotData(itemSlotUI);

                    // 정보창을 마우스 커서 위치로 옮기기
                    RectTransform rectTransform = itemInformationUI.GetComponent<RectTransform>();
                    Vector3 offset = new Vector3(rectTransform.sizeDelta.x / 2f, rectTransform.sizeDelta.y / 2f, 0);
                    itemInformationUI.transform.position = (Vector3)eventData.position - offset;
                }
                isPointerEntering = false;
                break;
        }
    }

    // 마우스가 버튼에서 벗어났을 때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        switch(buttonType)
        {
            case ButtonType.HomeButton:
                targetCanvasGroup.DOFade(0.5f, 0.3f);
                targetButton.transform.DOScale(1.0f, 0.2f);
                break;
            case ButtonType.UIButton:
                targetButton.transform.DOScale(1.0f, 0.2f);
                break;
                
            case ButtonType.ItemSlotButton:
                // itemDescriptionUI 변수가 null이 아닐 때만 실행
                if (itemInformationUI != null && itemInformationUI.isActiveAndEnabled)
                {
                    this.transform.DOScale(1.0f, 0.2f);

                    // 아이템정보UI 끄기
                    itemInformationUI.Hide();
                }
                break;
        }
    }
}
