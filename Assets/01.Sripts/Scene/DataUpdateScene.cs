using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
    [SerializeField] AssetLabelReference defaultLabel;
    [SerializeField] AssetLabelReference uiLabel;

    private long patchSize;
    private Dictionary<string, long> patchMap = new Dictionary<string, long>();

    protected override void Awake()
    {
        base.Awake();

        waitMessage.SetActive(true);
        downMessage.SetActive(false);

        StartCoroutine(InitAddressable());
        StartCoroutine(CheckUpdateFile());
    }

    IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
        // 1) 원격 카탈로그 체크
        var check = Addressables.CheckForCatalogUpdates(false);
        yield return check;
        if (check.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded
            && check.Result != null && check.Result.Count > 0)
        {
            // 2) 카탈로그 갱신
            var update = Addressables.UpdateCatalogs(check.Result);
            yield return update;
            Debug.Log($"Catalogs updated: {string.Join(", ", check.Result)}");
        }
        else
        {
            Debug.Log("No catalog updates.");
        }
    }

    #region Check DownLoad
    IEnumerator CheckUpdateFile()
    {
        var labels = new List<string> { defaultLabel.labelString, uiLabel.labelString };

        patchSize = default;

        foreach (var label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);
            
            yield return handle;

            patchSize += handle.Result;
        }

        // 패치사이즈가 0보다 크면 패치있음
        if (patchSize > decimal.Zero)
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
            yield return new WaitForSeconds(2f);
            SceneLoadManager.Instance.ChangeScene(1, null, LoadSceneMode.Single);
        }
    }

    private string GetFileSize(long byteCnt)
    {
        string size = "0 Bytes";

        if(byteCnt >= 1073741824.0)
        {
            size = string.Format("{0:##.##} GB", (float)byteCnt / 1073741824.0);
        }
        else if(byteCnt >= 1048576.0)
        {
            size = string.Format("{0:##.##} MB", (float)byteCnt / 1048576.0);
        }
        else if(byteCnt >= 1024.0)
        {
            size = string.Format("{0:##.##} KB", (float)byteCnt / 1024.0);
        }
        else if(byteCnt > 0 && byteCnt < 1024.0)
        {
            size = string.Format("{0} Bytes", byteCnt);
        }

        return size;
    }
    #endregion

    #region DownLoad
    public void OnClickDownButton(string str)
    {
        switch(str)
        {
            case "DownLaod":

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
        var labels = new List<string> { uiLabel.labelString };

        patchSize = default;

        foreach (var label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);

            yield return handle;

            if (handle.Result != decimal.Zero)
            {
                StartCoroutine(DownLoadLabel(label));
            }
        }

        yield return CheckDownLoad();
    }

    IEnumerator DownLoadLabel(string label)
    {
        patchMap.Add(label, 0);

        var handle = Addressables.DownloadDependenciesAsync(label, false);

        while (!handle.IsDone)
        {
            patchMap[label] = handle.GetDownloadStatus().DownloadedBytes;
            yield return new WaitForEndOfFrame();
        }

        patchMap[label] = handle.GetDownloadStatus().TotalBytes;
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
            total += patchMap.Sum(tmp => tmp.Value);

            downSliders.value = total / patchSize;
            downValueText.text = (int)(downSliders.value * 100) + "%";

            if (total == patchSize)
            {
                SceneLoadManager.Instance.ChangeScene(1, null, LoadSceneMode.Single);
                break;
            }

            total = 0f;
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
