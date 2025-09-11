using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class CSVtoSOConverter : EditorWindow
{
    private string csvPath = "Assets/01.Sripts/Monster/MonsterData/MonsterData.csv";
    private string outputPath = "Assets/01.Sripts/Monster/MonsterData";
    private string soTypeName = "MonsterSO"; // 원하는 ScriptableObject 클래스 이름

    [MenuItem("Tools/Generic CSV to SO")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CSVtoSOConverter));
    }

    void OnGUI() //보기 편하게 GUI로 표시
    {
        GUILayout.Label("CSV → ScriptableObjects (Generic)", EditorStyles.boldLabel);

        csvPath = EditorGUILayout.TextField("CSV File Path", csvPath);
        outputPath = EditorGUILayout.TextField("Output Folder", outputPath);
        soTypeName = EditorGUILayout.TextField("SO Class Name", soTypeName);

        if (GUILayout.Button("Convert"))
        {
            ConvertCSVtoSO(csvPath, outputPath, soTypeName);
        }
    }

    public static void ConvertCSVtoSO(string csvPath, string outputPath, string soTypeName)
    {
        if (!File.Exists(csvPath))
        {
            Debug.LogError("CSV file not found at path: " + csvPath); //없을시에는 에러 표시
            return;
        }

        // SO 타입 가져오기
        Type soType = Type.GetType(soTypeName);
        if (soType == null || !typeof(ScriptableObject).IsAssignableFrom(soType))
        {
            Debug.LogError("SO Type not found or not a ScriptableObject: " + soTypeName);
            return;
        }

        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath); //출력 파일이 없다면 생성

        string[] lines = File.ReadAllLines(csvPath); //CSV 모든 줄 읽기
        if (lines.Length <= 1)
        {
            Debug.LogError("CSV has no data rows.");
            return;
        }

        //첫줄은 해더
        string[] headers = lines[0].Split(',');

        // 데이터 줄 반복
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue; // 빈 줄인 경우에는 건너뜀
            string[] values = lines[i].Split(',');

            // 새로운 SO 인스턴스 생성
            ScriptableObject soInstance = ScriptableObject.CreateInstance(soType);

            // CSV 열로 통해 SO 필드 매핑
            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                string header = headers[j];
                string value = values[j];

                // 필드 정보 가져오기
                FieldInfo field = soType.GetField(header, BindingFlags.Public | BindingFlags.Instance);
                if (field == null) continue;

                try
                {
                    // 필드 타입별로 값 변환
                    if (field.FieldType == typeof(int))
                        field.SetValue(soInstance, int.Parse(value));
                    else if (field.FieldType == typeof(float))
                        field.SetValue(soInstance, float.Parse(value.Replace("f", "")));
                    else if (field.FieldType == typeof(string))
                        field.SetValue(soInstance, value);
                    else if (field.FieldType == typeof(List<int>))
                    {
                        var list = new List<int>();
                        if (!string.IsNullOrEmpty(value))
                            list.AddRange(value.Split(';').Select(s => int.Parse(s)));
                        field.SetValue(soInstance, list);
                    }
                    else if (field.FieldType == typeof(List<string>))
                    {
                        var list = new List<string>();
                        if (!string.IsNullOrEmpty(value))
                            list.AddRange(value.Split(';'));
                        field.SetValue(soInstance, list);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing field '{header}' with value '{value}': {e.Message}");
                }
            }

            //생성된 SO 이름 결졍
            string assetName = soInstance.name;
            if (string.IsNullOrEmpty(assetName))
                assetName = $"SO_{i}";

            string assetPath = $"{outputPath}/{assetName}.asset";
            AssetDatabase.CreateAsset(soInstance, assetPath);
        }

        // 저장 및 갱신
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("CSV → SO 변환 완료!");
    }
}
