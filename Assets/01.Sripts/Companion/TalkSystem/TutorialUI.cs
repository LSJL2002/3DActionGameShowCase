using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TutorialUI : UIBase
{
    public TMP_Text talkText;
    public GameObject tutoriaUI;

    public void OnClick()
    {
        tutoriaUI.SetActive(true);
        talkText.text = "안녕 나는 vixi야 잘 부탁해";
    }
}
