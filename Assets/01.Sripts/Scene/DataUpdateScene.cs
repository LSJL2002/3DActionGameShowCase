using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
    [SerializeField] AssetLabelReference label;

    private long patchSize;
    private Dictionary<string, long> patchMap = new Dictionary<string, long>();

    protected override void Awake()
    {
        waitMessage.SetActive(true);
        downMessage.SetActive(false);

        StartCoroutine(InitAddressable());
        StartCoroutine(CheckUpdateFile());
    }

    IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
    }

    #region Check DownLoad
    IEnumerator CheckUpdateFile()
    {
        var labels = new List<string> { label.labelString };

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
            SceneLoadManager.Instance.LoadScene(1);
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
        var labels = new List<string> { label.labelString };

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
                SceneLoadManager.Instance.LoadScene(1);
                break;
            }

            total = 0f;
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
