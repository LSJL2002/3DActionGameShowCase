using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// 각 씬의 스크립트가 상속받을 클래스
public abstract class SceneBase : MonoBehaviour
{
    // 현재 매니저가 만들어져 있는지 확인하는 과정
    protected virtual void Awake()
    {
        // 씬로드매니저의 isManager 변수가 false라면
        if (!SceneLoadManager.Instance.isManager)
        {
            // 씬로드매니저의 Change씬 함수를 호출
            // 매개변수1 : n번 씬으로 변경 (매니저씬)
            // 매개변수2 : 람다식을 이용하여 이름없는 함수식을 매개변수로 전달 (inManager 변수를 true로 변경 + "매니저씬"을 언로드씬)
            // 매개변수3 : Additive형식의 로드씬모드 지정 (한 씬에 두개이상의 씬이 존재가능, 매니저씬은 매니저만 생성후 바로 언로드씬)
            SceneLoadManager.Instance.ChangeScene
            (
                2, // 매개변수1
                () =>{SceneLoadManager.Instance.isManager = true; SceneLoadManager.Instance.UnLoadScene(2);}, // 매개변수2
                LoadSceneMode.Additive // 매개변수3
            );
        }
    }

    protected virtual void OnEnable() { }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void FixedUpdate() { }

    protected virtual void LateUpdate() { }

    protected virtual void OnDisable() { }

    protected virtual void OnDestroy() { }
}
