using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[System.Serializable] // 직렬화하여 파일에 저장하거나 네트워크를통해 전송가능
public class PlayerDatas
{
    public int LastClearStage = 0;
    public List<int> Inventory = new List<int>();
}

public class SaveManager : Singleton<SaveManager>
{
    public PlayerDatas playerData = new();

    private string path;
    private string fileName = "/save.json";
    private string keyWord = "projectEight";

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
            playerData = new PlayerDatas();
            SaveData();
            return false;
        }

        string data = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<PlayerDatas>(EncryptAndDecrypt(data));
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
}
