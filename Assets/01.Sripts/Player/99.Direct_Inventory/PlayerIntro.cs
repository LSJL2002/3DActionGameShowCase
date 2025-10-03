using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerIntro : MonoBehaviour
{
    [Header("LetterBox")]
    public LetterBox letterBox;

    [Header("Intro Camera")]
    public CinemachineFreeLook freeLook;
    public CinemachineBlendListCamera introCam;

    [Header("Intro Settings")]
    [SerializeField] private float introDuration = 5f; // 카메라 연출 길이

    private void Start()
    {
        PlayerManager.Instance.EnableInput(false);
        Cursor.lockState = CursorLockMode.Locked;
        // 카메라 우선순위 설정
        introCam.Priority = 20;

        // 레터박스 활성화 + 준비 완료 시 등장
        letterBox.gameObject.SetActive(true);

        // 카메라 연출 길이 끝나면 자동 종료
        StartCoroutine(FinishIntroAfterDelay(introDuration));
    }

    private IEnumerator FinishIntroAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 레터박스 슬라이드 아웃 + 영상 종료
        letterBox.LetterBoxOut();

        // 카메라 원래 우선순위로 복귀
        introCam.Priority = 0;

        PlayerManager.Instance.EnableInput(true);
    }
}
