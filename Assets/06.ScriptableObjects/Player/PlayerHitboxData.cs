using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SkillHitboxData", menuName = "Hitbox/SkillHitboxData")]
public class PlayerHitboxData : ScriptableObject
{
    // ===== 히트박스 정보 통합 =====
    [field: SerializeField, Tooltip("히트박스 타입")]
    public HitboxType Type;

    [field: SerializeField, Tooltip("히트박스 오프셋")]
    public Vector3 offset = Vector3.zero;

    [field: SerializeField, Tooltip("히트박스 대상 레이어")]
    public LayerMask targetLayer;

    // Sphere / Capsule
    [field: SerializeField, Tooltip("반지름 (Sphere / Capsule)")]
    public float radius = 1f;

    // Capsule
    [field: SerializeField, Tooltip("캡슐 시작 지점 (Capsule)")]
    public Vector3 capsulePoint1;

    [field: SerializeField, Tooltip("캡슐 끝 지점 (Capsule)")]
    public Vector3 capsulePoint2;

    // Box
    [field: SerializeField, Tooltip("박스 크기 (Box)")]
    public Vector3 boxSize = Vector3.one;

    // Ray
    [field: SerializeField, Tooltip("레이 방향 (Ray)")]
    public Vector3 rayDirection = Vector3.forward;

    [field: SerializeField, Tooltip("레이 거리 (Ray)")]
    public float rayDistance = 5f;
}