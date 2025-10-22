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
    // 리스트에 직접 접근하지 않고 안전하게 데이터를 가져오기 위한 접근자(Accessor) 역할
    // 리스트에 몇 개의 스킬이 들어있는지 개수를 반환
    // UI나 시스템에서 반복문을 돌며 모든 스킬 정보를 처리할 때 유용
    public AttackInfoData GetAttackInfoData(int index) => AttackInfoDatas[index];
    // 리스트의 특정 인덱스에 있는 SkillInfoData를 반환
}

[Serializable]
public class AttackInfoData
{
    [field: SerializeField, Tooltip("공격 에니메이션 이름")]
    public string AttackName { get; private set; }

    [field: SerializeField, Tooltip("데미지")]
    public float DamageMultiplier { get; private set; } = 1.0f;

    [field: SerializeField, Tooltip("다음 콤보 입력 가능 시작 시점 (0~1)")]
    [field: Range(0f, 1f)] public float ComboTimingStart { get; private set; } = 0.4f;

    [field: SerializeField, Tooltip("다음 콤보 입력 가능 종료 시점 (0~1)")]
    [field: Range(0f, 1f)] public float ComboTimingEnd { get; private set; } = 0.7f;

    [field: SerializeField, Tooltip("입력 버퍼 허용 시간 (초)")]
    [field: Range(0f, 0.5f)] public float InputBufferTime { get; private set; } = 0.2f;


    [field: SerializeField, Tooltip("앞으로 밀리는 시간")]
    [field: Range(0f, 3f)] public float ForceTransitionTime { get; private set; }

    [field: SerializeField, Tooltip("공격 시 캐릭터에 적용되는 힘")]
    [field: Range(-10f, 10f)] public float Force { get; private set; }

    [field: SerializeField, Tooltip("공격 판정 시작 시간(0~1)")]
    [field: Range(0f, 1f)] public float HitStartTime { get; private set; } = 0f;

    [field: SerializeField, Tooltip("공격 판정 종료 시간(0~1)")]
    [field: Range(0f, 1f)] public float HitEndTime { get; private set; } = 1f;

    [field: SerializeField, Tooltip("다단히트 공격 타수")]
    [field: Range(1, 10)] public int HitCount { get; private set; } = 1;

    [field: SerializeField, Tooltip("다단히트 간격(초)")]
    [field: Range(0f, 0.1f)] public float Interval { get; private set; } = 0.03f;

    [NonSerialized] public bool HasFired; // ComboHandler 내부 사용
}


[Serializable]
public class PlayerSkillData
{
    [field: SerializeField]
    public List<SkillInfoData> SkillInfoDatas { get; private set; } = new List<SkillInfoData>();
    public int GetSkillInfoCount() => SkillInfoDatas.Count;
    public SkillInfoData GetSkillInfoData(int index) => SkillInfoDatas[index];
}

[Serializable]
public class SkillInfoData
{
    [field: SerializeField, Tooltip("스킬 에니메이션 이름")]
    public string SkillName { get; private set; }

    [field: SerializeField, Tooltip("공격 타입")]
    public EffectType EffectType { get; private set; }

    [field: SerializeField, Tooltip("데미지")]
    public float DamageMultiplier { get; private set; } = 1.5f;

    [field: SerializeField, Tooltip("지속시간")]
    public float Duration { get; private set; }

    [field: SerializeField, Tooltip("넉백 거리")]
    public float KnockbackDistance { get; private set; }

    [field: SerializeField, Tooltip("쿨타임")]
    [field: Range(0f, 30f)] public float Cooldown { get; private set; } = 5f;

    [field: SerializeField, Tooltip("버퍼개수")]
    public int SkillCount { get; private set; } = 1;

    [field: SerializeField, Tooltip("Mp 소모량")]
    public int MpCost { get; private set; }


    [field: SerializeField, Tooltip("입력 버퍼 허용 시간 (초)")]
    [field: Range(0f, 0.5f)] public float InputBufferTime { get; private set; } = 0.2f;

    [field: SerializeField, Tooltip("앞으로 밀리는 시간")]
    [field: Range(0f, 3f)] public float ForceTransitionTime { get; private set; }

    [field: SerializeField, Tooltip("공격 시 캐릭터에 적용되는 힘")]
    [field: Range(-10f, 10f)] public float Force { get; private set; }

    [field: SerializeField, Tooltip("공격 판정 시작 시간(0~1)")]
    [field: Range(0f, 1f)] public float HitStartTime { get; private set; } = 0f;

    [field: SerializeField, Tooltip("공격 판정 종료 시간(0~1)")]
    [field: Range(0f, 1f)] public float HitEndTime { get; private set; } = 1f;

    [field: SerializeField, Tooltip("다단히트 공격 타수")]
    [field: Range(1, 10)] public int HitCount { get; private set; } = 1;

    [field: SerializeField, Tooltip("다단히트 간격(초)")]
    [field: Range(0f, 0.1f)] public float Interval { get; private set; } = 0.03f;
}



[Serializable]
public class PlayerStatData
{
    [field: SerializeField] public string Id { get; private set; } = "00000001";
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float MaxHp { get; private set; } = 100;
    [field: SerializeField] public float MaxMp { get; private set; } = 100;
    [field: SerializeField] public int AttackPower { get; private set; } = 10;
    [field: SerializeField] public int Defense { get; private set; } = 0;
    [field: SerializeField] public float AttackSpeed { get; private set; } = 1.0f;
    [field: SerializeField] public float MoveSpeed { get; private set; } = 1.0f;
    [field: SerializeField] public int JumpCount { get; private set; } = 1;
    [field: SerializeField] public int DodgeCount { get; private set; } = 4;
    [field: SerializeField] public int DodgeCooldown { get; private set; } = 4;
}




[CreateAssetMenu(fileName = "Player", menuName = "Characters/Player")]
public class PlayerInfo : ScriptableObject
{
    [field: SerializeField] public PlayerGroundData GroundData { get; private set; }
    [field: SerializeField] public PlayerAirData AirData { get; private set; }
    [field: SerializeField] public PlayerAttackData AttackData { get; private set; }
    [field: SerializeField] public PlayerSkillData SkillData { get; private set; }
    [field: SerializeField] public PlayerStatData StatData { get; private set; }

    // 특수 공격 SO를 단일 필드로 두고 캐릭터에 따라 다르게 할당
    [field: SerializeField] public ModuleDataBase ModuleData { get; private set; }
}