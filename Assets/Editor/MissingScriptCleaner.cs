using UnityEditor;
using UnityEngine;

public class MissingScriptFinder : EditorWindow
{
    [MenuItem("Tools/Missing Script Finder")]
    public static void ShowWindow()
    {
        GetWindow<MissingScriptFinder>("Missing Script Finder");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Scan Scene for Missing Scripts"))
        {
            ScanScene();
        }
    }

    private void ScanScene()
    {
        GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None); // 비활성 포함
        int count = 0;

        foreach (var go in allObjects)
        {
            var components = go.GetComponents<Component>();
            foreach (var c in components)
            {
                if (c == null)
                {
                    Debug.LogWarning($"Missing Script found in: {GetFullPath(go)}", go);
                    count++;
                }
            }
        }

        Debug.Log($"Scan complete. Found {count} missing scripts.");
    }

    private string GetFullPath(GameObject go)
    {
        return go.transform.parent == null ? go.name : GetFullPath(go.transform.parent.gameObject) + "/" + go.name;
    }
}