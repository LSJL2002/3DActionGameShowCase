using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum SceneType
{
    Tutorial,
}
public class TutorialUI : UIBase
{
    public TMP_Text talkText;

    public GameObject playerCamera;
    public GameObject companionCamera;

    private bool playText;

    public List<TextSO> dialogues = new List<TextSO>();

    protected override void OnEnable()
    {
        // playText = true;
    }

    IEnumerator ShowText(List<string> scene, float time)
    {
        foreach (string text in scene)
        {
            talkText.text = text;
            yield return new WaitForSeconds(time);

        }

        // playText = false;
    }

    public void PlayDialogue(SceneType type)
    {
        
        List<string> scene = new List<string>();

        foreach (TextSO text in dialogues)
        {
            if (text.scenes == type.ToString())
            {
                scene.Add(text.textContent);
            }
        }

        StartCoroutine(ShowText(scene, 2));
    }
}
