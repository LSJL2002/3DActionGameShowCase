using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class CSVtoSOConverter : EditorWindow
{
    private string firstCsvFilePath = "";
    private string secondCsvFilePath = "";
    private int selectedSoTypeIndex = 0;

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

        Event currentEvent = Event.current;

        GUILayout.Label("Drag & Drop First CSV File Here", EditorStyles.boldLabel);
        Rect firstCsvDropArea = EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Height(50));
        GUILayout.Label(string.IsNullOrEmpty(firstCsvFilePath) ? "No file selected" : Path.GetFileName(firstCsvFilePath), new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter });
        EditorGUILayout.EndVertical();
        HandleCsvDragAndDrop(currentEvent, firstCsvDropArea, ref firstCsvFilePath);
        EditorGUILayout.TextField("Selected CSV Path (1)", firstCsvFilePath, GUI.skin.textField);

        GUILayout.Label("Drag & Drop Second CSV File Here (Optional)", EditorStyles.boldLabel);
        Rect secondCsvDropArea = EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Height(50));
        GUILayout.Label(string.IsNullOrEmpty(secondCsvFilePath) ? "No file selected" : Path.GetFileName(secondCsvFilePath), new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter });
        EditorGUILayout.EndVertical();
        HandleCsvDragAndDrop(currentEvent, secondCsvDropArea, ref secondCsvFilePath);
        EditorGUILayout.TextField("Selected CSV Path (2)", secondCsvFilePath, GUI.skin.textField);


        Rect soDropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(soDropArea, "Drag & Drop ScriptableObject Script Here");
        HandleSODragAndDrop(currentEvent, soDropArea);

        GUILayout.Label("Registered ScriptableObject Types:", EditorStyles.boldLabel);
        if (settings.soTypeNames.Count > 0)
        {
            string[] typeNames = settings.soTypeNames.ToArray();
            selectedSoTypeIndex = EditorGUILayout.Popup("Select SO Class", selectedSoTypeIndex, typeNames);

            if (GUILayout.Button("Delete Selected"))
            {
                if (EditorUtility.DisplayDialog("Confirm Deletion", "Are you sure you want to delete the selected ScriptableObject type?", "Delete", "Cancel"))
                {
                    settings.soTypeNames.RemoveAt(selectedSoTypeIndex);
                    selectedSoTypeIndex = Mathf.Clamp(selectedSoTypeIndex, 0, settings.soTypeNames.Count - 1);
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
            if (!string.IsNullOrEmpty(firstCsvFilePath) && settings.soTypeNames.Count > 0)
            {
                string soTypeName = settings.soTypeNames[selectedSoTypeIndex];
                ConvertCSVtoSO(firstCsvFilePath, Path.GetDirectoryName(firstCsvFilePath), soTypeName, secondCsvFilePath);
            }
            else
            {
                Debug.LogError("First CSV file path or ScriptableObject type is not selected.");
            }
        }
    }

    private void HandleCsvDragAndDrop(Event currentEvent, Rect dropArea, ref string pathVariable)
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
                        pathVariable = path;
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

    private void SaveSettings()
    {
        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void ConvertCSVtoSO(string firstCsvPath, string outputPath, string soTypeName, string secondCsvPath)
    {
        if (!File.Exists(firstCsvPath))
        {
            Debug.LogError("First CSV file not found at path: " + firstCsvPath);
            return;
        }

        Type soType = GetTypeByName(soTypeName);
        if (soType == null || !typeof(ScriptableObject).IsAssignableFrom(soType))
        {
            Debug.LogError("SO Type not found or not a ScriptableObject: " + soTypeName);
            return;
        }

        // 두 번째 CSV 파일의 ID와 텍스트를 미리 로드하여 딕셔너리에 저장
        Dictionary<int, string> scriptTextMap = new Dictionary<int, string>();
        if (!string.IsNullOrEmpty(secondCsvPath) && File.Exists(secondCsvPath))
        {
            string[] secondLines = File.ReadAllLines(secondCsvPath);
            if (secondLines.Length > 1)
            {
                // 변수 이름 'headers'를 'secondHeaders'로 변경하여 충돌 해결
                string[] secondHeaders = secondLines[0].Split(',');
                int idIndex = Array.IndexOf(secondHeaders, "id");
                int textIndex = Array.IndexOf(secondHeaders, "textContent"); // 또는 "text", "scriptText" 등 실제 헤더 이름
                if (idIndex != -1 && textIndex != -1)
                {
                    for (int i = 1; i < secondLines.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(secondLines[i])) continue;
                        string[] values = secondLines[i].Split(',');
                        if (values.Length > idIndex && values.Length > textIndex && int.TryParse(values[idIndex], out int id))
                        {
                            string text = values[textIndex];
                            if (!scriptTextMap.ContainsKey(id))
                            {
                                scriptTextMap.Add(id, text);
                            }
                        }
                    }
                }
                Debug.Log($"Second CSV 파일에서 {scriptTextMap.Count}개의 스크립트 텍스트 로드 완료.");
            }
        }

        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        string[] firstLines = File.ReadAllLines(firstCsvPath);
        if (firstLines.Length <= 1)
        {
            Debug.LogError("First CSV has no data rows.");
            return;
        }

        string[] headers = firstLines[0].Split(',');

        FieldInfo spriteIdField = soType.GetField("spriteID", BindingFlags.Public | BindingFlags.Instance);
        FieldInfo spriteField = soType.GetField("sprite", BindingFlags.Public | BindingFlags.Instance);
        FieldInfo scriptIdField = soType.GetField("scriptID", BindingFlags.Public | BindingFlags.Instance);
        FieldInfo scriptTextField = soType.GetField("scriptText", BindingFlags.Public | BindingFlags.Instance);

        string spriteFolderPath = null;
        if (spriteIdField != null && spriteField != null)
        {
            spriteFolderPath = "Assets/10.Resources/Images/Sprite"; // 스프라이트 경로 직접 지정
        }

        for (int i = 1; i < firstLines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(firstLines[i])) continue;
            string[] values = firstLines[i].Split(',');

            ScriptableObject soInstance = ScriptableObject.CreateInstance(soType);

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                string header = headers[j];
                string value = values[j];

                FieldInfo field = soType.GetField(header, BindingFlags.Public | BindingFlags.Instance);
                if (field == null) continue;

                try
                {
                    if (field.FieldType == typeof(int))
                    {
                        int parsed = 0;
                        if (!string.IsNullOrWhiteSpace(value)) parsed = int.Parse(value);
                        field.SetValue(soInstance, parsed);
                    }
                    else if (field.FieldType == typeof(float))
                    {
                        float parsed = 0f;
                        if (!string.IsNullOrEmpty(value)) parsed = float.Parse(value.Replace("f", ""));
                        field.SetValue(soInstance, parsed);
                    }
                    else if (field.FieldType == typeof(string))
                    {
                        field.SetValue(soInstance, value);
                    }
                    else if (field.FieldType == typeof(List<int>))
                    {
                        var list = new List<int>();
                        if (!string.IsNullOrEmpty(value)) list.AddRange(value.Split(';').Select(s => int.Parse(s)));
                        field.SetValue(soInstance, list);
                    }
                    else if (field.FieldType == typeof(List<string>))
                    {
                        var list = new List<string>();
                        if (!string.IsNullOrEmpty(value)) list.AddRange(value.Split(';'));
                        field.SetValue(soInstance, list);
                    }
                    else if (field.FieldType.IsEnum)
                    {
                        object enumValue = Enum.Parse(field.FieldType, value, true);
                        field.SetValue(soInstance, enumValue);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing field '{header}' with value '{value}': {e.Message}");
                }
            }

            // Sprite 할당 로직 (기존과 동일)
            if (spriteIdField != null && spriteField != null && !string.IsNullOrEmpty(spriteFolderPath))
            {
                int spriteId = (int)spriteIdField.GetValue(soInstance);
                FindAndAssignSprite(soInstance, spriteField, spriteId, spriteFolderPath);
            }

            // ScriptText 할당 로직
            if (scriptIdField != null && scriptTextField != null)
            {
                int scriptId = (int)scriptIdField.GetValue(soInstance);
                if (scriptTextMap.TryGetValue(scriptId, out string scriptText))
                {
                    scriptTextField.SetValue(soInstance, scriptText);
                    Debug.Log($"ID {scriptId}에 해당하는 스크립트 텍스트 할당 완료.");
                }
                else
                {
                    Debug.LogWarning($"ID {scriptId}에 해당하는 스크립트 텍스트를 찾을 수 없습니다.");
                }
            }

            string assetName = values.Length > 1 ? values[0] : $"SO_{i}";
            if (string.IsNullOrEmpty(assetName)) assetName = $"SO_{i}";
            string assetPath = $"{outputPath}/{assetName}.asset";
            AssetDatabase.CreateAsset(soInstance, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("CSV → SO 변환 완료!");
    }

    private static void FindAndAssignSprite(ScriptableObject so, FieldInfo spriteField, int spriteId, string spriteFolderPath)
    {
        string spriteName = spriteId.ToString();
        string searchFilter = $"t:Sprite {spriteName}";
        string[] guids = AssetDatabase.FindAssets(searchFilter, new[] { spriteFolderPath });

        if (guids.Length > 0)
        {
            string spritePath = AssetDatabase.GUIDToAssetPath(guids[0]);
            if (Path.GetFileNameWithoutExtension(spritePath) == spriteName)
            {
                Sprite foundSprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                if (foundSprite != null)
                {
                    spriteField.SetValue(so, foundSprite);
                    Debug.Log($"ID {spriteName}에 해당하는 스프라이트 할당 완료.");
                }
            }
        }
        else
        {
            Debug.LogWarning($"ID {spriteName}에 해당하는 스프라이트를 '{spriteFolderPath}'에서 찾을 수 없습니다.");
        }
    }

    private static Type GetTypeByName(string typeName)
{
    // 우선 완전한 이름으로 시도
    var type = Type.GetType(typeName);
    if (type != null) return type;

    // 모든 로드된 어셈블리 탐색
    foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
    {
        type = asm.GetType(typeName);
        if (type != null) return type;
    }

    // Unity6에서는 Assembly-CSharp 이름 붙여야 하는 경우가 많음
    if (!typeName.Contains(", Assembly-CSharp"))
    {
        type = Type.GetType($"{typeName}, Assembly-CSharp");
        if (type != null) return type;
    }

    Debug.LogError($"❌ Failed to find type: {typeName}");
    return null;
}
}