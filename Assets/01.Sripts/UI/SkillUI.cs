using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

// 전투 돌입시 어딘가에서 생성해 줘야함
public class SkillUI : UIBase
{
    // Addressables를 사용하여 UI 프리팹을 참조합니다.
    public AssetReference skillIconPrefabReference;

    // 스킬 아이콘이 생성될 부모 Transform (UI Canvas 등)
    public Transform skillIconParent;

    protected override void OnEnable()
    {
        base.OnEnable();

        OnEnableSkill();
    }

    public async void OnEnableSkill()
    {
        // 스킬 매니저로부터 스킬 목록을 가져옵니다.
        Dictionary<string, SkillDataEX> skillDictionary = SkillManagerEX.Instance.GetSkillDictionary();

        foreach (var skillData in skillDictionary.Values)
        {
            // Addressables를 사용해 UI 프리팹을 동적으로 인스턴스화합니다.
            GameObject skillIconInstance = await Addressables.InstantiateAsync(skillIconPrefabReference, skillIconParent);

            // 생성된 스킬 아이콘에 데이터를 할당합니다.
            SkillIconUI skillUI = skillIconInstance.GetComponent<SkillIconUI>();
            if (skillUI != null)
            {
                // SkillDataEX의 skillID를 SkillIconUI에 전달하여 초기화합니다.
                await skillUI.InitializeAsync(skillData.skillID, skillData.skillIconPath);
            }
        }
    }
}