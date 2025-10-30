using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewScene : MonoBehaviour
{
    [Header("Preview Scene")]
    [SerializeField] private string previewSceneName = "ShowroomScene";

    private bool isLoaded = false;


    private void LoadPreviewScene()
    {
        if (isLoaded) return;
        SceneManager.LoadScene(previewSceneName, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnPreviewSceneLoaded;
        isLoaded = true;
    }

    private void UnloadPreviewScene()
    {
        if (!isLoaded) return;
        SceneManager.UnloadSceneAsync(previewSceneName);
        isLoaded = false;
    }

    private void OnPreviewSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != previewSceneName) return;

        SceneManager.sceneLoaded -= OnPreviewSceneLoaded;
    }
}