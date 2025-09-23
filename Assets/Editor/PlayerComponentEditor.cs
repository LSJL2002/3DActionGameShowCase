using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerComponent))]
public class PlayerComponentEditor : Editor
{
    SerializedProperty dodgeStrengthProp;

    private void OnEnable()
    {
        dodgeStrengthProp = serializedObject.FindProperty("DodgeStrength");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUIStyle tooltipStyle = new GUIStyle(EditorStyles.label);
        tooltipStyle.normal.textColor = Color.yellow;
        tooltipStyle.fontSize = 14;

        // 레이블 + 툴팁
        EditorGUILayout.LabelField(new GUIContent("Dodge Strength", "캐릭터가 회피할 때 가해지는 힘"), tooltipStyle);

        // Slider
        EditorGUILayout.Slider(dodgeStrengthProp, 0f, 20f);

        serializedObject.ApplyModifiedProperties();
    }
}