using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonHoverEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // DOTween으로 페이드인/아웃을 제어할 Canvas Group 컴포넌트
    [SerializeField] private CanvasGroup targetCanvasGroup;
    [SerializeField] private AudioSource audioSource;

    // 마우스가 버튼에 진입했을 때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 캔버스 그룹의 투명도를 0.7f로 변경 (반투명하게)
        if (targetCanvasGroup != null)
        {
            targetCanvasGroup.DOFade(0.2f, 0.2f);
        }

        audioSource.PlayOneShot(audioSource.clip);
    }

    // 마우스가 버튼에서 벗어났을 때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        // 캔버스 그룹의 투명도를 원래 값인 1.0f로 되돌림 (불투명하게)
        if (targetCanvasGroup != null)
        {
            targetCanvasGroup.DOFade(1.0f, 0.2f);
        }
    }
}
