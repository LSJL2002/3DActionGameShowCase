using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SkillIconUI : MonoBehaviour
{
    public Image cooltimeOverlay; // 스킬 쿨타임 시각효과용 이미지

    public Image skillIcon; // 스킬아이콘

    public string skillID; // 스킬ID

    private void OnEnable()
    {
        // 스킬 매니저의 이벤트를 구독
        SkillManagerEX.OnSkillUsed += CooltimeEffect;
    }

    private void OnDisable()
    {
        // 스킬 매니저의 이벤트를 구독 해제
        SkillManagerEX.OnSkillUsed -= CooltimeEffect;
    }

    // 이벤트를 통해 호출될 메서드
    public void CooltimeEffect(float skillCooltime, string usedSkillID)
    {
        // 사용된 스킬의 ID가 이 스킬 아이콘의 ID와 일치할 때만
        if (this.skillID == usedSkillID)
        {
            StopCoroutine(CooltimeRoutine(skillCooltime));
            StartCoroutine(CooltimeRoutine(skillCooltime));
        }
    }

    // 초기화 메서드
    public async UniTask InitializeAsync(string skillId, string skillIconPaht)
    {
        skillID = skillId;

        skillIcon.sprite = await Addressables.LoadAssetAsync<Sprite>(skillIconPaht).Task;
    }

    // 쿨타임 UI를 업데이트하는 코루틴
    private IEnumerator CooltimeRoutine(float cooltime)
    {
        cooltimeOverlay.gameObject.SetActive(true);
        cooltimeOverlay.fillAmount = 1f;

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