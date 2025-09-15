using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

// 전투 돌입시 어딘가에서 생성해 줘야함
public class SkillUI : UIBase
{
    // Addressables를 사용하여 UI 프리팹을 참조 (인스펙터 할당)
    public AssetReference skillIconPrefabReference;

    // 스킬 아이콘이 생성될 부모
    public Transform skillIconParent;

    protected override void OnEnable()
    {
        base.OnEnable();

        OnEnableSkill();
    }

    public async void OnEnableSkill()
    {
        // 스킬 매니저의 스킬목록(딕셔너리)을 가져와서 딕셔너리 생성
        Dictionary<string, SkillDataEX> skillDictionary = SkillManagerEX.Instance.GetSkillDictionary();

        // 딕셔너리 목록만큼 스킬아이콘UI 생성
        foreach (var skillDataEX in skillDictionary.Values)
        {
            GameObject skillIconInstance = await Addressables.InstantiateAsync(skillIconPrefabReference, skillIconParent);

            SkillIconUI skillUI = skillIconInstance.GetComponent<SkillIconUI>();

            if (skillUI != null)
            {
                await skillUI.InitializeAsync(skillDataEX.skillID, skillDataEX.skillIconPath);
            }
        }
    }
}