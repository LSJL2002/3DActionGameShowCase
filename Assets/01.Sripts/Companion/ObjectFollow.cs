using UnityEngine;

public class ObjectFollow : MonoBehaviour
{
    [Header("FollowObject")]
    [SerializeField] private Transform targetObject;

    // 회전과 이동에 속도를 적용합니다.
    [Header("Speed")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("This.RigidBody")]
    [SerializeField] private Rigidbody rb;

    private void Update()
    {
        FollowObject();
    }

    void FollowObject()
    {
        #region 이전코드
        // 단순 위치 이동 (순간이동하는 느낌) 
        //if (targetObject != null)
        //{
        //    this.transform.position = targetObject.position;
        //    this.transform.rotation = targetObject.rotation;
        //}

        // 부드러운 위치 이동
        //if ( targetObject == null ) return;
        //this.transform.position = Vector3.Lerp(transform.position, targetObject.position, Time.deltaTime * moveSpeed);
        //this.transform.rotation = Quaternion.Slerp(transform.rotation, targetObject.rotation, Time.deltaTime * rotationSpeed);
        #endregion

        if (targetObject == null || rb == null) return;

        // Rigidbody를 이용한 부드러운 위치 이동
        Vector3 nextMove = Vector3.MoveTowards(rb.position, targetObject.position, moveSpeed * Time.deltaTime);
        rb.MovePosition(nextMove);
        // Rigidbody를 이용한 부드러운 회전
        Quaternion nextRotation = Quaternion.RotateTowards(rb.rotation, targetObject.rotation, rotationSpeed * Time.deltaTime);
        rb.MoveRotation(nextRotation);
    }
}
