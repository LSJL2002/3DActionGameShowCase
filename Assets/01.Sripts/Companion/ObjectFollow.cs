using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow : MonoBehaviour
{
    [Header("FollowObject")]
    [SerializeField] private Transform targetObject;

    private void Update()
    {
        FollowObject();
    }

    void FollowObject()
    {
        if (targetObject != null)
        {
            this.transform.position = targetObject.position;
            this.transform.rotation = targetObject.rotation;
        }
    }
}
