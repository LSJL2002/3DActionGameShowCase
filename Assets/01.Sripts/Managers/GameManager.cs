using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

// 게임의 상태를 나타내는 열거형
public enum eGameState
{
    Home,
    //Intro,
    GamePlaying,
    Pause,
    GameEnd,
    GameClear
}

// 시작할 게임의 모드 (새게임, 불러오기)


// 제네릭 싱글톤 스크립트를 상속
public class GameManager : Singleton<GameManager>
{
    private eGameState previousState; // 이전 상태를 저장할 변수
    private eGameState currentState; // 현재 게임 상태를 저장할 변수

    

    // 상태 변경을 위한 Action 델리게이트
    public event Action<eGameState> changeState;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        // 게임의 목표 프레임 속도를 설정
        Application.targetFrameRate = 120;
    }

    // GameState 전환 함수
    public void ChangeState(eGameState newState)
    {
        Debug.Log($"ChangeState : {newState}");

        // 이전 상태를 변수에 저장
        previousState = currentState;

        // 현재 상태를 변수에 저장
        currentState = newState;

        // GameState 상태를 전환하는 이벤트 발생
        changeState?.Invoke(newState);
    }

    // 게임 시작
    public void StartGame()
    {
        // 게임 상태를 변경
        ChangeState(eGameState.GamePlaying);

        // 게임씬을 로드
        //SceneLoadManager.Instance.LoadScene(5);
    }

    // 게임 일시정지
    public async UniTask PauseGame(bool check)
    {
        // 게임 정지
        if (check)
        {
            Time.timeScale = 0;
        }

        // 게임 재개
        else if (!check)
        {
            Time.timeScale = 1;
        }

        // 게임 상태를 변경
        ChangeState(eGameState.Pause);

        Debug.Log($"IsPauseGame : {check}");
    }
}
