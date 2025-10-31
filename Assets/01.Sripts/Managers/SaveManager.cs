using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable] // 직렬화하여 파일에 저장하거나 네트워크를통해 전송가능
public class SaveData
{
    public int round = 0;
    public int LastClearStage = 0;
    public List<InventorySaveData> ConsumableInventory = new();  // 아이템 ID + 수량
    public List<int> SkillInventory = new();                     // ID만
    public List<int> CoreInventory = new();                      // ID만
}

[System.Serializable]
public class InventorySaveData
{
    public int id;
    public int count;

    public InventorySaveData(int id, int count)
    {
        this.id = id;
        this.count = count;
    }
}

public enum eGameMode
{
    None,
    NewGame,
    LoadGame
}

public class SaveManager : Singleton<SaveManager>
{
    public enum PlayerPrefsSaveType
    {
        Volume,
        BuildVersion,
    }
    public eGameMode gameMode; //게임 모드를 저장할 변수

    public SaveData playerData = new();

    public string path { get; private set; }
    private string fileName = "/save.json";
    private string keyWord = "projectEight";

    // PlayerPrefs 키
    private const string Master_VOLUME_KEY = "MasterVolume";
    private const string Bgm_VOLUME_KEY = "BgmVolume";
    private const string Sfx_VOLUME_KEY = "SfxVolume";
    private const string Build_Version_KEY = "BuildVersion";

    protected override void Awake()
    {
        base.Awake(); // 반드시 호출
        path = Application.persistentDataPath + fileName;
        //Debug.Log("[SaveManager] Path initialized: " + path);
    }

    private void EnsurePath()
    {
        if (string.IsNullOrEmpty(path))
        {
            path = Application.persistentDataPath + fileName;
            //Debug.LogWarning("[SaveManager] Path was null, reinitialized to: " + path);
        }
    }

    public void SaveData()
    {
        EnsurePath();
        GetItemDataInList();
        string data = JsonUtility.ToJson(playerData);
        File.WriteAllText(path, EncryptAndDecrypt(data));
        //Debug.Log($"{data}를 저장했습니다");
    }

    public bool LoadData()
    {
        if (SaveManager.Instance.gameMode != eGameMode.LoadGame) return false;
        EnsurePath();

        if (!File.Exists(path))
        {
            //Debug.LogWarning("Save file not found, creating new one...");
            playerData = new SaveData();
            SaveData();
            return false;
        }

        string data = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<SaveData>(EncryptAndDecrypt(data));
        //Debug.Log($"{EncryptAndDecrypt(data)}를 불러왔습니다");
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

    public void GetItemDataInList()
    {
        playerData.ConsumableInventory.Clear();
        playerData.SkillInventory.Clear();
        playerData.CoreInventory.Clear();
        IReadOnlyList<InventoryItem> consumableItems = InventoryManager.Instance.GetConsumableItems();
        IReadOnlyList<InventoryItem> skillItems = InventoryManager.Instance.GetSkillItems();
        IReadOnlyList<InventoryItem> coreItems = InventoryManager.Instance.GetCoreItems();

        // 리스트를 순회하며 아이템 데이터 안의 ID 값을 저장 (소비형)
        foreach (InventoryItem item in consumableItems)
        {
            playerData.ConsumableInventory.Add(new InventorySaveData(item.data.id, item.stackCount));
        }

        // 리스트를 순회하며 아이템 데이터 안의 ID 값을 저장 (스킬카드)
        foreach (InventoryItem item in skillItems)
        {
            playerData.SkillInventory.Add(item.data.id);
        }

        // 리스트를 순회하며 아이템 데이터 안의 ID 값을 저장 (코어)
        foreach (InventoryItem item in coreItems)
        {
            playerData.CoreInventory.Add(item.data.id);
        }
    }


    //public void ResetData()
    //{
    //    playerData = new SaveData(); // 완전 새 데이터로 교체
    //    SaveData();                  // 즉시 저장 (새로 빈 파일 생성)
    //    Debug.Log("[SaveManager] 세이브 데이터 초기화 완료 (NewGame)");
    //}

    public void DeleteSaveFile()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            //Debug.Log("[SaveManager] Save 파일 삭제 완료");
        }
        //ResetData(); // 새 데이터로 다시 저장 (빈 파일 생성)
    }

    public void SetStageData(int stageId) => playerData.LastClearStage = stageId;

    #region PlayerPrefs
    // PlayerPrefs에 설정 저장하는 함수
    public void SavePlayerPrefs(PlayerPrefsSaveType saveType, string value = null)
    {
        switch(saveType)
        {
            case PlayerPrefsSaveType.Volume:
                {
                    PlayerPrefs.SetFloat(Master_VOLUME_KEY, AudioManager.Instance.MasterVolume);
                    PlayerPrefs.SetFloat(Bgm_VOLUME_KEY, AudioManager.Instance.BgmVolume);
                    PlayerPrefs.SetFloat(Sfx_VOLUME_KEY, AudioManager.Instance.SfxVolume);
                    break;
                }

            case PlayerPrefsSaveType.BuildVersion:
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        PlayerPrefs.SetString(Build_Version_KEY, value);
                    }
                    break;
                }
        }
        PlayerPrefs.Save();
    }

    // Volume 설정을 구조체로 정의
    public struct VolumeSettings
    {
        public float MasterVolume;
        public float BgmVolume;
        public float SfxVolume;
    }

    // Volume 구조체를 반환하는 함수
    public VolumeSettings LoadVolumePlayerPrefs() // 반환 타입을 VolumeSettings로 명확히 분리
    {
        return new VolumeSettings
        {
            MasterVolume = PlayerPrefs.GetFloat(Master_VOLUME_KEY, 0.5f),
            BgmVolume = PlayerPrefs.GetFloat(Bgm_VOLUME_KEY, 0.5f),
            SfxVolume = PlayerPrefs.GetFloat(Sfx_VOLUME_KEY, 0.5f)
        };
    }

    public string LoadBuildVersionPlayerPrefs() // 반환 타입을 string으로 명확히 분리
    {
        return PlayerPrefs.GetString(Build_Version_KEY, "Version not found");
    }
    #endregion
}
