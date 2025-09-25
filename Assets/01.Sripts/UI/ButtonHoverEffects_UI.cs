using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonHoverEffects_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // DOTween으로 페이드인/아웃을 제어할 Canvas Group 컴포넌트
    [SerializeField] private GameObject buttonObject;

    // 마우스가 버튼에 진입했을 때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonObject != null)
        {
            buttonObject.transform.DOScale(1.05f, 0.2f);
        }

        AudioManager.Instance.PlaySFX("ButtonSoundEffect3");
    }

    // 마우스가 버튼에서 벗어났을 때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonObject != null)
        {
            buttonObject.transform.DOScale(1.0f, 0.2f);
        }
    }
}
