using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    // [Inspector에서 EventSO 에셋을 연결]
    [SerializeField] private BaseEventSO targetEventSO;

    // [Inspector에서 이벤트 발생 시 실행할 함수들을 연결]
    [SerializeField] private UnityEvent response;

    // 이 리스너가 활성화될 때 이벤트에 함수를 등록(구독)
    private void OnEnable()
    {
        if (targetEventSO != null)
        {
            targetEventSO.OnAnyEventRaised += OnEventRaised;
        }
    }

    // 이 리스너가 비활성화될 때 구독을 해제합니다. (메모리 누수 방지)
    private void OnDisable()
    {
        if (targetEventSO != null)
        {
            targetEventSO.OnAnyEventRaised -= OnEventRaised;
        }
    }

    // 이벤트가 발생했을 때 호출되는 실제 핸들러 함수
    public void OnEventRaised()
    {
        // 연결된 인스펙터의 모든 함수를 실행
        response.Invoke();
    }
}