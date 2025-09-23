using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindManager : Singleton<KeybindManager>
{
    private KeybindButton currentButton;
    private Action currentActionToRebind;

    // 게임에서 사용하는 액션과 기본 키를 정의합니다.
    public enum Action
    {
        MoveForward,
        MoveBackward,
        MoveLeft,
        MoveRight,
        Dodge,
        Look,
        Zoom,
        Attack,
        Companion,
    }

    // 각 액션에 할당된 키를 저장하는 딕셔너리
    private Dictionary<Action, KeyCode> keyBinds = new Dictionary<Action, KeyCode>();

    protected override void Awake()
    {
        base.Awake();

        // 초기 키 바인딩 설정 및 저장된 설정 불러오기
        SetDefaultKeybinds();
        LoadKeybinds();
    }

    // 기본 키 설정
    private void SetDefaultKeybinds()
    {
        keyBinds[Action.MoveForward] = KeyCode.W;
        keyBinds[Action.MoveBackward] = KeyCode.S;
        keyBinds[Action.MoveLeft] = KeyCode.A;
        keyBinds[Action.MoveRight] = KeyCode.D;
        keyBinds[Action.Dodge] = KeyCode.Space;
    }

    // PlayerPrefs에서 키 설정 불러오기
    private void LoadKeybinds()
    {
        foreach (Action action in System.Enum.GetValues(typeof(Action)))
        {
            string keyName = action.ToString();
            if (PlayerPrefs.HasKey(keyName))
            {
                keyBinds[action] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(keyName));
            }
        }
    }

    // 키를 저장하는 함수
    public void SaveKeybinds()
    {
        foreach (var kvp in keyBinds)
        {
            PlayerPrefs.SetString(kvp.Key.ToString(), kvp.Value.ToString());
        }
        PlayerPrefs.Save();
    }

    // 키를 변경하는 함수
    public void SetKeybind(Action action, KeyCode newKey)
    {
        keyBinds[action] = newKey;
    }

    // 특정 액션에 할당된 키를 가져오는 함수
    public KeyCode GetKeybind(Action action)
    {
        return keyBinds[action];
    }

    // 키입력을 기다리는 함수
    public void StartRebindProcess(Action action, KeybindButton button)
    {
        currentActionToRebind = action;
        currentButton = button;
        StartCoroutine(WaitForInput());
    }

    private IEnumerator WaitForInput()
    {
        // 모든 키보드 입력을 확인
        while (true)
        {
            if (Input.anyKeyDown)
            {
                // 입력된 키를 찾음
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        SetKeybind(currentActionToRebind, keyCode);
                        currentButton.UpdateKeyText();
                        SaveKeybinds();
                        yield break; // 코루틴 종료
                    }
                }
            }
            yield return null;
        }
    }
}