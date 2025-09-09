using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JSONtoMultipleSO : EditorWindow
{
    [MenuItem("Tools/JSON to Multiple Monster SOs")]
    public static void ConvertJSONtoMultipleSO()
    {
        string jsonPath = Application.dataPath + "/01.Sripts/Monster/MonsterData/MonsterData.json";
        string folderPath = "Assets/01.Sripts/Monster/MonsterData/SOs/"; //test

        if (!File.Exists(jsonPath))
        {
            Debug.LogError("JSON 파일을 찾을 수 없습니다: " + jsonPath);
            return;
        }

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string json = File.ReadAllText(jsonPath);
        MonsterListWrapper wrapper = JsonUtility.FromJson<MonsterListWrapper>(json);

        if (wrapper == null || wrapper.monsters == null || wrapper.monsters.Length == 0)
        {
            Debug.LogError("JSON 파싱 실패 또는 monsters 배열이 없습니다.");
            return;
        }

        foreach (var m in wrapper.monsters)
        {
            MonsterDataSO monsterSO = ScriptableObject.CreateInstance<MonsterDataSO>();

            monsterSO.ID = m.ID;
            monsterSO.이름 = m.이름;
            monsterSO.최대체력 = m.최대체력;
            monsterSO.최대엠피 = m.최대엠피;
            monsterSO.공격력 = m.공격력;
            monsterSO.방어력 = m.방어력;
            monsterSO.공격속도 = Mathf.Round(m.공격속도 * 100f) / 100f;
            monsterSO.상태이상 = m.상태이상 ?? new List<string>();
            monsterSO.이동속도 = Mathf.Round(m.이동속도 * 100f) / 100f;
            monsterSO.장비한무기 = m.장비한무기;
            monsterSO.장비한방어구 = m.장비한방어구;
            monsterSO.장비한악세사리 = m.장비한악세사리;
            monsterSO.드랍아이템 = m.드랍아이템 ?? new List<int>();
            monsterSO.인식범위 = m.인식범위;
            monsterSO.공격범위 = m.공격범위;

            string soFilePath = folderPath + m.이름 + ".asset";
            AssetDatabase.CreateAsset(monsterSO, soFilePath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("JSON → 개별 Monster ScriptableObject 변환 완료!");
    }

    [System.Serializable]
    private class MonsterListWrapper
    {
        public MonsterDataJSON[] monsters;
    }

    [System.Serializable]
    private class MonsterDataJSON
    {
        public string ID;
        public string 이름;
        public int 최대체력;
        public int 최대엠피;
        public int 공격력;
        public int 방어력;
        public float 공격속도;
        public List<string> 상태이상;
        public float 이동속도;
        public int 장비한무기;
        public int 장비한방어구;
        public int 장비한악세사리;
        public List<int> 드랍아이템;
        public int 인식범위;
        public int 공격범위;
    }
}
