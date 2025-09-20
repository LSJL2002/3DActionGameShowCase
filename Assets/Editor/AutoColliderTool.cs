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

        // MeshFilter / MeshRenderer는 남겨두고 InvisibleMat 적용
        Material invisibleMat = Resources.Load<Material>("InvisibleMat");
        if (invisibleMat != null)
        {
            colliderObj.GetComponent<MeshRenderer>().sharedMaterial = invisibleMat;
        }
        else
        {
            Debug.LogWarning("InvisibleMat을 Resources 폴더에 넣어야 합니다! (Resources/InisibleMat.mat)");
        }

        GameObject root = GameObject.Find("CollisionRoot");
        if (root == null) root = new GameObject("CollisionRoot");
        colliderObj.transform.SetParent(root.transform);

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

            // InvisibleMat 적용
            Material invisibleMat = Resources.Load<Material>("InvisibleMat");
            if (invisibleMat != null)
            {
                colliderObj.GetComponent<MeshRenderer>().sharedMaterial = invisibleMat;
            }

            GameObject root = GameObject.Find("CollisionRoot");
            if (root == null) root = new GameObject("CollisionRoot");
            colliderObj.transform.SetParent(root.transform);
        }

        Debug.Log($"FlatCollider 생성 (개별) → {Selection.gameObjects.Length}개");
    }

    // ==============================
    // 메뉴 등록 (Tools 전용)
    // ==============================
    [MenuItem("Tools/Auto Colliders/Combined #x", false, 0)]
    static void MakeColliderCombinedMenu() => MakeColliderCombined();

    [MenuItem("Tools/Auto Colliders/Each #z", false, 1)]
    static void MakeCollidersEachMenu() => MakeCollidersEach();
}
