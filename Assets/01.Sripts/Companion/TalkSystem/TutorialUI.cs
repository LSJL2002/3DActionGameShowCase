using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TutorialUI : UIBase
{
    public TMP_Text talkText;

    public List<TextSO> dialogues = new List<TextSO>();
    
    // 창이 켜질 때 자동으로 실행됨
    protected override void OnEnable()
    {
        base.OnEnable(); // UIBase의 OnEnable 호출 (없어도 되지만 안전하게 남겨두기)

        if (talkText != null)
            talkText.text = "안녕 나는 vixi야 잘 부탁해";
    }
}
