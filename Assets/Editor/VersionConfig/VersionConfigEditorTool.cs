using UnityEditor;
using UnityEngine;
using System.IO;

public class VersionConfigEditorTool
{
    // config.json 파일을 저장할 프로젝트 내의 위치
    private const string JSON_EXPORT_PATH = "Assets/Editor/VersionConfig/VersionConfig.json";

    [MenuItem("Tools/Export VersionConfigData to JSON")]
    public static void ExportConfigJson()
    {
        // 1. 저장할 데이터 생성 (실제 툴에서는 UI에서 입력받거나 자동으로 설정됨)
        VersionConfigData config = new VersionConfigData
        {
            // Addressables 빌드가 완료된 후, 새 버전을 여기에 설정
            LatestContentVersion = "1.0.0",
            MinimumAppVersion = Application.version, // 현재 앱 버전 사용
            CDN_URL = "https://s3.ap-southeast-2.amazonaws.com/project8.addressable/StandaloneWindows64/"
        };

        // 2. JSON 문자열로 변환
        string jsonString = JsonUtility.ToJson(config, true);

        // 3. 파일로 저장
        string fullPath = Application.dataPath.Replace("Assets", "") + JSON_EXPORT_PATH;
        File.WriteAllText(fullPath, jsonString);

        Debug.Log($"VersionConfigData JSON 파일 저장 완료: {fullPath}");
        AssetDatabase.Refresh(); // 유니티 에디터에 변경 사항을 알림
    }
}