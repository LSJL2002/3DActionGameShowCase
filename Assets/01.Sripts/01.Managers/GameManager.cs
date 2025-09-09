using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//씬관리, 일시정지, 로딩
public enum GameScene
{
    [SceneName("IntroScene")] IntroScene,
    [SceneName("MainScene")] MainScene,
    [SceneName("Level2")] Level2,
    [SceneName("GameOver")] GameOver
}
[AttributeUsage(AttributeTargets.Field)]
public class SceneNameAttribute : Attribute
{
    public string Name { get; }
    public SceneNameAttribute(string name) => Name = name;
}
public static class SceneUtility
{
    public static string GetSceneName(GameScene scene)
    {
        var type = typeof(GameScene);
        var memInfo = type.GetMember(scene.ToString());
        var attr = memInfo[0].GetCustomAttributes(typeof(SceneNameAttribute), false);
        return (attr.Length > 0) ? ((SceneNameAttribute)attr[0]).Name : scene.ToString();
    }
}


class GameState
{

}
public partial class GameManager : Singleton<GameManager>
{
    private GameState _currentState;
    bool IsPause = false;

    // 현재 씬을 저장 할 변수 세팅
    private Scene currentScene;



    // Player 인스턴스에 접근하기 위한 Instance 함수

    private void Awake()
    {

        DontDestroyOnLoad(gameObject);

        // 현재 씬을 가져와서 변수에 저장
        currentScene = SceneManager.GetActiveScene();
    }
    private void Start()
    {
        Application.targetFrameRate = 120;
    }

    // GameState 전환 함수


    // 게임 시작
    public void StartGame()
    {
        // SceneManagement를 통해 1번 씬으로 전환
        SceneManager.LoadScene(1);
    }

    // 게임 일시정지
    public void PauseGame()
    {
        // GameState 상태를 Paused로 전환
        Debug.Log($"GameState : {_currentState}");

        // 게임 정지
        if (IsPause == false)
        {
            Time.timeScale = 0;
            IsPause = true;
            return;
        }
    }

    // 게임 일시정지
    public void ReturnGame()
    {
        // GameState 상태를 Playing으로 전환

        // 게임 정지
        if (IsPause == true)
        {
            Time.timeScale = 1;
            IsPause = false;
            return;
        }
    }

    // 게임 오버
    public void GameOver()
    {
        // GameState 상태를 GameOver로 전환
    }

    // SceneManager.sceneLoaded 라는 이벤트에 구독 할 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 로드된 씬의 buildIndex에 따라 UI 상태를 다르게 설정
        //switch (scene.buildIndex)
        //{
        //    case 0:
        //        ChangeGameState(GameState.Intro);
        //        break;
        //    case 1:
        //        ChangeGameState(GameState.Playing);
        //        break;
        //}

    }

    private void OnDestroy()
    {
        // 오브젝트가 파괴될 때 이벤트 등록 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
