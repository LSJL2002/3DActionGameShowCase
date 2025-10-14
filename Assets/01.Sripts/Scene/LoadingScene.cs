using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : SceneBase
{
    [SerializeField] private TextMeshProUGUI loadingText; // 씬로딩 텍스트
    [SerializeField] private Slider loadingSlider; // 씬로딩 슬라이더바
    private float currentProgress = 0f;

    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(LoadAsyncScene(SceneLoadManager.Instance.targetSceneIndex));
    }

    IEnumerator LoadAsyncScene(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // 씬변경 비허가
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // 실제 진행률 (최대 0.9까지)
            float targetProgress = operation.progress;

            // 90%까지 부드럽게 보간
            // 현재 슬라이더 값(currentProgress)을 실제 로딩 진행률(targetProgress)까지 부드럽게 증가
            currentProgress = Mathf.Lerp(currentProgress, targetProgress, Time.deltaTime * 5f); // 5f는 속도 조절

            // 0.9로 나누어 0~100%로 변환
            float displayProgress = Mathf.Clamp01(currentProgress / 0.9f);

            loadingSlider.value = displayProgress;
            loadingText.text = $"Loading... {(int)(displayProgress * 100)}%";

            // 진행율이 90% 이상이 되면,
            if (operation.progress >= 0.9f)
            {
                // 씬 변경 허가
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
