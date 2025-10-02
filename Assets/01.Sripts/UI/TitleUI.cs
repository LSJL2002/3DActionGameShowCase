using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class TitleUI : UIBase
{
    [SerializeField] private Image gameTitleImage;
    [SerializeField] private TextMeshProUGUI gameTitleText;
    [SerializeField] private TextMeshProUGUI clickToGameText;
    [SerializeField] private Image blackEffectPanelImage;
    [SerializeField] private Button startButton;

    public void OnClickButton(string str)
    {
        AudioManager.Instance.PlaySFX("ButtonSoundEffect");

        switch (str)
        {
            case "GameStart":
                startButton.gameObject.SetActive(false); // 버튼 다시 안눌리게 비활성화

                // 실행되고 있던 닷트윈 정지 및 세팅
                blackEffectPanelImage.DOKill();
                gameTitleImage.DOKill();
                gameTitleText.DOKill();
                clickToGameText.DOKill();
                blackEffectPanelImage.DOFade(0f, 0f);
                clickToGameText.DOFade(0f, 0f);

                // 첫 번째 DOFade가 완료된 후 두 번째 DOFade가 시작
                gameTitleText.DOFade(1f, 0f).OnComplete(() => { gameTitleText.DOFade(0f, 1.5f); });

                // 첫 번째 DOFade가 완료된 후 두 번째 DOFade가 시작
                gameTitleImage.DOFade(1f, 0f).OnComplete(() =>
                {
                    gameTitleImage.DOFade(0f, 1.5f).OnComplete(async () =>
                    {
                        await UIManager.Instance.Show<HomeUI>();
                        Hide();
                    });
                });

                break;
        }
    }

    protected override void Awake()
    {
        // 초기 이미지 및 텍스트 투명도 세팅 (알파값, 시간)
        gameTitleImage.DOFade(0f, 0f);
        gameTitleText.DOFade(0f, 0f);
        clickToGameText.DOFade(0f, 0f);
        blackEffectPanelImage.DOFade(1f, 0f);
    }

    protected override void Start()
    {
        base.Start();

        // '까만화면'을 0f(알파값0)까지 1초에 걸쳐 동작하고, 완료되면 '게임타이틀' 이미지를 1f(알파값255)까지 5초에 걸쳐 동작
        blackEffectPanelImage.DOFade(0f, 1f).OnComplete(() => { gameTitleImage.DOFade(1f, 5f); });
        gameTitleText.DOFade(1f, 5f).SetDelay(2f); // '게임타이틀' 텍스트를 1초 딜레이 후 5초에 걸쳐 동작


        clickToGameText.DOFade(1f, 2.5f).SetLoops(-1, LoopType.Yoyo).SetDelay(3f);
    }
}
