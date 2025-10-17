using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SceneType
{
    Tutorial,
    Boss_1,
}

public enum Speaker
{
    Vix,
    Player
}

public class TutorialUI : UIBase
{
    public TMP_Text talkText;

    public GameObject playerCamera;
    public GameObject CompanionCamera;

    public Image skipProgressImage; // fill amount 이미지
    private float holdToSkipAll = 1.5f; // 1.5초 동안 fill amount 1로
    private float releaseReturnTime = 0.6f; // 중간에 손을 때면 0.6초 동안 0으로 

    // private bool playText;
    private bool bossOneIntroPlayed = false;
    // 목스터 생명 수치
    private bool bossOneAt90Played, bossOneAt60Played, bossOneAt30Played, bossOneAt10Played, bossOneAt0Played;
    // 선택 후 대화
    private bool bossOneAfterSelectPlayed = false;

    public List<TextSO> dialogues = new List<TextSO>();

    public static event Action endTutorial;

    [SerializeField] CanvasGroup tutorialUICanvasGroup;

    protected override void Awake()
    {
        base.Awake();

        // UI매니저의 튜토리얼 재생 여부 확인 후 재생
        if (UIManager.Instance.tutorialEnabled)
        {
            // n초 대기 후 실행
            DOVirtual.DelayedCall(6f, () => 
            {
                UIManager.Instance.Get<TutorialUI>().PlayDialogue(SceneType.Tutorial);
            });            
        }
    }

    protected override void OnEnable()
    {
        // playText = true;
    }

    #region Tutorial
    IEnumerator ShowText(List<TextSO> scene, float time)
    {
        float xHeld = 0f;      // 누적 홀드시간(초)
        bool skipAll = false;  // 전체 스킵 플래그

        // 로컬 함수: 매 프레임 게이지/홀드시간 갱신
        void UpdateSkipUI(bool isHolding)
        {
            if (skipProgressImage != null)
            {
                if (isHolding)
                {
                    xHeld += Time.deltaTime;
                    skipProgressImage.fillAmount = Mathf.Clamp01(xHeld / holdToSkipAll);
                }
                else
                {
                    // 손을 떼면 fillAmount가 releaseReturnTime초에 걸쳐 0으로 감쇠
                    float step = (releaseReturnTime > 0f) ? Time.deltaTime / releaseReturnTime : 1f;
                    skipProgressImage.fillAmount = Mathf.MoveTowards(skipProgressImage.fillAmount, 0f, step);
                    xHeld = skipProgressImage.fillAmount * holdToSkipAll; // 일관성 유지
                }

                // 게이지가 0이면 감춰도 됨
                skipProgressImage.enabled = skipProgressImage.fillAmount > 0f;
            }
        }

        foreach (TextSO text in scene)
        {
            if (skipAll) break;

            if (text.abc == Speaker.Player.ToString())
            {
                playerCamera.SetActive(true);
                CompanionCamera.SetActive(false);
            }
            else if (text.abc == Speaker.Vix.ToString())
            {
                CompanionCamera.SetActive(true);
                playerCamera.SetActive(false);
            }

            talkText.text = "";
            bool completedBySkip = false; // 이번문장이 x키로 즉시 완성됐는지 표시.

            // 글자 출력
            for (int i = 0; i < text.textContent.Length; i++)
            {
                if (skipAll) break;

                talkText.text += text.textContent[i];

                // 0.05초 대기 or X키 입력 시 즉시 스킵
                float elapsed = 0.0f;
                while (elapsed < 0.05f) 
                {
                    bool holding = Input.GetKeyUp(KeyCode.X);
                    UpdateSkipUI(holding);

                    // 전체 스킵
                    if (xHeld >= holdToSkipAll) { skipAll = true; break; }

                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        talkText.text = text.textContent;
                        i = text.textContent.Length;
                        completedBySkip = true;
                        break;
                    }
                    elapsed += Time.deltaTime;
                    yield return null;
                }
            }

            if (skipAll) break;

            // 같은 프레임 중복 입력 방지 + 길게 누름 감시
            if (completedBySkip)
            {
                yield return null; // 한 프레임 쉬기
                while (true)
                {
                    bool holding = Input.GetKey(KeyCode.X);
                    UpdateSkipUI(holding);

                    if (xHeld >= holdToSkipAll) { skipAll = true; break; }
                    if (!holding) break; // 키를 떼면 다음 단계로
                    yield return null;
                }
            }

            if (skipAll) break;

            // 문장 간 대기
            float wait = 0f;
            while (wait < time)
            {
                bool holding = Input.GetKey(KeyCode.X);
                UpdateSkipUI(holding);

                if (xHeld >= holdToSkipAll) { skipAll = true; break; }
                if (Input.GetKeyDown(KeyCode.X)) break; // 다음 문장으로

                wait += Time.deltaTime;
                yield return null;
            }

            if (skipAll) break;
        }
        endTutorial?.Invoke();
        Hide();
        playerCamera.SetActive(false);
        CompanionCamera.SetActive(false);

