using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ComboStep
{
    public string stateName; //에니메이션 이름
    public float maxStepTime = 0.8f; //콤보가 종료될 시간
    public float comboTimingstart = 0.3f; //다음콤보로 넘어갈수 있는 범위
    public float combotimingEnd = 0.7f;
}
public class ComboCombo : MonoBehaviour
{
    [SerializeField] ComboStep[] _steps;

    [SerializeField] float _inputBufferTime = 0.2f; //쉽게 콤보공격을 이어주기위한
    Animator _anim;

    int _currentCombo = -1; //현재인덱스, -1은 공격중X, 1콤보부터0 시작
    float _comboStartTime; //콤보공격이 언제시작되었는지 기록
    bool _queuedNextCombo; //다음콤보로 넘어가는 조건 충족 검사
    float _lastInputTime; //마지막 입력시간

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) //마우스버튼을 누르면
        {
            _lastInputTime = Time.time; //현재입력 시간저장
            StartComboattack();
        }
        if (_currentCombo >= 0)
            UpdateCombo();
    }

    void StartComboattack()
    {
        if(_currentCombo < 0) //공격중이 아니라면 1타부터
        {
            StartCombo(0);
            return;
        }
        var step = _steps[_currentCombo]; //현재실행된 공격정보
        float elapsed = Time.time - _comboStartTime; //콤보시작후 얼마나 흘렀는지

        bool isInTiming = elapsed >= step.comboTimingstart && elapsed <= step.combotimingEnd;
        bool withinBuffer = Time.time - _lastInputTime <= _inputBufferTime;
        // elapsed가 콤보 타이밍내 존재한다면
        if (isInTiming && withinBuffer)
            _queuedNextCombo = true;
    }
    void StartCombo(int index)
    {
        _currentCombo = Mathf.Clamp(index, 0, _steps.Length - 1); //steps의 범위 넘기지않게 안전하게 보장
        _comboStartTime = Time.time;
        _queuedNextCombo = false; //다음콤보로 저절로 넘어가지 방지

        _anim.CrossFade(_steps[_currentCombo].stateName, 0.05f);
    }
    void UpdateCombo()
    {
        var step = _steps[_currentCombo];
        float elapsed = Time.time - _comboStartTime;

        //다음콤보가 예약이 되어있고 다음콤보로 넘어갈수있는 시간대로 진입했다면 진입 ㄱㄱ
        if(_queuedNextCombo && elapsed >= step.comboTimingstart)
        {
            int next = _currentCombo + 1;
            if (next < _steps.Length)
                StartCombo(next);
            else
                ResetCombo(); //막타라면 리셋
            return;
        }

        if(elapsed >= step.maxStepTime) //경과 시간 초과시 리셋
            ResetCombo();
    }
    void ResetCombo()
    {
        _currentCombo --;
        _queuedNextCombo = false;
    }
}
