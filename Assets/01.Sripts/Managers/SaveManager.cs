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

    private string path; //파일경로
    private string fileName = "/save.json"; // path/fileName.json을 위해 앞에 /추가
    private string keyWord = "projectEight";

    protected override void Start()
    {
        path = Application.persistentDataPath + fileName;
        Debug.Log(path);
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(playerData); //Json형식의 문자열로 변환

        File.WriteAllText(path, EncryptAndDecrypt(data)); // File사용위해 using System.IO 필요 (경로, 내용)
        Debug.Log($"{data}를 저장했습니다");
    }

    public bool LoadData()
    {
        if (!File.Exists(path)) // 파일이 존재하지 않으면
        {
            Debug.LogWarning("Save file not found, creating new one...");
            playerData = new PlayerDatas(); // 기본값으로 초기화
            SaveData(); // 새로 저장
            return false; // false 리턴
        }

        string data = File.ReadAllText(path);

        playerData = JsonUtility.FromJson<PlayerDatas>(EncryptAndDecrypt(data));
        Debug.Log($"{data}를 불러왔습니다");
        return true; // true리턴               <-불러올데이터가있는지 확인가능
    }

    private string EncryptAndDecrypt(string data) // 암호화 및 복호화
    {
        string result = "";

        for (int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ keyWord[i % keyWord.Length]);
        }    //i를 KeyWord의 길이로 나눈 나머지를 구하여 xor연산(^)을 해줌 같을땐 0 다르면 1
        return result;
    }


    public void AddStageData(int stageId)
    {
        playerData.LastClearStage = stageId;
    }

    public void AddItemData(int itemId)
    {
        playerData.Inventory.Add(itemId);
    }


}
