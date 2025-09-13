using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// SkillIconUI.cs
public class SkillIconUI : MonoBehaviour
{
    // 쿨타임 오버레이 이미지 (Filled 타입)
    public Image cooltimeOverlay;
    // 이 스킬 아이콘이 나타내는 스킬의 ID
    public string skillID;

    // 초기화 메서드
    public void Initialize(string id)
    {
        skillID = id;
        // 필요하다면 여기서 아이콘 이미지와 텍스트도 설정할 수 있습니다.
    }

    private void OnEnable()
    {
        // 스킬 매니저의 이벤트를 구독합니다.
        // CooltimeEffect 메서드가 이벤트를 수신하도록 설정합니다.
        SkillManagerEX.OnSkillUsed += CooltimeEffect;
    }

    private void OnDisable()
    {
        // 스크립트가 비활성화되면 구독을 해제합니다.
        SkillManagerEX.OnSkillUsed -= CooltimeEffect;
    }

    // 이벤트를 통해 호출될 메서드
    public void CooltimeEffect(float skillCooltime, string usedSkillID)
    {
        // 사용된 스킬의 ID가 이 스킬 아이콘의 ID와 일치할 때만
        // 쿨타임 이펙트를 시작합니다.
        if (this.skillID == usedSkillID)
        {
            StopAllCoroutines();
            StartCoroutine(CooltimeRoutine(skillCooltime));
        }
    }

    // 쿨타임 UI를 업데이트하는 코루틴
    private IEnumerator CooltimeRoutine(float cooltime)
    {
        cooltimeOverlay.fillAmount = 1f;
        cooltimeOverlay.gameObject.SetActive(true);

        float elapsedTime = 0f;
        while (elapsedTime < cooltime)
        {
            elapsedTime += Time.deltaTime;
            cooltimeOverlay.fillAmount = 1f - (elapsedTime / cooltime);
            yield return null;
        }

        cooltimeOverlay.fillAmount = 0f;
        cooltimeOverlay.gameObject.SetActive(false);
    }
}