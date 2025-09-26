using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject.SpaceFighter;

public class VFXManager : MonoBehaviour
{
    [Header("Dash Effect")]
    [SerializeField] private GameObject dashVFX;       // Looping 파티클 오브젝트
    [SerializeField] private Transform playerModel;    // 숨길 플레이어 모델 (아바타)
    [SerializeField] private Transform player;         // 위치 기준

    private ParticleSystem ps;

    private void Awake()
    {
        if (dashVFX != null)
        {
            ps = dashVFX.GetComponent<ParticleSystem>();
            dashVFX.SetActive(false);  // 처음엔 꺼둠
        }
    }

    /// <summary>
    /// 스킬 시작 시 파티클 켜고 플레이어 숨기기
    /// </summary>
    public void StartDash()
    {
        if (dashVFX == null || player == null || playerModel == null) return;

        // 플레이어 모델 숨기기
        playerModel.gameObject.SetActive(false);

        // 파티클 켜기
        dashVFX.SetActive(true);
        dashVFX.transform.position = player.position;
        dashVFX.transform.rotation = player.rotation;

        ps?.Play();
    }

    /// <summary>
    /// 스킬 종료 시 파티클 끄고 플레이어 다시 보이게
    /// </summary>
    public void StopDash()
    {
        if (dashVFX == null || playerModel == null) return;

        ps?.Stop();
        dashVFX.SetActive(false);

        // 플레이어 모델 다시 보이기
        playerModel.gameObject.SetActive(true);
    }

    private void LateUpdate()
    {
        if (dashVFX.activeSelf && player != null)
        {
            // 플레이어 이동 따라다니기
            dashVFX.transform.position = player.position;
            dashVFX.transform.rotation = player.rotation;
        }
    }
}
