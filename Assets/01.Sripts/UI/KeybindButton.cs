using UnityEngine;
using UnityEngine.UI;

public class KeybindButton : MonoBehaviour
{
    // 어떤 액션에 대한 버튼인지 인스펙터에서 설정
    public KeybindManager.Action action;

    private Button button;
    private Text buttonText;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<Text>();

        // 버튼 클릭 시 키 변경 로직을 시작하도록 이벤트 리스너 추가
        button.onClick.AddListener(StartKeyRebinding);
    }

    private void OnEnable()
    {
        // UI가 활성화될 때마다 현재 키를 텍스트에 표시
        UpdateKeyText();
    }

    public void UpdateKeyText()
    {
        buttonText.text = KeybindManager.Instance.GetKeybind(action).ToString();
    }

    private void StartKeyRebinding()
    {
        // 모든 키 변경 프로세스를 중단하고 새 키 변경 프로세스 시작
        KeybindManager.Instance.StartRebindProcess(action, this);
    }
}