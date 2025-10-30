using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class IntroCamera : MonoBehaviour
{
    [Header("LetterBox")]
    public LetterBox letterBox;
    [Header("Intro Camera")]
    public CinemachineSequencerCamera seqCam;
    public CinemachineCameraEvents seqEvent;

    private PlayerCharacter player;
    private Transform face;

    private void OnEnable()
    {
        seqEvent.BlendFinishedEvent.AddListener(OnBlendFinished);
    }
    private void OnDisable()
    {
        seqEvent.BlendFinishedEvent.RemoveListener(OnBlendFinished);
    }

    private void Start()
    {
        player = PlayerManager.Instance.ActiveCharacter;
        face = player.Face;
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;

        if (seqCam != null)
            seqCam.DefaultTarget.Target.TrackingTarget = face;

        player.PlayerManager.EnableInput(false);
        Cursor.lockState = CursorLockMode.Locked;

        // 레터박스 활성화 + 준비 완료 시 등장
        letterBox.gameObject.SetActive(true);
    }

    private void OnBlendFinished(ICinemachineMixer mixer, ICinemachineCamera cam)
    {
        // 레터박스 닫기 + 시퀀스 카메라 비활성화 + 입력 복구
        letterBox.LetterBoxOut();
        seqCam.enabled = false;
        player.PlayerManager.EnableInput(true);
    }
}