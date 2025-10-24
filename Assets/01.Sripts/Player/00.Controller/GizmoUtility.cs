using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


// Bezier Curve, 컨트롤 포인트 직접 지정, 부드럽고 정확, 카메라 이동, 스킬/탄환 경로, 연출용 이동
// Catmull-Rom, 경유점 통과, 자동으로 자연스러운 곡선, 캐릭터 이동 경로, NPC 경로, 자동 길찾기
// Spline (Cubic, B-Spline 등), 더 정교한 다중 점 부드러운 경로, 레이싱 트랙, 카메라 레일, 장기 경로

// 선형대수학 + 미적분학 (행렬곱셉은 제외 => 벡터연산으로 커브 구현)

public static class GizmoUtility
{
    //  Catmull-Rom 선형수학 점들의 곡률 계산식 (수학 공식 그대로 이용)
    //  0.5는 tension = 0.5인 Catmull-Rom의 기본 설정, 항의 형태 (2b, -a+c, 2a-5b+4c-d, -a+3b-3c+d)
    private static Vector3 CatmullRom(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        return 0.5f * (
            (2f * b) +
            (-a + c) * t +
            (2f * a - 5f * b + 4f * c - d) * t * t +
            (-a + 3f * b - 3f * c + d) * t * t * t
        );
    }

    //  곡률 그리기 + 점 표시 
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

    //  3차 Bezier 선형수학 곡선 계산식 (수학 공식 그대로 이용)
    //  파스칼(1,3,3,1) 상수 => u2제곱의t는 p3으로 수렴한다
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

    //  Cubic B-Spline 기저 함수 기반 곡선 (Cox–de Boor 공식 그대로)
    //  제어점 인덱스, 차수(order)Cubic이면 k = 3, 
    private static float BSplineBasis(int i, int k, float t)
    {
        // Cubic B-Spline k = 3, Uniform Knots 가정
        if (k == 0)
        {
            if (t >= i && t < i + 1) return 1f;
            return 0f;
        }

        float a = (t - i) / k * BSplineBasis(i, k - 1, t);
        float b = ((i + k + 1 - t) / k) * BSplineBasis(i + 1, k - 1, t);
        return a + b;
    }

    //  B-Spline 곡선 계산 (모든 제어점의 가중합(weighted sum)으로 t 위치의 좌표 결정)
    //  곡선은 제어점들의 ‘혼합’으로 만들어진다
    public static Vector3 BSplinePoint(Vector3[] points, float t)
    {
        int n = points.Length - 1;
        int k = 3; // Cubic
        Vector3 result = Vector3.zero;

        for (int i = 0; i <= n; i++)
        {
            result += points[i] * BSplineBasis(i, k, t * (n - k + 1));
        }

        return result;
    }

    //  Gizmo로 그리기
    public static void DrawBSplineCurve(Vector3[] points, int resolution = 30, Color color = default, bool drawPoints = true)
    {
        if (points == null || points.Length < 4) return;

        Gizmos.color = color == default ? Color.cyan : color;
        Vector3 prev = BSplinePoint(points, 0f);

        for (int i = 1; i <= resolution; i++)
        {
            float t = (float)i / resolution;
            Vector3 curr = BSplinePoint(points, t);
            Gizmos.DrawLine(prev, curr);

            if (drawPoints)
                Gizmos.DrawSphere(curr, 0.05f);

            prev = curr;
        }

#if UNITY_EDITOR
        for (int i = 0; i < points.Length; i++)
        {
            Handles.Label(points[i], $"P{i}");
        }
#endif
    }
}