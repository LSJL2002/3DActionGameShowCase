using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Reflection;

public class CSVtoJSONConverter : EditorWindow
{
    [MenuItem("Tools/CSV to JSON")]
    public static void ConvertCSVtoJSON()
    {
        string csvPath = Application.dataPath + "/01.Sripts/Monster/MonsterData/MonsterData.csv";
        string jsonPath = Application.dataPath + "/01.Sripts/Monster/MonsterData/MonsterData.json";

        string[] lines = File.ReadAllLines(csvPath);
        if (lines.Length <= 1) return;

        string[] headers = lines[0].Split(',');
        List<MonsterData> monsters = new List<MonsterData>();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            MonsterData monster = new MonsterData();

            for (int j = 0; j < headers.Length; j++)
            {
                string header = headers[j];
                string value = j < values.Length ? values[j] : "";

                FieldInfo field = typeof(MonsterData).GetField(header);
                if (field == null) continue;

                if (field.FieldType == typeof(int))
                    field.SetValue(monster, string.IsNullOrEmpty(value) ? 0 : int.Parse(value));
                else if (field.FieldType == typeof(float))
                    field.SetValue(monster, string.IsNullOrEmpty(value) ? 0f : float.Parse(value));
                else if (field.FieldType == typeof(string))
                    field.SetValue(monster, value);
                else if (field.FieldType == typeof(List<string>))
                    field.SetValue(monster, string.IsNullOrEmpty(value) ? new List<string>() : new List<string>(value.Split(';')));
                else if (field.FieldType == typeof(List<int>))
                {
                    List<int> list = new List<int>();
                    if (!string.IsNullOrEmpty(value))
                        foreach (var part in value.Split(';'))
                            list.Add(int.Parse(part));
                    field.SetValue(monster, list);
                }
            }

            monsters.Add(monster);
        }

        MonsterList wrapper = new MonsterList { monsters = monsters.ToArray() };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(jsonPath, json);

        Debug.Log("CSV → JSON 변환 완료: " + jsonPath);
    }
}
