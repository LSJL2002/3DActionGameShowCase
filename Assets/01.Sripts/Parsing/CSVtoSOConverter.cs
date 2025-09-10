using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CSVtoSOConverter : EditorWindow
{
    private string csvPath = "Assets/01.Sripts/Monster/MonsterData/MonsterData.csv";
    private string outputPath = "Assets/01.Sripts/Monster/MonsterData";

    [MenuItem("Tools/CSV to SO")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CSVtoSOConverter));
    }

    void OnGUI()
    {
        GUILayout.Label("CSV → ScriptableObjects", EditorStyles.boldLabel);

        csvPath = EditorGUILayout.TextField("CSV File Path", csvPath);
        outputPath = EditorGUILayout.TextField("Output Folder", outputPath);

        if (GUILayout.Button("Convert"))
        {
            ConvertCSVtoSO(csvPath, outputPath);
        }
    }

    public static void ConvertCSVtoSO(string csvPath, string outputPath)
    {
        if (!File.Exists(csvPath))
        {
            Debug.LogError("CSV file not found at path: " + csvPath);
            return;
        }

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        string[] lines = File.ReadAllLines(csvPath); // CSV 모든 줄 읽어오기
        if (lines.Length <= 1)
        {
            Debug.LogError("CSV has no data rows.");
            return;
        }

        string[] headers = lines[0].Split(','); // 첫 줄은 헤더(컬럼 이름)

        for (int i = 1; i < lines.Length; i++) // 1번째 줄부터 데이터 시작
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue; // 빈 줄은 스킵

            string[] values = lines[i].Split(',');

            MonsterSO monster = ScriptableObject.CreateInstance<MonsterSO>(); // 새로운 MonsterSO 생성

            // CSV 데이터 → SO에 매핑
            monster.id = int.Parse(values[0]);
            monster.monsterName = values[1];
            monster.maxHp = int.Parse(values[2]);
            monster.maxMp = int.Parse(values[3]);
            monster.attackPower = int.Parse(values[4]);
            monster.defense = int.Parse(values[5]);
            monster.attackSpeed = float.Parse(values[6]);

            // 상태이상 효과 (여러 개일 경우 ';'로 구분)
            monster.statusEffect = new List<string>();
            if (!string.IsNullOrEmpty(values[7]))
                monster.statusEffect.AddRange(values[7].Split(';'));

            monster.moveSpeed = float.Parse(values[8].Replace("f", ""));

            monster.equipWeaponId = int.Parse(values[9]);
            monster.equipArmorId = int.Parse(values[10]);
            monster.equipAccId = int.Parse(values[11]);

            // dropItems (list)
            monster.dropItem = new List<int>();
            if (!string.IsNullOrEmpty(values[12]))
            {
                foreach (var item in values[12].Split(';'))
                {
                    if (int.TryParse(item, out int id))
                        monster.dropItem.Add(id);
                }
            }

            monster.detectRange = int.Parse(values[13]);
            monster.attackRange = int.Parse(values[14]);

            // useSkills (list) 나중에 추가
            // monster.useSkill = new List<int>();
            // if (values.Length > 15 && !string.IsNullOrEmpty(values[15]))
            // {
            //     foreach (var skill in values[15].Split(';'))
            //     {
            //         if (int.TryParse(skill, out int id))
            //             monster.useSkill.Add(id);
            //     }
            // }

            string assetPath = $"{outputPath}/{monster.id}_{monster.monsterName}.asset";
            AssetDatabase.CreateAsset(monster, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("CSV → SO 변환 완료!");
    }
}
