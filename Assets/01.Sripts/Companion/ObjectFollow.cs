using UnityEngine;

public class ObjectFollow : MonoBehaviour
{
    [Header("FollowObject")]
    [SerializeField] private Transform targetObject;

    // 회전과 이동에 속도를 적용합니다.
    [Header("Speed")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private void Update()
    {
        FollowObject();
    }

    void FollowObject()
    {
        // 단순 위치 이동 (순간이동하는 느낌) 
        //if (targetObject != null)
        //{
        //    this.transform.position = targetObject.position;
        //    this.transform.rotation = targetObject.rotation;
        //}

        // 부드러운 위치 이동
        if ( targetObject == null ) return;
        this.transform.position = Vector3.Lerp(transform.position, targetObject.position, Time.deltaTime * moveSpeed);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, targetObject.rotation, Time.deltaTime * rotationSpeed);
    }
}
