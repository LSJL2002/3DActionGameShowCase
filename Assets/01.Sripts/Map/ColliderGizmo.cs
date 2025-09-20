using UnityEngine;

[ExecuteInEditMode]
public class ColliderGizmo : MonoBehaviour
{
    public Color gizmoColor = new Color(1f, 0f, 0f, 0.25f); // 빨강 반투명

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        var box = GetComponent<BoxCollider>();
        if (box != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box.center, box.size);        // 면
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(box.center, box.size);   // 외곽선
        }
    }
}
