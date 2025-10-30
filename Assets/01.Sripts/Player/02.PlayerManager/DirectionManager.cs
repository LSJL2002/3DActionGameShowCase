using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DirectionManager : MonoBehaviour
{
    [Header("Additive Scene")]
    [SerializeField] private string previewSceneName = "ShowroomScene";

    private bool isLoaded = false;

    public void LoadScene()
    {
        if (isLoaded) return;
        SceneManager.LoadScene(previewSceneName, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnSceneLoaded;
        isLoaded = true;
    }

    public void UnloadScene()
    {
        if (!isLoaded) return;
        SceneManager.UnloadSceneAsync(previewSceneName);
        isLoaded = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != previewSceneName) return;

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}