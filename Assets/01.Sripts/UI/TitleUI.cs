using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class TitleUI : UIBase
{
    [SerializeField] private TextMeshProUGUI gameTitleText;
    [SerializeField] private TextMeshProUGUI clickToGameText;
    [SerializeField] private Image blackEffectPanelImage;
    [SerializeField] private AudioSource audioSource;

    public async void OnClickButton(string str)
    {
        switch (str)
        {
            case "GameStart":
                audioSource.PlayOneShot(audioSource.clip);
                await UIManager.Instance.Show<HomeUI>();
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }

    protected override void Awake()
    {
        gameTitleText.color = new Color(gameTitleText.color.r, gameTitleText.color.g, gameTitleText.color.b, 0f);
        clickToGameText.color = new Color(clickToGameText.color.r, clickToGameText.color.g, clickToGameText.color.b, 0f);
        blackEffectPanelImage.color = new Color(blackEffectPanelImage.color.r, blackEffectPanelImage.color.g, blackEffectPanelImage.color.b, 1f);
    }

    protected override void Start()
    {
        base.Start();
        blackEffectPanelImage.DOFade(0f, 1f).OnComplete(() => { gameTitleText.DOFade(1f, 5f); });
        
        clickToGameText.DOFade(1f, 2.5f).SetLoops(-1, LoopType.Yoyo);
    }
}
