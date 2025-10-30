using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DirectionManager : MonoBehaviour
{
    [Header("Additive Scene")]
    [SerializeField] private string previewSceneName = "ShowroomScene";

    private PlayerCharacter currentPlayer;


    public void LoadScene(PlayerCharacter player)
    {
        Scene scene = SceneManager.GetSceneByName(previewSceneName);
        if (scene.isLoaded) return;

        currentPlayer = player;
        Time.timeScale = 0f;
        player.PlayerManager.EnableInput(false);
        player._camera.MainCamera.gameObject.SetActive(false);
        SceneManager.LoadScene(previewSceneName, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void UnloadScene()
    {
        Scene scene = SceneManager.GetSceneByName(previewSceneName);
        if (!scene.isLoaded) return;

        SceneManager.UnloadSceneAsync(previewSceneName);

        if (currentPlayer != null)
        {
            currentPlayer.PlayerManager.EnableInput(true);
            currentPlayer._camera.MainCamera.gameObject.SetActive(true);
            currentPlayer = null;
        }

        Time.timeScale = 1f;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != previewSceneName) return;

        SceneManager.sceneLoaded -= OnSceneLoaded;
        // 씬이 로드 완료되었을 때 호출할거 있으면 추가
        // 로드 직후 특정 초기화 작업
    }
}