using UnityEngine;
using UnityEditor;

public class SceneMaterialInstancingEnabler
{
    [MenuItem("Tools/Enable GPU Instancing (Scene Only)")]
    static void EnableInstancingInScene()
    {
        int count = 0;

        // 현재 씬에 있는 모든 Renderer 가져오기
        var renderers = Object.FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var renderer in renderers)
        {
            foreach (var mat in renderer.sharedMaterials)
            {
                if (mat != null && mat.shader != null && !mat.enableInstancing)
                {
                    mat.enableInstancing = true;
                    EditorUtility.SetDirty(mat);
                    count++;
                }
            }
        }

        Debug.Log($"씬에 있는 머테리얼 {count}개에 GPU Instancing 활성화 완료 ✅");
    }
}
