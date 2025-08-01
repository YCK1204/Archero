using Lee.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageRewardData))]
public class StageRewarDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        StageRewardData data = (StageRewardData)target;

        if (data.rewardEntries == null)
            data.rewardEntries = new List<StageRewardEntry>();

        SerializedProperty listProperty = serializedObject.FindProperty("rewardEntries");
        EditorGUILayout.PropertyField(listProperty, new GUIContent("Reward Entries"), true);

        serializedObject.ApplyModifiedProperties();

        if (Application.isPlaying && GameManager.SkillReward != null)
        {
            if (GUILayout.Button("동기화된 Skill 목록 보기"))
            {
                foreach (var skill in GameManager.SkillReward.GetAllSkills())
                {
                    Debug.Log($"[Skill] {skill.EffectID}, {skill.Grade}, {skill.Category}");
                }
            }
        }
    }
}
