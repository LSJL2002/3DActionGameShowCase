using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class PlayerGroundData
{
    [field: Header("IdleData")]
    [field: SerializeField][field: Range(0f, 50f)] public float BaseRotationDamping { get; private set; } = 25f;
    //캐릭터회전

    [field: Header("WalkData")]
    [field: SerializeField][field: Range(0f, 5f)] public float BaseSpeed { get; private set; } = 1f;
    //기본이속
    [field: SerializeField][field: Range(0f, 2f)] public float WalkSpeedModifier { get; private set; } = 0.225f;
    //걷기속도
    [field: SerializeField][field: Range(0f, 5f)] public float RunAccelerationTime { get; private set; } = 2f;
    //가속시간

    [field: Header("RunData")]
    [field: SerializeField][field: Range(0f, 2f)] public float RunSpeedModifier { get; private set; } = 1f;
    //달리기속도
}

[Serializable]
public class PlayerAirData
{
    [field: Header("JumpData")]
    [field: SerializeField][field: Range(0f, 25f)] public float JumpForce { get; private set; } = 5f;

    //이중 점프 여부
    //점프 소모량
}

[Serializable]
public class PlayerDefenseData
{
    //슈퍼아머여부, 스태미너 소모량
    //방어시 감소율, 패일 가능타이밍
    //대시소모량
}


[Serializable]
public class PlayerAttackData
{
    [field: SerializeField][field: Range(0f, 10f)] public float DashSpeed { get; private set; } = 10f;
    [field: SerializeField][field: Range(0f, 5f)] public float StopDistance { get; private set; } = 3f;
    //돌진 거리
    [field: SerializeField][field: Range(0f, 10f)] public float AttackRange { get; private set; } = 5f;
    //공격 범위

    [field: SerializeField] public List<AttackInfoData> AttackInfoDatas { get; private set; }
    public int GetAttackInfoCount() { return AttackInfoDatas.Count; }
    public AttackInfoData GetAttackInfoData(int index) { return AttackInfoDatas[index]; }
}

[Serializable]
public class AttackInfoData
{
    [field: SerializeField] public string AttackName { get; private set; }
    [field: SerializeField] public int ComboStateIndex { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float ComboTransitionTime { get; private set; } = 0.8f;
    //다음공격 실행시점
    [field: SerializeField][field: Range(0f, 3f)] public float ForceTransitionTime { get; private set; }
    //앞으로 밀리는 시간
    [field: SerializeField][field: Range(-10f, 10f)] public float Force { get; private set; } = 1f;
    //캐릭터에 적용되는 힘
    [field: SerializeField][field: Range(0f, 1f)] public float Dealing_Start_TransitionTime { get; private set; }
    //공격판정 시작시간
    [field: SerializeField][field: Range(0f, 1f)] public float Dealing_End_TransitionTime { get; private set; }
    //공격판정 종료시점
}

public enum AttackType
{
    LightAttack,
    HeavyAttack,
    Skill,
    Dodge
}

public enum EffectType
{
    None,
    knockback,
    groggy,
}

[Serializable]
public class PlayerSkillData
{
    [field: SerializeField] public List<SkillInfoData> SkillInfoData { get; private set; }
    public int GetSkillInfoCount() { return SkillInfoData.Count; }
    public SkillInfoData GetSkillInfoCount(int index) { return SkillInfoData[index]; }
}

[Serializable]
public class SkillInfoData
{
    [field: SerializeField] public int id { get; private set; }
    [field: SerializeField] public string name { get; private set; }
    [field: SerializeField] public AttackType attackType { get; private set; }
    [field: SerializeField] public EffectType effectType { get; private set; }
    [field: SerializeField] public float effectValue { get; private set; }
    [field: SerializeField] public float duration { get; private set; }
    //public List<castEffectName> = new List<castEffectName>();
    [field: SerializeField] public float range { get; private set; }
    //public List<areaEffectPrefab> = new List<areaEffectPrefab>();
    [field: SerializeField] public float knockbackDistance { get; private set; }
    [field: SerializeField] public int comboSkillId { get; private set; }
    [field: SerializeField] public float cooldown { get; private set; }
    [field: SerializeField] public int mpCost { get; private set; }
    [field: SerializeField] public int hitCount { get; private set; }
    [field: SerializeField] public float skillUseRange { get; private set; }
    [field: SerializeField] public float preCastTime { get; private set; }
}



[Serializable]
public class PlayerStatData
{
    [field: SerializeField] public int id { get; private set; } = 0;
    [field: SerializeField] public string name { get; private set; } = "Test";
    [field: SerializeField] public float maxHp { get; private set; } = 100;
    [field: SerializeField] public float maxMp { get; private set; } = 100;
    [field: SerializeField] public int attackPower { get; private set; } = 10;
    [field: SerializeField] public int defense { get; private set; } = 0;
    [field: SerializeField] public float attackSpeed { get; private set; } = 1.0f;
    //public List<StatusEffect> statusEffectList = new List<StatusEffect>();
    [field: SerializeField] public float moveSpeed { get; private set; } = 1.0f;
    //public List<EquipWeaponId> = new List<EquipWeaponId>();
    //public List<equipArmorId> = new List<equipArmorId>();
    //public List<equipAccId> = new List<equipAccId>();
    [field: SerializeField] public int jumpCount { get; private set; } = 1;
    [field: SerializeField] public int dodgeCount { get; private set; } = 1;
    //public List<inventoryItemList> = new List<inventoryItemList>();
    //public List<effectItemList> = new List<effectItemList>();
    [field: SerializeField] public int usableSkillDicList { get; private set; }

}




[CreateAssetMenu(fileName = "Player", menuName = "Characters/Player")]
public class PlayerInfo : ScriptableObject
{
    [field: SerializeField] public PlayerGroundData GroundData { get; private set; }
    [field: SerializeField] public PlayerAirData AirData { get; private set; }
    [field: SerializeField] public PlayerAttackData AttackData { get; private set; }
    [field: SerializeField] public PlayerStatData StatData { get; private set; }
    [field: SerializeField] public PlayerSkillData SkillData { get; private set; }
}