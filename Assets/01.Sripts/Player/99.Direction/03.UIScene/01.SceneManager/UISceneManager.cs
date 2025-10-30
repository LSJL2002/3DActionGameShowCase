using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class UISceneManager : MonoBehaviour
{
    [Header("Cinemachine")]
    public CinemachineCamera seqCam1;
    public CinemachineCamera seqCam2;
    public CinemachineCamera charCam;
    public CinemachineCameraEvents seq1Event;
    public CinemachineCameraEvents seq2Event;
    public CinemachineCameraEvents charEvent;

    [Header("Panel")]
    public GameObject introUI;
    public GameObject charUI;
    [Header("UI Text")]
    public TMP_Text nameText; // 텍스트에 띄울 이름

    private int currnetCharacterIndex;


    void OnEnable()
    {
        seq1Event = seqCam1.GetComponent<CinemachineCameraEvents>();
        seq2Event = seqCam2.GetComponent<CinemachineCameraEvents>();
        charEvent = charCam.GetComponent<CinemachineCameraEvents>();
        seq1Event.CameraActivatedEvent.AddListener(OnSeqCam1Actived);
        seq2Event.BlendFinishedEvent.AddListener(OnSeqCam2BlendFinished);
        charEvent.CameraActivatedEvent.AddListener(OnCharCamBlendFinished);
    }

    void OnDisable()
    {
        seq1Event.CameraActivatedEvent.RemoveListener(OnSeqCam1Actived);
        seq2Event.BlendFinishedEvent.RemoveListener(OnSeqCam2BlendFinished);
        charEvent.CameraActivatedEvent.RemoveListener(OnCharCamBlendFinished);
    }

    void Start()
    {
        introUI?.SetActive(false);
        charUI?.SetActive(false);
        seqCam2.enabled = false;
        charCam.enabled = false;
    }

    private void OnSeqCam1Actived(ICinemachineMixer mixer, ICinemachineCamera cam)
    {
        if (cam == seqCam1)
        {
            DOVirtual.DelayedCall(1.5f, () =>
            {
                seqCam1.enabled = false; // 이전 카메라 비활성화
                seqCam2.enabled = true;  // 다음 카메라 활성화
            })
                    .SetUpdate(true); // timeScale=0에서도 실행
        }
    }

    private void OnSeqCam2BlendFinished(ICinemachineMixer mixer, ICinemachineCamera cam)
    {
        if (cam == seqCam2)
        {
            introUI?.SetActive(true);
        }
    }

    private void OnCharCamBlendFinished(ICinemachineMixer mixer, ICinemachineCamera cam)
    {
        if (cam == charCam)
        {
            charUI?.SetActive(true);
        }
    }

    public async void UpdateUI(string name)
    {
        if (nameText != null)
        {
            string verticalName = string.Join("\n", name.ToCharArray());
            nameText.text = verticalName;
        }

        await UIManager.Instance.Show<CharacterInfomationUI>();

        switch (name)
        {
            case "Yuki":
                EventsManager.Instance.TriggerEvent<int>(GameEventT.OnSelectChange, 0);
                currnetCharacterIndex = 0;
                break;

            case "Aoi":
                EventsManager.Instance.TriggerEvent<int>(GameEventT.OnSelectChange, 1);
                currnetCharacterIndex = 1;
                break;

            case "Mika":
                EventsManager.Instance.TriggerEvent<int>(GameEventT.OnSelectChange, 2);
                currnetCharacterIndex = 2;
                break;
        }
    }
}