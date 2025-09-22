using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewScene : MonoBehaviour
{
    [SerializeField] private string previewSceneName = "PlayerUIScene";
    private bool isLoaded = false;

    private void OnEnable()
    {
        // RawImage가 켜질 때 프리뷰 씬 로드
        if (!isLoaded)
        {
            SceneManager.LoadScene(previewSceneName, LoadSceneMode.Additive);
            isLoaded = true;
        }
    }

    private void OnDisable()
    {
        // RawImage가 꺼질 때 프리뷰 씬 언로드
        if (isLoaded)
        {
            SceneManager.UnloadSceneAsync(previewSceneName);
            isLoaded = false;
        }
    }
}
