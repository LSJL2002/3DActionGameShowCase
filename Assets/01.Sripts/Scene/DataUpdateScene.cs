using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataUpdateScene : SceneBase
{
    [Header("UI")]
    [SerializeField] private GameObject waitMessage;
    [SerializeField] private GameObject UpdateMessage;
    [SerializeField] private Slider downSliders;
    [SerializeField] private TextMeshProUGUI waitMessageText;
    [SerializeField] private TextMeshProUGUI versionCheckText;
    [SerializeField] private TextMeshProUGUI sizeInfoText;
    [SerializeField] private TextMeshProUGUI downValueText;

    // 최초 실행시 다운로드할 라벨 List 설정 (Inspector에서 설정)
    [Header("Label")]
    [SerializeField] private List<AssetLabelReference> labelList = new List<AssetLabelReference>();

    // 각 라벨별 다운로드 용량을 저장할 딕셔너리 (라벨명, 다운로드용량)
    private Dictionary<string, long> labelToPatchDic = new Dictionary<string, long>();
    // 다운로드가 필요한 라벨목록을 string으로 저장할 리스트 추가 (Dictionary 키로 사용하기 위함)
    private List<string> labelsToDownload = new List<string>();

    private long patchSize;
    private float currentVersion;

    protected override void Awake()
    {
        base.Awake();

        waitMessage.SetActive(true);
        UpdateMessage.SetActive(false);
        downSliders.gameObject.SetActive(false);

        StartCoroutine(InitAddressableAndCheck());
    }

    // Addressables 초기화 및 카탈로그 업데이트 체크
    IEnumerator InitAddressableAndCheck()
    {
        // Addressables 초기화
        var init = Addressables.InitializeAsync();
        yield return init;

        // 원격 카탈로그 체크 및 갱신
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

        yield return CheckUpdateFile();
    }

    #region Check DownLoad
    // 다운로드할 파일이 있는지 체크
    IEnumerator CheckUpdateFile()
    {
        patchSize = 0;
        labelsToDownload.Clear(); // 새 다운로드 목록 작성 전 초기화

        // 설정 된 AssetLabelReference를 순회하여 전체 다운로드 크기 확인
        foreach (var labelRef in labelList)
        {
            var handle = Addressables.GetDownloadSizeAsync(labelRef);

            yield return handle;

            if (handle.Result > 0)
            {
                // 다운로드가 필요한 용량이 있으면 패치사이즈에 추가
                patchSize += handle.Result;
                // 다운로드가 필요한 라벨의 string만 저장
                labelsToDownload.Add(labelRef.labelString);
            }
            Addressables.Release(handle); // 핸들 해제
        }

        // 패치사이즈가 0보다 크면 패치있음
        if (patchSize > 0)
        {
            waitMessage.SetActive(false);
            UpdateMessage.SetActive(true);
            versionCheckText.text = $"현재 {currentVersion} / 최신 {Application.version}";
            sizeInfoText.text = GetFileSize(patchSize);
        }
        else
        {
            waitMessageText.text = $"최신 버전입니다.<br>현재 ver.{currentVersion}<br>잠시만 기다려주세요.";

            yield return new WaitForSeconds(3f);
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
                UpdateMessage.SetActive(false);
                downSliders.gameObject.SetActive(true);
                break;

            case "Quit":
                Application.Quit();
                break;
        }
    }

    IEnumerator PatchFiles()
    {
        labelToPatchDic.Clear(); // 다운로드 딕셔너리 초기화

        // 다운로드가 필요한 라벨 목록(labelsToDownload)만 순회
        foreach (var labelString in labelsToDownload)
        {
            // string 타입인 labelString을 DownLoadLabel에 전달
            StartCoroutine(DownLoadLabel(labelString));
        }

        yield return CheckDownLoad();
    }

    IEnumerator DownLoadLabel(string label)
    {
        if (!labelToPatchDic.ContainsKey(label))
        {
            labelToPatchDic.Add(label, 0);
        }

        var handle = Addressables.DownloadDependenciesAsync(label, false);

        while (!handle.IsDone)
        {
            labelToPatchDic[label] = handle.GetDownloadStatus().DownloadedBytes;
            yield return new WaitForEndOfFrame();
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            labelToPatchDic[label] = handle.GetDownloadStatus().TotalBytes;
        }
        else
        {
            Debug.LogError($"'{label}' 다운로드 실패: {handle.OperationException}");
            // 실패해도 UI 진행을 멈추지 않기 위해 TotalBytes로 업데이트
            labelToPatchDic[label] = handle.GetDownloadStatus().TotalBytes;
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
            
            total = 0f;

            // 딕셔너리의 값(다운로드된 바이트) 합산
            total += labelToPatchDic.Sum(tmp => tmp.Value);

            // 다운로드 진행률 업데이트
            if (patchSize > 0)
            {
                downSliders.value = total / patchSize;
            }
            downValueText.text = (int)(downSliders.value * 100) + "%";

            // 다운로드 완료 조건
            if (total >= patchSize)
            {
                currentVersion = float.Parse(Application.version); // 현재 버전을 float로 변환하여 저장
                yield return new WaitForSeconds(3f); // 캐시 정리를 위한 일정시간 대기

                SceneLoadManager.Instance.ChangeScene(1, null, LoadSceneMode.Single); // 목표 씬으로 Single 전환
                break;
            }
        }
    }
    #endregion
}