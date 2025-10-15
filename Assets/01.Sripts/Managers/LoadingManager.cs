using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : Singleton<LoadingManager>
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Image loadingImage;
    [SerializeField] private TextMeshProUGUI loadingText;

    protected override void Awake()
    {
        base.Awake();

        Vector3 rotateValue = new Vector3(0, 0, -360);
        Vector3 punchValue = new Vector3(0, 10, 0);

        loadingImage.rectTransform.DORotate(rotateValue, 2f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart);
        loadingText.rectTransform.DOPunchAnchorPos(punchValue, 5f, 2, 10f).SetLoops(-1, LoopType.Restart);
    }

    public void SetLoadingPanel(bool value)
    {
        loadingPanel.SetActive(value);
    }
}
