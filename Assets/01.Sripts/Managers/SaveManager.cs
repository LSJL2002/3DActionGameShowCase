using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[System.Serializable] // 직렬화하여 파일에 저장하거나 네트워크를통해 전송가능
public class SaveData
{
    public int LastClearStage = 0;
    public List<int> Inventory = new List<int>();
    public string BuildVersion; // 현재 게임 빌드 버전(어드레서블도 동일)
}

public class SaveManager : Singleton<SaveManager>
{
    public SaveData playerData = new();

    private string path;
    private string fileName = "/save.json";
    private string keyWord = "projectEight";

    // 볼륨 설정을 저장할 PlayerPrefs 키
    private const string Master_VOLUME_KEY = "MasterVolume";
    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    protected override void Awake()
    {
        base.Awake(); // 반드시 호출
        path = Application.persistentDataPath + fileName;
        Debug.Log("[SaveManager] Path initialized: " + path);
    }

    private void EnsurePath()
    {
        if (string.IsNullOrEmpty(path))
        {
            path = Application.persistentDataPath + fileName;
            Debug.LogWarning("[SaveManager] Path was null, reinitialized to: " + path);
        }
    }

    public void SaveData()
    {
        EnsurePath();
        string data = JsonUtility.ToJson(playerData);
        File.WriteAllText(path, EncryptAndDecrypt(data));
        Debug.Log($"{data}를 저장했습니다");
    }

    public bool LoadData()
    {
        EnsurePath();

        if (!File.Exists(path))
        {
            Debug.LogWarning("Save file not found, creating new one...");
            playerData = new SaveData();
            SaveData();
            return false;
        }

        string data = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<SaveData>(EncryptAndDecrypt(data));
        Debug.Log($"{EncryptAndDecrypt(data)}를 불러왔습니다");
        return true;
    }

    private string EncryptAndDecrypt(string data)
    {
        string result = "";
        for (int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ keyWord[i % keyWord.Length]);
        }
        return result;
    }

    public void AddStageData(int stageId) => playerData.LastClearStage = stageId;
    public void AddItemData(int itemId) => playerData.Inventory.Add(itemId);

    // PlayerPrefs에 설정 저장하는 함수
    public void SaveVolumes()
    {
        PlayerPrefs.SetFloat(Master_VOLUME_KEY, AudioManager.Instance.MasterVolume);
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, AudioManager.Instance.BgmVolume);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, AudioManager.Instance.SfxVolume);
        PlayerPrefs.Save();
    }

    // PlayerPrefs에서 설정 가져오는 함수
    public void LoadVolumes(int value)
    {
        switch (value)
        {
            // 기본값 세팅
            case 0:
                AudioManager.Instance.SetMasterVolume(AudioManager.Instance.DefaultVolume);
                AudioManager.Instance.SetBgmVolume(AudioManager.Instance.DefaultVolume);
                AudioManager.Instance.SetSfxVolume(AudioManager.Instance.DefaultVolume);
                break;

            // 저장값 세팅
            case 1:
                AudioManager.Instance.SetMasterVolume(PlayerPrefs.GetFloat(Master_VOLUME_KEY, 0.5f));
                AudioManager.Instance.SetBgmVolume(PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 0.5f));
                AudioManager.Instance.SetSfxVolume(PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.5f));
                break;
        }
    }
}
