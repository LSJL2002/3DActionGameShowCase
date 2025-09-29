using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    // private bool playText;
    private bool bossOneIntroPlayed = false;
    // 목스터 생명 수치
    private bool bossOneAt90Played, bossOneAt60Played, bossOneAt30Played, bossOneAt10Played, bossOneAt0Played;
    // 선택 후 대화
    private bool bossOneAfterSelectPlayed = false;

    public List<TextSO> dialogues = new List<TextSO>();

    protected override void OnEnable()
    {
        // playText = true;
    }

    #region Tutorial
    IEnumerator ShowText(List<TextSO> scene, float time)
    {
        foreach (TextSO text in scene)
        {
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

            for (int i = 0; i < text.textContent.Length; i++)
            {
                talkText.text += text.textContent[i];
                yield return new WaitForSeconds(0.05f); // 글자 사이 텀 (0.05초 예시)
            }

            yield return new WaitForSeconds(time);
        }

        Hide();
        playerCamera.SetActive(false);
        CompanionCamera.SetActive(false);
        // playText = false;
    }

    public void PlayDialogue(SceneType type)
    {

        List<TextSO> scene = new List<TextSO>();

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

