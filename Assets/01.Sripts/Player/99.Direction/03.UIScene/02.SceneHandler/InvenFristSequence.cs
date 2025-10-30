using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class InvenFristSequence : MonoBehaviour
{
    [SerializeField] private UISceneManager manager;

    [Header("Buttons (Optional, for camera activation)")]
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;

    [Header("Characters")]
    [SerializeField] private GameObject char1; // 이미 씬에 있는 캐릭터
    [SerializeField] private GameObject char2; // 새로 생성할 캐릭터 프리팹
    [SerializeField] private GameObject char3; // 새로 생성할 캐릭터 프리팹
    [Header("Spawn Point")]
    [SerializeField] private Transform spawnPoint;
    private GameObject currentChar; // 지금 씬에 존재하는 캐릭


    void Start()
    {
        if (button1 != null) button1.onClick.AddListener(() => SpawnCharacter(char1));
        if (button2 != null) button2.onClick.AddListener(() => SpawnCharacter(char2));
        if (button3 != null) button3.onClick.AddListener(() => SpawnCharacter(char3));
    }

    private void SpawnCharacter(GameObject prefab)
    {
        if (prefab == null || spawnPoint == null) return;

        // 기존 캐릭터 제거
        if (currentChar != null) Destroy(currentChar);
        currentChar = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
        currentChar.name = prefab.name; // 뒤에 Clone이 붙노 자꾸 ㅋㅋ

        manager.UpdateUI(prefab.name);

        // 카메라 비활성 및 UI 닫기
        manager.seqCam2.enabled = false;
        manager.charCam.enabled = true;
        manager.introUI.SetActive(false);
    }
}