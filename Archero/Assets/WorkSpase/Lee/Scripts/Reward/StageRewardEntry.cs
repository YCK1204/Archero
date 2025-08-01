using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageRewardEntry
{
    public int clearCount;

    [Header("���� ����")]
    public string rewardInfo;

    [Header("���� ��ų ID (Skill.EffectID)")]
    public string skillEffectID;

    [Header("��ų ���")]
    public ESkillGrade skillGrade;

    [Header("��ų ī�װ�")]
    public ESkillCategory skillCategory;
}
