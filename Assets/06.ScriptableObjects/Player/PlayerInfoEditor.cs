#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerInfo))]
public class PlayerInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerInfo playerInfo = (PlayerInfo)target;
        if (GUILayout.Button("Reset Values"))
        {
            //playerInfo.GroundData = new PlayerGroundData();
            //playerInfo.AirData = new PlayerAirData();
            //playerInfo.AttackData = new PlayerAttackData();
            //playerInfo.SkillData = new PlayerSkillData();
            //playerInfo.StatData = new PlayerStatData();

            EditorUtility.SetDirty(playerInfo); // 저장 가능 상태 표시
        }
    }
}
#endif
