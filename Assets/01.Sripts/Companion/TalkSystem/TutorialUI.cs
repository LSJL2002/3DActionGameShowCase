using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum SceneType
{
    Tutorial,
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

    private bool playText;

    public List<TextSO> dialogues = new List<TextSO>();

    protected override void OnEnable()
    {
        // playText = true;
    }

    IEnumerator ShowText(List<TextSO> scene, float time)
    {
        foreach (TextSO text in scene)
        {
            if (text.abc == Speaker.Player.ToString())
            {
                playerCamera.SetActive(true);
            }
            else if (text.abc == Speaker.Vix.ToString())
            {
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
}
