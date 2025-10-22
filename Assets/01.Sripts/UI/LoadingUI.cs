using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : UIBase
{
    [SerializeField] private Image loadingImage;
    [SerializeField] private TextMeshProUGUI loadingText;

    private Tweener rotationTweener;
    private Tweener punchTweener;

    protected override void Awake()
    {
        base.Awake();

        InitializeAnimation();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        rotationTweener?.Play();
        punchTweener?.Play();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        rotationTweener?.Pause();
        punchTweener?.Pause();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        rotationTweener?.Kill();
        punchTweener?.Kill();
    }

    private void InitializeAnimation()
    {
        Vector3 rotateValue = new Vector3(0, 0, -360);
        Vector3 punchValue = new Vector3(0, 10, 0);

        rotationTweener = loadingImage.rectTransform
            .DORotate(rotateValue, 2f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart);

        punchTweener = loadingText.rectTransform
            .DOPunchAnchorPos(punchValue, 5f, 2, 10f)
            .SetLoops(-1, LoopType.Restart);
    }
}
