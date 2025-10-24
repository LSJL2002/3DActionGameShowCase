using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
// 빌드시에는 무시되므로 안전, 에디터전용 네임스페이스


// Bezier Curve, 컨트롤 포인트 직접 지정, 부드럽고 정확, 카메라 이동, 스킬/탄환 경로, 연출용 이동
// Catmull-Rom, 경유점 통과, 자동으로 자연스러운 곡선, 캐릭터 이동 경로, NPC 경로, 자동 길찾기
// Spline (Cubic, B-Spline 등), 더 정교한 다중 점 부드러운 경로, 레이싱 트랙, 카메라 레일, 장기 경로
public static class GizmoUtility
{
    //  Catmull-Rom 계산식 (수학 공식 그대로 유지)
    private static Vector3 CatmullRom(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        return 0.5f * (
            (2f * b) +
            (-a + c) * t +
            (2f * a - 5f * b + 4f * c - d) * t * t +
            (-a + 3f * b - 3f * c + d) * t * t * t
        );
    }

    //  Catmull-Rom 곡선을 Gizmo로 그려주는 함수
    public static void DrawCatmullRomCurve(Vector3[] points, int resolution, Color color)
    {
        if (points == null || points.Length < 4) return;

        Gizmos.color = color;
        Vector3 prevPos = points[1];

        for (int i = 1; i < points.Length - 2; i++)
        {
            for (int r = 1; r <= resolution; r++)
            {
                float t = r / (float)resolution;
                Vector3 pos = CatmullRom(points[i - 1], points[i], points[i + 1], points[i + 2], t);
                Gizmos.DrawLine(prevPos, pos);
                prevPos = pos;
            }
        }
    }

    //  3차 Bezier 계산
    public static Vector3 BezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        return
            u * u * u * p0 +
            3 * u * u * t * p1 +
            3 * u * t * t * p2 +
            t * t * t * p3;
    }

    //  곡선 그리기 + 점 표시
    public static void DrawBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3,
                                       int resolution = 30, bool drawPoints = true)
    {
        Vector3 prev = p0;
        for (int i = 1; i <= resolution; i++)
        {
            float t = (float)i / resolution;
            Vector3 curr = BezierPoint(p0, p1, p2, p3, t);
            Gizmos.DrawLine(prev, curr);

            if (drawPoints)
                Gizmos.DrawSphere(curr, 0.07f);

            prev = curr;
        }

#if UNITY_EDITOR
        Handles.Label(p0, "P0");
        Handles.Label(p1, "P1");
        Handles.Label(p2, "P2");
        Handles.Label(p3, "P3");
#endif
    }
}