using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("FollowObject")]
    [SerializeField] public Transform targetObject;

    public Vector3 lookOffset = new Vector3(0f, 1.6f, 0f);
    private Vector3 localOffset;

    void Start()
    {
        if (targetObject == null) return;
        localOffset = targetObject.InverseTransformPoint(transform.position);
    }

    void LateUpdate()
    {
        if (targetObject == null) return;

        transform.position = targetObject.TransformPoint(localOffset);
        Vector3 lookPos = targetObject.position + lookOffset;
        transform.rotation = Quaternion.LookRotation(lookPos - transform.position, Vector3.up);
    }
}
