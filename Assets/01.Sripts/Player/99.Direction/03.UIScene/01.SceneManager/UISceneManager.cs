using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class UISceneManager : MonoBehaviour
{
    [Header("Cinemachine")]
    public CinemachineBrain brain;
    public CinemachineSequencerCamera seqCam;
    public CinemachineCamera CharCam;

    [Header("Panel")]
    public GameObject IntroUI;
    public GameObject CharUI;
    [Header("UI Text")]
    public TMP_Text nameText; // 텍스트에 띄울 이름


    void Start()
    {
        if (IntroUI != null) IntroUI.SetActive(false);
        if (CharUI != null) CharUI.SetActive(false);
    }

    // UI 제어 메서드
    public void ShowIntroUI() => IntroUI?.SetActive(true);
    public void HideIntroUI() => IntroUI?.SetActive(false);
    public void ShowCharUI() => CharUI?.SetActive(true);
    public void HideCharUI() => CharUI?.SetActive(false);

    public void UpdateUI(string name)
    {
        if (nameText != null)
        {
            string verticalName = string.Join("\n", name.ToCharArray());
            nameText.text = verticalName;
        }

        switch (name)
        {
            case "Yuki":
                //ShowChar1UI();
                break;

            case "Aoi":
                //ShowChar2UI();
                break;

            case "Mika":
                //ShowChar3UI();
                break;
        }
    }
}