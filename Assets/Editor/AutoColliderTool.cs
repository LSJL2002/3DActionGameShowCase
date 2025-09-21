using UnityEditor;
using UnityEngine;
using static PlasticGui.PlasticTableColumn;

public class AutoColliderTool
{
    // ==============================
    // 합치기 버전 (전체 → 1개)
    // ==============================
    public static void MakeColliderCombined()
    {
        if (Selection.gameObjects.Length == 0) return;

        GameObject firstObj = Selection.gameObjects[0];
        MeshRenderer first = firstObj.GetComponentInChildren<MeshRenderer>();
        if (first == null) return;

        Bounds combinedBounds = first.bounds;
        foreach (GameObject obj in Selection.gameObjects)
        {
            var renderers = obj.GetComponentsInChildren<MeshRenderer>();
            foreach (var r in renderers)
                combinedBounds.Encapsulate(r.localBounds);
        }

        Vector3 size = combinedBounds.size;
        size.y = 1f;

        Vector3 center = combinedBounds.center;
        center.y = 1f;

        GameObject colliderObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        colliderObj.transform.position = center;
        colliderObj.transform.rotation = firstObj.transform.rotation;
        colliderObj.transform.localScale = size;

        colliderObj.name = Selection.gameObjects[0].gameObject.transform.parent.name + "_Collider";

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
            var renderer = obj.GetComponentInChildren<MeshRenderer>();
            if (renderer == null) continue;

            Bounds localBounds = renderer.localBounds;

            // 실제 월드 크기로 변환
            Vector3 worldSize = Vector3.Scale(localBounds.size, renderer.transform.lossyScale);
            worldSize.y = 1f;

            // 실제 월드 위치로 변환
            Vector3 worldCenter = renderer.transform.TransformPoint(localBounds.center);
            worldCenter.y = 1f;



            GameObject colliderObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            colliderObj.transform.position = worldCenter;
            colliderObj.transform.rotation = obj.transform.rotation;
            colliderObj.transform.localScale = worldSize;

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
