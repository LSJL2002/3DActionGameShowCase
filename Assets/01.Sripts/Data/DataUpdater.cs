using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataUpdater : MonoBehaviour
{
    // config.json 파일이 있는 서버 URL (정적으로 관리)
    private const string CONFIG_URL = "https://s3.ap-southeast-2.amazonaws.com/project8.addressable/VersionConfig.json";

    [Header("UI")]
    [SerializeField] private GameObject waitMessage;
    [SerializeField] private GameObject UpdateMessage;
    [SerializeField] private GameObject QuitMessage;
    [SerializeField] private Slider downSliders;
    [SerializeField] private TextMeshProUGUI waitMessageText;
    [SerializeField] private TextMeshProUGUI sizeInfoText;
    [SerializeField] private TextMeshProUGUI versionCheckText;
    [SerializeField] private TextMeshProUGUI downValueText;

    // 최초 실행시 다운로드할 라벨 List 설정 (Inspector에서 설정)
    [Header("Label")]
    [SerializeField] private List<AssetLabelReference> labelList = new List<AssetLabelReference>();

    // 각 라벨별 다운로드 용량을 저장할 딕셔너리 (라벨명, 다운로드용량)
    private Dictionary<string, long> labelToPatchDic = new Dictionary<string, long>();
    // 다운로드가 필요한 라벨목록을 string으로 저장할 리스트 추가 (Dictionary 키로 사용하기 위함)
    private List<string> labelsToDownload = new List<string>();

    private long patchSize;
    private string currentVersion; // 로컬에 저장된 현재 버전
    private string latestVersion; // 서버에서 가져온 최신 버전

    // Analytics boolean
    private bool step0 = false;
    private bool step20 = false;
    private bool step40 = false;
    private bool step60 = false;
    private bool step80 = false;

    public VersionConfigData VersionConfig { get; private set; } = new VersionConfigData();

    public void Awake()
    {
        waitMessage.SetActive(true);
        UpdateMessage.SetActive(false);
        downSliders.gameObject.SetActive(false);

        StartCoroutine(InitAddressableAndCheck());
    }

    #region Initialize Addressables & Check Catalog Update
    // Addressables 초기화 및 카탈로그 업데이트 체크
    IEnumerator InitAddressableAndCheck()
    {
        // VersionConfig.json 다운로드
        yield return LoadRemoteConfig().ToCoroutine();

        // Addressables 런타임 경로 재정의 (VersionConfig.json에서 CDN_URL을 읽어와 적용)
        string cdnUrl = VersionConfig.CDN_URL;

        if (!string.IsNullOrEmpty(cdnUrl))
        {
            // VersionConfig에 경로 정보가 있다면 어드레서블 프로필의 리모트 패스를 덮어쓰기(override) 함
            // {0}은 플랫폼 빌드 타겟 등을 나타내는 플레이스 홀더
            string remotePathOverride = $"{cdnUrl}{{0}}";

            Addressables.InternalIdTransformFunc = (IResourceLocation location) =>
            {
                string internalId = location.InternalId;

                if (internalId.Contains("{AddressablesRuntimePath}"))
                {
                    internalId = internalId.Replace("{AddressablesRuntimePath}", remotePathOverride);
                }

                return internalId;
            };
            Debug.Log($"Addressables Remote Path Override set to: {remotePathOverride}");
        }

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
    #endregion Initialize Addressables & Check Catalog Update

    #region Check File Update
    // 다운로드할 파일이 있는지 체크
    IEnumerator CheckUpdateFile()
    {
        patchSize = 0;
        labelsToDownload.Clear(); // 새 다운로드 목록 작성 전 초기화

        // 현재 로컬 버전과 서버 최신 버전 로드
        currentVersion = SaveManager.Instance.LoadBuildVersionPlayerPrefs(); // PlayerPrefs에서 저장된 버전 가져옴
        latestVersion = VersionConfig.LatestContentVersion; // config.json에서 최신 버전 가져옴

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
            // 강제 앱 업데이트 체크 (Optional, MinAppVersion이 현재 Application.version보다 높으면 처리)
            if (VersionConfig.MinimumAppVersion.CompareTo(Application.version) > 0)
            {
                waitMessageText.text = $"앱의 버전이 너무 낮습니다.<br>최신 버전을 다시 설치해주세요.";
                yield break; // 더 이상 진행하지 않고 종료
            }

            waitMessage.SetActive(false);
            UpdateMessage.SetActive(true);

            // UI 표시 시 서버에서 가져온 최신 버전을 사용
            versionCheckText.text = $"현재 Version:{currentVersion} / 최신 Version:{latestVersion}";
            sizeInfoText.text = GetFileSize(patchSize);
        }
        else
        {
            waitMessageText.text = $"최신 버전입니다.<br>현재 Version:{currentVersion}<br>잠시만 기다려주세요.";

            // 패치 사이즈가 0이면 최신 버전을 로컬에 저장 (패치 완료와 동일하게 처리)
            SaveManager.Instance.SavePlayerPrefs(SaveManager.PlayerPrefsSaveType.BuildVersion, latestVersion);

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
    #endregion Check File Update

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

        yield return UpdateDownLoad();
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

    IEnumerator UpdateDownLoad()
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
            int percentage = (int)(downSliders.value * 100);
            downValueText.text = percentage + "%";

            // 퍼널 스텝 전송 로직
            if (!step20 && percentage >= 20) 
            { 
                step20 = true;
                AnalyticsManager.SendFunnelStep("2"); 
            }
            else if (!step40 && percentage >= 40) 
            {
                step40 = true;
                AnalyticsManager.SendFunnelStep("3"); 
            }
            else if (!step60 && percentage >= 60) 
            {
                step60 = true;
                AnalyticsManager.SendFunnelStep("4"); 
            }
            else if (!step80 && percentage >= 80) 
            { 
                step80 = true;
                AnalyticsManager.SendFunnelStep("5"); 
            }
            else if (!step0) 
            { 
                step0 = true;
                AnalyticsManager.SendFunnelStep("1"); 
            }

            // 다운로드 완료 조건
            if (total >= patchSize)
            {
                AnalyticsManager.SendFunnelStep("6");

                SaveManager.Instance.SavePlayerPrefs(SaveManager.PlayerPrefsSaveType.BuildVersion, latestVersion); // 다운로드한 버전정보 저장
                QuitMessage.SetActive(true);
                yield return new WaitForSeconds(5f); // 캐시 정리를 위한 일정시간 대기
                Application.Quit();
                //SceneLoadManager.Instance.ChangeScene(1, null, LoadSceneMode.Single); // 목표 씬으로 Single 전환

                break;
            }
        }
    }
    #endregion DownLoad

    #region Parsing Version Config
    // config.json을 서버에서 다운로드하고 파싱하는 함수
    public async UniTask<bool> LoadRemoteConfig()
    {
        using var request = UnityWebRequest.Get(CONFIG_URL);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                VersionConfig = JsonUtility.FromJson<VersionConfigData>(request.downloadHandler.text);
                Debug.Log($"[Config] Loaded Version: {VersionConfig.LatestContentVersion}, CDN: {VersionConfig.CDN_URL}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Config] JSON parsing error: {e.Message}");
                return false;
            }
        }
        else
        {
            Debug.LogError($"[Config] Download failed: {request.error}");
            // 실패 시 로컬 기본값(null)을 사용하거나 예외 처리 필요
            return false;
        }
    }
    #endregion Parsing Version Config
}