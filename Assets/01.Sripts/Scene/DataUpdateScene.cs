using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations; // AsyncOperationStatus를 위해 추가
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataUpdateScene : SceneBase
{
    [Header("UI")]
    [SerializeField] private GameObject waitMessage;
    [SerializeField] private GameObject downMessage;
    [SerializeField] private Slider downSliders;
    [SerializeField] private TextMeshProUGUI sizeInfoText;
    [SerializeField] private TextMeshProUGUI downValueText;

    [Header("Label")]
    [SerializeField] private List<AssetLabelReference> labelList = new List<AssetLabelReference>();

    private long patchSize;
    private Dictionary<string, long> patchDic = new Dictionary<string, long>();

    // 💡 개선 2 해결: 다운로드가 필요한 라벨 목록을 저장할 리스트 추가
    private List<string> labelsToDownload = new List<string>();

    protected override void Awake()
    {
        base.Awake();

        waitMessage.SetActive(true);
        downMessage.SetActive(false);

        // 💡 개선 1 해결: InitAddressable만 시작하고, 완료 후 CheckUpdateFile을 호출하도록 구조 변경
        StartCoroutine(InitAddressableAndCheck());
    }

    IEnumerator InitAddressableAndCheck()
    {
        // 1. Addressables 초기화
        var init = Addressables.InitializeAsync();
        yield return init;

        // 2. 원격 카탈로그 체크 및 갱신
        var check = Addressables.CheckForCatalogUpdates(false);
        yield return check;

        if (check.Status == AsyncOperationStatus.Succeeded && check.Result != null && check.Result.Count > 0)
        {
            // 카탈로그 갱신
            var update = Addressables.UpdateCatalogs(check.Result);
            yield return update;

            if (update.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"카탈로그 업데이트 실패: {update.OperationException}");
            }
            Addressables.Release(update);

            Debug.Log($"Catalogs updated: {string.Join(", ", check.Result)}");
        }
        else
        {
            Debug.Log("No catalog updates.");
        }
        Addressables.Release(check);

        // 💡 초기화 및 카탈로그 업데이트 완료 후, 파일 체크 로직 시작
        yield return CheckUpdateFile();
    }

    #region Check DownLoad
    IEnumerator CheckUpdateFile()
    {
        patchSize = 0; // long 타입 초기화 (default 대신 0 사용)
        labelsToDownload.Clear(); // 다운로드 목록 초기화

        foreach (var labelRef in labelList)
        {
            // AssetLabelReference를 사용하여 다운로드 크기 확인
            var handle = Addressables.GetDownloadSizeAsync(labelRef);

            yield return handle;

            // ⚠️ 개선: 결과 타입을 long과 비교 (decimal.Zero 대신 0)
            if (handle.Result > 0)
            {
                patchSize += handle.Result;
                // 다운로드가 필요한 라벨의 string만 저장
                labelsToDownload.Add(labelRef.labelString);
            }

            Addressables.Release(handle); // 핸들 해제

        }

        // 패치사이즈가 0보다 크면 패치있음
        if (patchSize > 0)
        {
            //Down
            waitMessage.SetActive(false);
            downMessage.SetActive(true);

            sizeInfoText.text = GetFileSize(patchSize);
        }
        else
        {
            downValueText.text = $"다운로드 필요없음";
            downSliders.value = 1f;
            yield return new WaitForSeconds(1f);
            SceneLoadManager.Instance.ChangeScene(1, null, LoadSceneMode.Single);
        }
    }

    private string GetFileSize(long byteCnt)
    {
        string size = "0 Bytes";

        if (byteCnt >= 1073741824.0)
        {
            size = string.Format("{0:##.##} GB", (float)byteCnt / 1073741824.0);
        }
        else if (byteCnt >= 1048576.0)
        {
            size = string.Format("{0:##.##} MB", (float)byteCnt / 1048576.0);
        }
        else if (byteCnt >= 1024.0)
        {
            size = string.Format("{0:##.##} KB", (float)byteCnt / 1024.0);
        }
        else if (byteCnt > 0 && byteCnt < 1024.0)
        {
            size = string.Format("{0} Bytes", byteCnt);
        }

        return size;
    }
    #endregion

    #region DownLoad
    public void OnClickDownButton(string str)
    {
        switch (str)
        {
            case "DownLaod":
                // 다운로드 시작
                StartCoroutine(PatchFiles());
                downMessage.SetActive(false);
                break;

            case "Quit":
                Application.Quit();
                break;
        }
    }

    IEnumerator PatchFiles()
    {
        // 💡 개선 2 해결: patchSize 및 GetDownloadSizeAsync 호출 제거
        // patchSize는 CheckUpdateFile에서 이미 최종적으로 계산되어 있으므로 0으로 초기화하지 않습니다.

        patchDic.Clear(); // 다운로드 딕셔너리 초기화

        // 💡 개선 2 해결: 다운로드가 필요한 라벨 목록(labelsToDownload)만 순회합니다.
        foreach (var labelString in labelsToDownload)
        {
            // string 타입인 labelString을 DownLoadLabel에 전달
            StartCoroutine(DownLoadLabel(labelString));
        }

        yield return CheckDownLoad();
    }

    IEnumerator DownLoadLabel(string label)
    {
        // label은 이제 string 타입이므로, Dictionary 키로 바로 사용 가능
        if (!patchDic.ContainsKey(label))
        {
            patchDic.Add(label, 0);
        }

        var handle = Addressables.DownloadDependenciesAsync(label, false);

        while (!handle.IsDone)
        {
            patchDic[label] = handle.GetDownloadStatus().DownloadedBytes;
            yield return new WaitForEndOfFrame();
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            patchDic[label] = handle.GetDownloadStatus().TotalBytes;
        }
        else
        {
            Debug.LogError($"'{label}' 다운로드 실패: {handle.OperationException}");
            // 실패해도 UI 진행을 멈추지 않기 위해 TotalBytes로 업데이트
            patchDic[label] = handle.GetDownloadStatus().TotalBytes;
        }

        Addressables.Release(handle);
    }

    IEnumerator CheckDownLoad()
    {
        var total = 0f;
        downValueText.text = "0%";

        while (true)
        {
            yield return new WaitForEndOfFrame();
            // total을 계산하기 전에 0으로 초기화해야 합니다.
            total = 0f;

            // 딕셔너리의 값(다운로드된 바이트) 합산
            total += patchDic.Sum(tmp => tmp.Value);

            // 다운로드 진행률 업데이트
            // patchSize가 0일 경우 예외 방지 (if (patchSize > 0) 분기 때문에 사실상 0일 일은 없음)
            if (patchSize > 0)
            {
                downSliders.value = total / patchSize;
            }
            downValueText.text = (int)(downSliders.value * 100) + "%";

            // 다운로드 완료 조건
            if (total >= patchSize) // >= 로 안전하게 처리
            {
                yield return new WaitForSeconds(3f);

                SceneLoadManager.Instance.ChangeScene(1, null, LoadSceneMode.Single);
                break;
            }

            // ⚠️ 불필요한 중복 초기화 및 대기 제거 (이전 코드에서 제거)
        }
    }
    #endregion
}