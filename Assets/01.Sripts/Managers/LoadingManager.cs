using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : Singleton<LoadingManager>
{
    public TextMeshProUGUI loadingText; // 씬로딩 텍스트
    public Slider loadingSlider; // 씬로딩 슬라이더바

    protected override void Awake()
    {
        StartCoroutine(LoadAsyncScene(SceneLoadManager.Instance.targetSceneIndex));
    }

    IEnumerator LoadAsyncScene(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // 씬변경 비허가
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // 로딩 슬라이더의 값에 진행률을 대입
            loadingSlider.value = progress;
            // 로딩 텍스트 업데이트
            loadingText.text = $"Loading... {(int)(progress * 100)}%";

            // 진행율이 90% 이상이 되면,
            if (operation.progress >= 0.9f)
            {
                // 슬라이더를 100%로 맞추고
                loadingSlider.maxValue = 1f;

                // 씬변경 허가
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