        // 게이지 리셋
        if (skipProgressImage != null)
        {
            skipProgressImage.fillAmount = 0f;
            skipProgressImage.enabled = false;
        }
    }

    List<TextSO> scene;

    public void PlayDialogue(SceneType type)
    {

        scene = new List<TextSO>();

        foreach (TextSO text in dialogues)
        {
            if (text.scenes == type.ToString())
            {
                scene.Add(text);
            }
        }

        StartCoroutine(ShowText(scene, 3));
    }
    #endregion

    #region Boss_1
    public void PlayBossDialogue(SceneType type)
    {
        if (type != SceneType.Boss_1 || bossOneIntroPlayed) return;
        
        List<TextSO> scene = new List<TextSO>();

        foreach (TextSO text in dialogues)
        {
            if (text.scenes == type.ToString() && text.id >= 50010019 && text.id <= 50010023)
            {
                scene.Add(text);
            }
        }

        // id 오름차순 정렬(19 → 23 순서 보장)
        scene.Sort((a, b) => a.id.CompareTo(b.id));

        // 다시 재생되지 않도록 플래그 세팅
        bossOneIntroPlayed = true;

        StartCoroutine(ShowText(scene, 2.5f));
    }

    public void TryPlayBossThresholdDialogue(SceneType type, float hpPercent)
    {
        if (type != SceneType.Boss_1) return;

        if (!bossOneAt90Played && hpPercent <= 0.90f)
        {
            bossOneAt90Played = true;
            TextSO match = dialogues.Find(d => d.scenes == type.ToString() && d.id == 50010024);
            if (match != null) StartCoroutine(ShowText(new List<TextSO> { match }, 2.0f));
        }

        if (!bossOneAt60Played && hpPercent <= 0.60f)
        {
            bossOneAt60Played = true;
            TextSO match = dialogues.Find(d => d.scenes == type.ToString() && d.id == 50010025);
            if (match != null) StartCoroutine(ShowText(new List<TextSO> { match }, 2.0f));
        }

        if (!bossOneAt30Played && hpPercent <= 0.30f)
        {
            bossOneAt30Played = true;
            TextSO match = dialogues.Find(d => d.scenes == type.ToString() && d.id == 50010026);
            if (match != null) StartCoroutine(ShowText(new List<TextSO> { match }, 2.0f));
        }

        if (!bossOneAt10Played && hpPercent <= 0.10f)
        {
            bossOneAt10Played = true;
            TextSO match = dialogues.Find(d => d.scenes == type.ToString() && d.id == 50010027);
            if (match != null) StartCoroutine(ShowText(new List<TextSO> { match }, 2.0f));
        }

        if (!bossOneAt0Played && hpPercent <= 0.001f)
        {
            bossOneAt0Played = true;
            List<TextSO> scene = new List<TextSO>();
            foreach (TextSO text in dialogues)
            {
                if (text.scenes == type.ToString() && text.id >= 50010028 && text.id <= 50010031)
                {
                    scene.Add(text);
                }
            }
            scene.Sort((a, b) => a.id.CompareTo(b.id)); 
            StartCoroutine(ShowText(scene, 2.0f));      
        }
    }

    public void PlayBossAfterSelection(SceneType type)
    {
        if (type != SceneType.Boss_1 || bossOneAfterSelectPlayed) return;
        List<TextSO> scene = new List<TextSO>();
        foreach (TextSO text in dialogues)
        {
            // 50010032 ~ 50010033 두 줄만 수집
            if (text.scenes == type.ToString() && text.id >= 50010032 && text.id <= 50010033)
            {
                scene.Add(text);
            }
        }

        scene.Sort((a, b) => a.id.CompareTo(b.id));
        bossOneAfterSelectPlayed = true;

        StartCoroutine(ShowText(scene, 2.0f));
    }
    #endregion
}

