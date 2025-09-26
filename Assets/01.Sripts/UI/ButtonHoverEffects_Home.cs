using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class ButtonHoverEffects_Home : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CanvasGroup targetCanvasGroup;
    [SerializeField] private GameObject targetButton;
    public float fadeDuration = 1f;

    public void OnEnable()
    {
        targetCanvasGroup.DOFade(0f, 0f).OnComplete(() => { targetCanvasGroup.DOFade(0.5f, fadeDuration); });
    }

    // 마우스가 버튼에 진입했을 때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 타겟의 투명도를 변경
        if (targetCanvasGroup != null)
        {
            targetCanvasGroup.DOFade(1f, 0.3f);
            targetButton.transform.DOScale(1.05f, 0.2f);
        }

        AudioManager.Instance.PlaySFX("ButtonSoundEffect2");
    }

    // 마우스가 버튼에서 벗어났을 때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        // 타겟의 투명도를 원래 값으로 되돌림
        if (targetCanvasGroup != null)
        {
            targetCanvasGroup.DOFade(0.5f, 0.3f);
            targetButton.transform.DOScale(1.0f, 0.2f);
        }
    }
}
