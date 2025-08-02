using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageRewardEntry
{
    [Header("UI프리팹 등록")]
    public GameObject uiPrefab;

    [Header("보상 스킬 ID (Skill.EffectID)")]
    public string skillEffectID;

    [Header("보상 이름")]
    public string rewardInfo;

    [Header("스킬 등급")]
    public ESkillGrade skillGrade;

    [Header("스킬 카테고리")]
    public ESkillCategory skillCategory;

    [Header("기본 드롭 가중치")]
    [Tooltip("값이 클수록 뽑힐 확률이 높아짐")]
    public float baseWeight = 1f;

    [Header("(선택)보상 설명")]
    [TextArea] public string description;  // (선택) 보상 설명
}
