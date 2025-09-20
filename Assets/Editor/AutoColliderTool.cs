using UnityEngine;
using UnityEditor;

public class AutoColliderTool
{
    // ==============================
    // 합치기 버전 (전체 → 1개)
    // ==============================
    public static void MakeColliderCombined()
    {
        if (Selection.gameObjects.Length == 0) return;

        MeshRenderer first = Selection.gameObjects[0].GetComponentInChildren<MeshRenderer>();
        if (first == null) return;

        Bounds combinedBounds = first.bounds;
        foreach (GameObject obj in Selection.gameObjects)
        {
            var renderers = obj.GetComponentsInChildren<MeshRenderer>();
            foreach (var r in renderers)
                combinedBounds.Encapsulate(r.bounds);
        }

        Vector3 size = combinedBounds.size;
        size.y = 1f;

        Vector3 center = combinedBounds.center;
        center.y = 1f;

        GameObject colliderObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        colliderObj.transform.position = center;
        colliderObj.transform.localScale = size;

        colliderObj.name = "FlatCollider";

        Object.DestroyImmediate(colliderObj.GetComponent<MeshRenderer>());
        Object.DestroyImmediate(colliderObj.GetComponent<MeshFilter>());

        GameObject root = GameObject.Find("CollisionRoot");
        if (root == null) root = new GameObject("CollisionRoot");
        colliderObj.transform.SetParent(root.transform);

        colliderObj.AddComponent<ColliderGizmo>();
        Selection.activeGameObject = colliderObj;

        Debug.Log("FlatCollider 생성 (합치기)");
    }

    // ==============================
    // 개별 버전 (각각 → 1개씩)
    // ==============================
    public static void MakeCollidersEach()
    {
        if (Selection.gameObjects.Length == 0) return;

        foreach (GameObject obj in Selection.gameObjects)
        {
            var renderers = obj.GetComponentsInChildren<MeshRenderer>();
            if (renderers.Length == 0) continue;

            Bounds combinedBounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
                combinedBounds.Encapsulate(renderers[i].bounds);

            Vector3 size = combinedBounds.size;
            size.y = 1f;

            Vector3 center = combinedBounds.center;
            center.y = 1f;

            GameObject colliderObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            colliderObj.transform.position = center;
            colliderObj.transform.localScale = size;

            colliderObj.name = obj.name + "_Collider";

            Object.DestroyImmediate(colliderObj.GetComponent<MeshRenderer>());
            Object.DestroyImmediate(colliderObj.GetComponent<MeshFilter>());

            GameObject root = GameObject.Find("CollisionRoot");
            if (root == null) root = new GameObject("CollisionRoot");
            colliderObj.transform.SetParent(root.transform);

            colliderObj.AddComponent<ColliderGizmo>();
        }

        Debug.Log($"FlatCollider 생성 (개별) → {Selection.gameObjects.Length}개");
    }

    // ==============================
    // 메뉴 등록
    // ==============================
    [MenuItem("GameObject/Auto/Colliders/Combined", false, 10)]
    static void MakeColliderCombinedMenu(MenuCommand menuCommand) => MakeColliderCombined();

    [MenuItem("GameObject/Auto/Colliders/Each", false, 11)]
    static void MakeCollidersEachMenu(MenuCommand menuCommand) => MakeCollidersEach();
}
