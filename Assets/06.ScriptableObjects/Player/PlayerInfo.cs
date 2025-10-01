using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class PlayerGroundData
{
    [field: Header("IdleData")]
    [field: SerializeField,Tooltip("캐릭터가 회전할 때 감속되는 정도")]
    [field: Range(0f, 50f)] public float BaseRotationDamping { get; private set; } = 25f;


    [field: Header("WalkData")]
    [field: SerializeField, Tooltip("기본 이동 속도")]
    [field: Range(0f, 5f)] public float BaseSpeed { get; private set; } = 1f;

    [field: SerializeField, Tooltip("걷기 속도 보정 값")]
    [field: Range(0f, 2f)] public float WalkSpeedModifier { get; private set; } = 0.225f;

    [field: SerializeField, Tooltip("달리기 가속 시간")]
    [field: Range(0f, 5f)] public float RunAccelerationTime { get; private set; } = 2f;

    [field: Header("RunData")]
    [field: SerializeField, Tooltip("달리기 속도 보정 값")]
    [field: Range(0f, 2f)] public float RunSpeedModifier { get; private set; } = 1f;

    [field: Header("DodgeData")]
    [field: SerializeField, Tooltip("회피 상태 유지 시간")]
    [field: Range(0.1f, 5f)] public float DodgeDuration { get; private set; } = 0.8f;

    [field: SerializeField, Tooltip("캐릭터가 회피할 때 가해지는 힘의 크기")]
    [field: Range(0.1f, 20f)] public float DodgeStrength { get; private set; } = 6f;

    [field: SerializeField, Tooltip("회피 애니메이션 레이어 블렌딩 속도")]
    [field: Range(1f, 20f)] public float DodgeLayerBlendSpeed { get; private set; } = 5f;
}

[Serializable]
public class PlayerAirData
{
    [field: Header("JumpData")]
    [field: SerializeField, Tooltip("점프 시 가해지는 힘")]
    [field: Range(0f, 25f)] public float JumpForce { get; private set; } = 5f;

    // TODO: 이중 점프 여부, 점프 소모량에 대한 Tooltip 추가 가능
}

[Serializable]
public class PlayerDefenseData
{
    // TODO: 방어 관련 설정 Tooltip 추가 가능
}


[Serializable]
public class PlayerAttackData
{
    [field: SerializeField, Tooltip("공격 범위")]
    [field: Range(0f, 10f)] public float AttackRange { get; private set; } = 5f;

    [field: SerializeField, Tooltip("돌진 속도")]
    [field: Range(0f, 10f)] public float DashSpeed { get; private set; } = 5f;

    [field: SerializeField, Tooltip("돌진 거리")]
    [field: Range(0f, 10f)] public float StopDistance { get; private set; } = 3f;

    [field: SerializeField, Tooltip("공격 정보 리스트")]
    public List<AttackInfoData> AttackInfoDatas { get; private set; } = new List<AttackInfoData>();
    public int GetAttackInfoCount() => AttackInfoDatas.Count;
    public AttackInfoData GetAttackInfoData(int index) => AttackInfoDatas[index];
}

[Serializable]
public class AttackInfoData
{
    [field: SerializeField, Tooltip("공격 에니메이션 이름")]
    public string AttackName { get; private set; }

    [field: SerializeField, Tooltip("다음 콤보 입력 가능 시작 시점 (0~1)")]
    [field: Range(0f, 1f)] public float ComboTimingStart { get; private set; } = 0.6f;

    [field: SerializeField, Tooltip("다음 콤보 입력 가능 종료 시점 (0~1)")]
    [field: Range(0f, 1f)] public float ComboTimingEnd { get; private set; } = 0.7f;

    [field: SerializeField, Tooltip("입력 버퍼 허용 시간 (초)")]
    [field: Range(0f, 0.5f)] public float InputBufferTime { get; private set; } = 0.2f;


    [field: SerializeField, Tooltip("앞으로 밀리는 시간")]
    [field: Range(0f, 3f)] public float ForceTransitionTime { get; private set; }

    [field: SerializeField, Tooltip("공격 시 캐릭터에 적용되는 힘")]
    [field: Range(-10f, 10f)] public float Force { get; private set; } = 1f;

    [field: SerializeField, Tooltip("공격 판정 시작 시간(0~1)")]
    [field: Range(0f, 1f)] public float Dealing_Start_TransitionTime { get; private set; }

    [field: SerializeField, Tooltip("공격 판정 종료 시간(0~1)")]
    [field: Range(0f, 1f)] public float Dealing_End_TransitionTime { get; private set; }
}

public enum AttackType
{
    Lite,
    Heavy,
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
    [field: SerializeField] public float range { get; private set; }
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
    [field: SerializeField] public string id { get; private set; } = "00000001";
    [field: SerializeField] public string name { get; private set; }
    [field: SerializeField] public float maxHp { get; private set; } = 100;
    [field: SerializeField] public float maxMp { get; private set; } = 100;
    [field: SerializeField] public int attackPower { get; private set; } = 10;
    [field: SerializeField] public int defense { get; private set; } = 0;
    [field: SerializeField] public float attackSpeed { get; private set; } = 1.0f;
    [field: SerializeField] public float moveSpeed { get; private set; } = 1.0f;
    [field: SerializeField] public int jumpCount { get; private set; } = 1;
    [field: SerializeField] public int dodgeCount { get; private set; } = 1;
    //public List<inventoryItemList> = new List<inventoryItemList>();
    //public List<effectItemList> = new List<effectItemList>();
    [field: SerializeField] public string usableSkillDicList { get; private set; } = "70000000";

}




[CreateAssetMenu(fileName = "Player", menuName = "Characters/Player")]
public class PlayerInfo : ScriptableObject
{
    [field: SerializeField] public PlayerGroundData GroundData { get; private set; }
    [field: SerializeField] public PlayerAirData AirData { get; private set; }
    [field: SerializeField] public PlayerAttackData AttackData { get; private set; }
    [field: SerializeField] public PlayerSkillData SkillData { get; private set; }
    [field: SerializeField] public PlayerStatData StatData { get; private set; }
}