using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageRewardEntry
{
    public int clearCount;

    [Header("보상 설명")]
    public string rewardInfo;

    [Header("보상 스킬 ID (Skill.EffectID)")]
    public string skillEffectID;

    [Header("스킬 등급")]
    public ESkillGrade skillGrade;

    [Header("스킬 카테고리")]
    public ESkillCategory skillCategory;
}
