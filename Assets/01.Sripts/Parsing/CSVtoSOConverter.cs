using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class CSVtoSOConverter : EditorWindow
{
    private string csvFilePath = "";
    private int selectedTypeIndex = 0;

    private CSVtoSOSettings settings;
    private const string SETTINGS_PATH = "Assets/Editor/CSVtoSOSettings.asset";

    [MenuItem("Tools/Generic CSV to SO")]
    public static void ShowWindow()
    {
        GetWindow<CSVtoSOConverter>("CSV to SO Converter");
    }

    void OnEnable()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        settings = AssetDatabase.LoadAssetAtPath<CSVtoSOSettings>(SETTINGS_PATH);
        if (settings == null)
        {
            string directoryPath = Path.GetDirectoryName(SETTINGS_PATH);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Debug.Log($"Created directory: {directoryPath}");
            }
            settings = CreateInstance<CSVtoSOSettings>();
            AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    void OnGUI()
    {
        GUILayout.Label("CSV → ScriptableObjects (Generic)", EditorStyles.boldLabel);

        // CSV 파일 드래그 앤 드롭 영역
        Event currentEvent = Event.current;
        Rect csvDropArea = EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Height(50));
        // 오류가 발생했던 부분을 아래와 같이 수정
        GUILayout.Label("Drag & Drop CSV File Here", new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter });
        EditorGUILayout.EndVertical();

        HandleCSVDragAndDrop(currentEvent, csvDropArea);

        EditorGUILayout.TextField("Selected CSV Path", csvFilePath, GUI.skin.textField);

        Rect soDropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(soDropArea, "Drag & Drop ScriptableObject Script Here");
        HandleSODragAndDrop(currentEvent, soDropArea);

        GUILayout.Label("Registered ScriptableObject Types:", EditorStyles.boldLabel);
        if (settings.soTypeNames.Count > 0)
        {
            string[] typeNames = settings.soTypeNames.ToArray();
            selectedTypeIndex = EditorGUILayout.Popup("Select SO Class", selectedTypeIndex, typeNames);

            if (GUILayout.Button("Delete Selected"))
            {
                if (EditorUtility.DisplayDialog("Confirm Deletion", "Are you sure you want to delete the selected ScriptableObject type?", "Delete", "Cancel"))
                {
                    settings.soTypeNames.RemoveAt(selectedTypeIndex);
                    selectedTypeIndex = Mathf.Clamp(selectedTypeIndex, 0, settings.soTypeNames.Count - 1);
                    SaveSettings();
                }
            }
        }
        else

        {
            EditorGUILayout.LabelField("No ScriptableObject types added yet.");
        }

        if (GUILayout.Button("Convert"))
        {
            if (!string.IsNullOrEmpty(csvFilePath) && settings.soTypeNames.Count > 0)
            {
                string soTypeName = settings.soTypeNames[selectedTypeIndex];
                ConvertCSVtoSO(csvFilePath, Path.GetDirectoryName(csvFilePath), soTypeName);
            }
            else
            {
                Debug.LogError("CSV file path or ScriptableObject type is not selected.");
            }
        }
    }

    private void HandleCSVDragAndDrop(Event currentEvent, Rect dropArea)
    {
        if (dropArea.Contains(currentEvent.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (currentEvent.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (var draggedObject in DragAndDrop.objectReferences)
                {
                    string path = AssetDatabase.GetAssetPath(draggedObject);
                    if (path.EndsWith(".csv"))
                    {
                        csvFilePath = path;
                        break;
                    }
                }
            }
        }
    }

    private void HandleSODragAndDrop(Event currentEvent, Rect dropArea)
    {
        if (dropArea.Contains(currentEvent.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (currentEvent.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (var draggedObject in DragAndDrop.objectReferences)
                {
                    MonoScript monoScript = draggedObject as MonoScript;
                    if (monoScript != null)
                    {
                        Type scriptType = monoScript.GetClass();
                        if (scriptType != null && typeof(ScriptableObject).IsAssignableFrom(scriptType) && !settings.soTypeNames.Contains(scriptType.FullName))
                        {
                            settings.soTypeNames.Add(scriptType.FullName);
                            SaveSettings();
                        }
                    }
                }
            }
        }
    }

    static char DetectDelimiter(string line)
    {
        if (line.Contains('\t')) return '\t';
        if (line.Contains(';')) return ';';
        return ','; // 기본
    }

    // 2) CSV 한 줄을 따옴표 규칙 지키며 split
    static string[] SplitCsvLine(string line, char delimiter)
    {
        var list = new List<string>();
        var sb = new System.Text.StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == '"')
            {
                // "" -> 내부 따옴표 이스케이프
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    sb.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == delimiter && !inQuotes)
            {
                list.Add(sb.ToString());
                sb.Clear();
            }
            else
            {
                sb.Append(c);
            }
        }
        list.Add(sb.ToString());
        return list.ToArray();
    }

    // 3) 타입줄인지 판단 (int,string,List<int> 등만 있으면 true)
    static bool IsTypeRow(string[] tokens)
    {
        bool IsTypeToken(string t)
        {
            t = t.Trim().Trim('"', '\'');
            if (t.Equals("int", StringComparison.OrdinalIgnoreCase)) return true;
            if (t.Equals("float", StringComparison.OrdinalIgnoreCase)) return true;
            if (t.Equals("string", StringComparison.OrdinalIgnoreCase)) return true;
            if (t.StartsWith("List<", StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
        if (tokens.Length == 0) return false;
        return tokens.All(IsTypeToken);
    }

    private void SaveSettings()
    {
        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void ConvertCSVtoSO(string csvPath, string outputPath, string soTypeName)
    {
        if (!File.Exists(csvPath))
        {
            Debug.LogError("CSV file not found at path: " + csvPath);
            return;
        }

        Type soType = Type.GetType(soTypeName);
        if (soType == null || !typeof(ScriptableObject).IsAssignableFrom(soType))
        {
            Debug.LogError("SO Type not found or not a ScriptableObject: " + soTypeName);
            return;
        }

        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        string[] lines = File.ReadAllLines(csvPath);
        if (lines.Length <= 1)
        {
            Debug.LogError("CSV has no data rows.");
            return;
        }

        // 구분자 자동 감지
        char delim = DetectDelimiter(lines[0]);

        // 헤더 (0행)
        string[] headers = SplitCsvLine(lines[0], delim)
                            .Select(h => h.Trim().Trim('"', '\''))
                            .ToArray();

        // 타입줄(1행)이라면 건너뛰기
        int dataStart = 1;
        if (lines.Length > 1 && IsTypeRow(
                SplitCsvLine(lines[1], delim).Select(t => t.Trim()).ToArray()))
        {
            dataStart = 2;
        }

        // 데이터
        for (int i = dataStart; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = SplitCsvLine(lines[i], delim);

            var soInstance = ScriptableObject.CreateInstance(soType);

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                string header = headers[j];
                string value = values[j].Trim().Trim('"', '\'');

                FieldInfo field = soType.GetField(header,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (field == null) continue;

                try
                {
                    if (field.FieldType == typeof(int))
                    {
                        field.SetValue(soInstance,
                            string.IsNullOrWhiteSpace(value) ? 0 : int.Parse(value));
                    }
                    else if (field.FieldType == typeof(float))
                    {
                        field.SetValue(soInstance,
                            string.IsNullOrWhiteSpace(value) ? 0f :
                            float.Parse(value.Replace("f", ""),
                                        System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else if (field.FieldType == typeof(string))
                    {
                        field.SetValue(soInstance, value);
                    }
                    else if (field.FieldType == typeof(List<int>))
                    {
                        var list = new List<int>();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            list.AddRange(value
                                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => int.Parse(s.Trim().Trim('"', '\''))));
                        }
                        field.SetValue(soInstance, list);
                    }
                    else if (field.FieldType == typeof(List<string>))
                    {
                        var list = new List<string>();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            list.AddRange(value
                                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => s.Trim().Trim('"', '\'')));
                        }
                        field.SetValue(soInstance, list);
                    }
                    else if (field.FieldType.IsEnum)
                    {
                        var enumValue = Enum.Parse(field.FieldType, value, true);
                        field.SetValue(soInstance, enumValue);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing field '{header}' with value '{value}': {e.Message}");
                }
            }

            string assetName = (values.Length > 1 ? values[1] : $"SO_{i}") ?? $"SO_{i}";
            string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{outputPath}/{assetName}.asset");
            AssetDatabase.CreateAsset(soInstance, assetPath);
        }

    }
}