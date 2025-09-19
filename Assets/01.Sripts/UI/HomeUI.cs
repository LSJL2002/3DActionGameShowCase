using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class HomeUI : UIBase
{
    public async void OnClickButton(string str)
    {
        switch(str)
        {
            case "GameStart":
                // 게임씬을 로드
                SceneLoadManager.Instance.LoadScene(3);

                // 타임라인 매니저의 타임라인시작 함수 호출
                //TimeLineManager.Instance.PlayTimeLine();

                // 오디오매니저의 BGM을 정지
                break;

            case "OptionUI":
                // IU매니저의 Show 메서드를 호출하여 OptionUI를 화면에 표시
                await UIManager.Instance.Show<OptionUI>();
                break;

            case "Quit":
                // 어플리케이션 종료
                Application.Quit();
                break;
        }

        // 현재 팝업창 닫기
        Hide();
    }
}
