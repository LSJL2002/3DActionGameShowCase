using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    //캐릭터가 받아야 할 힘을 모아놓은 계산기
    //중력, 넉백, 점프 같은걸 더 자연스럽게 만들어줌
    //그 힘을 실제로 적용시키는건 캐릭터컨트롤러의 Move

    [SerializeField] private CharacterController controller;

    [SerializeField] private float drag = 0.3f;

    private float verticalVelocity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;
    private Vector3 dampingVelocity;
    private Vector3 impact;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
    }

    public void Reset()
    {
        verticalVelocity = 0;
        impact = Vector3.zero;
    }

    
    // 공격/점프 구분 가능
    public void AddForce(Vector3 force, bool horizontalOnly = false)
    {
        if (horizontalOnly)
            force.y = 0;

        impact += force;
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}
