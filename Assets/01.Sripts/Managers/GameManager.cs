using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임의 상태를 나타내는 열거형
public enum eGameState
{
    Home,
    //Intro,
    Playing,
    Pause,
    GameOver,
    GameClear
}

// 제네릭 싱글톤 스크립트를 상속
public class GameManager : Singleton<GameManager>
{
    private eGameState previousState; // 이전 상태를 저장할 변수
    private eGameState currentState; // 현재 게임 상태를 저장할 변수

    // 스테이지 변경을 위한 Action 델리게이트
    public event Action<eGameState> ChangeStage;

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
        // 이전 상태를 변수에 저장
        previousState = currentState;

        // 현재 상태를 변수에 저장
        currentState = newState;

        // GameState 상태를 전환하는 이벤트 발생
        ChangeStage?.Invoke(newState);
    }

    // 게임 시작
    public void StartGame()
    {
        // 게임 상태를 변경
        ChangeState(eGameState.Playing);

        // SceneLoadManager를 통해 게임 씬으로 전환
        //SceneLoadManager.Instance.ChangeScene(0);
    }

    // 게임 일시정지
    public void PauseGame(bool check)
    {
        // 게임 정지
        if (check)
        {
            Time.timeScale = 0;
            return;
        }

        // 게임 재개
        else if (!check)
        {
            Time.timeScale = 1;
            return;
        }

        // 게임 상태를 변경
        ChangeState(eGameState.Pause);

        Debug.Log($"IsPauseGame : {check}");
    }

    // 게임 오버
    public void GameOver()
    {
        // 게임 상태를 변경
        ChangeState(eGameState.GameOver);

        // 게임오버씬으로 전환
        //SceneLoadManager.Instance.ChangeScene(0);
    }

    // 게임 클리어
    public void GameClear()
    {
        // 게임 상태를 변경
        ChangeState(eGameState.GameClear);

        // 게임클리어씬으로 전환
        //SceneLoadManager.Instance.ChangeScene(0);
    }
}
