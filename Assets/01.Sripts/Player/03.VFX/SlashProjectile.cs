using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class SlashProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;  // 이동 속도
    [SerializeField] private Transform player;    // 플레이어 오브젝트를 인스펙터/코드로 세팅

    private Vector3 moveDir;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;



    private void OnEnable()
    {
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        if (player != null)
            moveDir = player.forward.normalized; // 항상 현재 플레이어 방향
        else
            moveDir = transform.forward; // fallback

        CancelInvoke(); // 혹시 이전 Invoke 남아있다면 제거
    }

    private void Update()
    {
        // 월드 좌표 기준으로 정확하게 이동
        transform.position += moveDir * speed * Time.deltaTime;
    }

    private void OnDisable()
    {
        // 풀에 돌아갈 때 발사 전 위치/회전으로 초기화
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        moveDir = Vector3.zero; // 이전 방향 초기화
    }
}