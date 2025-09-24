using UnityEditor;
using UnityEngine;
using static PlasticGui.PlasticTableColumn;

public class AutoColliderTool
{
    // ==============================
    // 합치기 버전 (선택한 전체 → 1개의 Collider)
    // ==============================
    public static void MakeColliderCombined()
    {
        // 아무것도 선택하지 않았으면 종료
        if (Selection.gameObjects.Length == 0) return;

        // 첫 번째 선택된 오브젝트의 MeshRenderer 가져오기
        GameObject firstObj = Selection.gameObjects[0];
        MeshRenderer first = firstObj.GetComponentInChildren<MeshRenderer>();
        if (first == null) return;

        // 첫 번째 렌더러의 Bounds로 시작
        Bounds combinedBounds = first.bounds;

        // 선택된 모든 오브젝트의 MeshRenderer Bounds를 합쳐서 하나의 큰 Bounds 생성
        foreach (GameObject obj in Selection.gameObjects)
        {
            var renderers = obj.GetComponentsInChildren<MeshRenderer>();
            foreach (var r in renderers)
                combinedBounds.Encapsulate(r.bounds); // 영역 확장
        }

        // 최종 Bounds 크기/중심값 → 평면화 처리(y는 얇게 1f 고정)
        Vector3 size = combinedBounds.size;
        size.y = 1f;

        Vector3 center = combinedBounds.center;
        center.y = 1f;

        // 최종 Collider 오브젝트 생성 (Cube로)
        GameObject colliderObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        colliderObj.transform.position = center;
        colliderObj.transform.rotation = firstObj.transform.rotation; // 첫 번째 오브젝트 회전값 맞춤
        colliderObj.transform.localScale = size;

        // 이름 설정: 부모 이름 + "_Collider"
        colliderObj.name = Selection.gameObjects[0].gameObject.transform.parent.name + "_Collider";

        // InvisibleMat 적용 (렌더링은 안 보이게, 충돌만 남김)
        Material invisibleMat = Resources.Load<Material>("InvisibleMat");
        if (invisibleMat != null)
        {
            colliderObj.GetComponent<MeshRenderer>().sharedMaterial = invisibleMat;
        }
        else
        {
            Debug.LogWarning("InvisibleMat을 Resources 폴더에 넣어야 합니다! (Resources/InvisibleMat.mat)");
        }

        // "CollisionRoot"라는 루트 오브젝트 하위에 정리
        GameObject root = GameObject.Find("CollisionRoot");
        if (root == null) root = new GameObject("CollisionRoot");
        colliderObj.transform.SetParent(root.transform);

        // 마지막으로 생성한 Collider 선택 상태로
        Selection.activeGameObject = colliderObj;

        Debug.Log("FlatCollider 생성 (합치기)");
    }

    // ==============================
    // 개별 버전 (선택한 각각 오브젝트 → Collider 1개씩)
    // ==============================
    public static void MakeCollidersEach()
    {
        if (Selection.gameObjects.Length == 0) return;

        foreach (GameObject obj in Selection.gameObjects)
        {
            // 각 오브젝트의 MeshRenderer 가져오기
            var renderer = obj.GetComponentInChildren<MeshRenderer>();
            if (renderer == null) continue;

            // 로컬 Bounds 가져오기
            Bounds localBounds = renderer.localBounds;

            // 월드 좌표계 기준으로 크기 변환
            Vector3 worldSize = Vector3.Scale(localBounds.size, renderer.transform.lossyScale);
            worldSize.y = 1f;

            // 월드 좌표계 기준 중심 위치 계산
            Vector3 worldCenter = renderer.transform.TransformPoint(localBounds.center);
            worldCenter.y = 1f;

            // Collider 오브젝트 생성
            GameObject colliderObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            colliderObj.transform.position = worldCenter;
            colliderObj.transform.rotation = obj.transform.rotation;
            colliderObj.transform.localScale = worldSize;

            // 이름 설정: 원본 이름 + "_Collider"
            colliderObj.name = obj.name + "_Collider";

            // InvisibleMat 적용
            Material invisibleMat = Resources.Load<Material>("InvisibleMat");
            if (invisibleMat != null)
            {
                colliderObj.GetComponent<MeshRenderer>().sharedMaterial = invisibleMat;
            }

            // CollisionRoot 하위에 정리
            GameObject root = GameObject.Find("CollisionRoot");
            if (root == null) root = new GameObject("CollisionRoot");
            colliderObj.transform.SetParent(root.transform);
        }

        Debug.Log($"FlatCollider 생성 (개별) → {Selection.gameObjects.Length}개");
    }

    // ==============================
    // 메뉴 등록 (Unity 상단 메뉴 Tools에 추가)
    // ==============================

    // 단축키 Shift+X → 합치기 버전 실행
    [MenuItem("Tools/Auto Colliders/Combined #x", false, 0)]
    static void MakeColliderCombinedMenu() => MakeColliderCombined();

    // 단축키 Shift+Z → 개별 버전 실행
    [MenuItem("Tools/Auto Colliders/Each #z", false, 1)]
    static void MakeCollidersEachMenu() => MakeCollidersEach();
}
