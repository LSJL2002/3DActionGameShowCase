using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

// GameUI의 Skill Part
public partial class GameUI : UIBase
{
    public GameObject skillInfoUI; // 스킬UI 최상위 오브젝트 (활성화 컨트롤용 변수)

    public void OnEnableSkill()
    {
        // 스킬매니저의 스킬 사용 이벤트를 구독
        // 스킬매니저 이벤트선언 -> 스킬매니저의 UseSkill함수 통해 스킬 사용(이벤트실행함수 포함) -> SkillUI에서 구독하여 쿨타임 표시
        // SkillManager.Instance.OnCooltimeStarted += UpdateCooltimeUI;
    }

    public void OnDisableSkill()
    {
        // 스킬매니저의 스킬 사용 이벤트를 구독해제
        // 스킬매니저 이벤트선언 -> 스킬매니저의 UseSkill함수 통해 스킬 사용(이벤트실행함수 포함) -> SkillUI에서 구독하여 쿨타임 표시
        // SkillManager.Instance.OnCooltimeStarted -= UpdateCooltimeUI;
    }

    public void UpdateCooltimeUI()
    {
        // 쿨타임 아이콘 효과
    }

    // 전투 돌입 / 종료시 호출할 함수
    // Battle상태 전환시에는 스킬정보를 매개변수로 받음? 아니면 플레이어 정보에서 찾아서 가져오기?
    public void ChangeState(eBattleState state)
    {
        // 매개변수를 받아서 상태를 변경 (Idle <-> Battle)
        currentBattleState = state;

        switch (state)
        {
            case eBattleState.Idle:

                break;

            case eBattleState.Battle:

                break;
        }
    }
}
