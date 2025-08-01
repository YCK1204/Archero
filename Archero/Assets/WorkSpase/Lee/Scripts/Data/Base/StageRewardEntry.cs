using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageRewardEntry
{
    [Header("UI������ ���")]
    public GameObject uiPrefab;

    [Header("���� ��ų ID (Skill.EffectID)")]
    public string skillEffectID;

    [Header("���� �̸�")]
    public string rewardInfo;

    [Header("��ų ���")]
    public ESkillGrade skillGrade;

    [Header("��ų ī�װ�")]
    public ESkillCategory skillCategory;

    [Header("�⺻ ��� ����ġ")]
    [Tooltip("���� Ŭ���� ���� Ȯ���� ������")]
    public float baseWeight = 1f;

    [Header("�⺻ �ۼ�Ʈ ���� ����")]
    [Tooltip("��: 0.1 = ���� ������ 10% ��ŭ �߰�")]
    public float percentageIncrease = 0.1f;

    [Header("���� ���Ȱ��� ���� �߰� ���� Ŀ��")]
    public AnimationCurve extraCurve = AnimationCurve.Constant(0f, 100f, 0.05f);

    [Header("(����)���� ����")]
    [TextArea] public string description;  // (����) ���� ����
}
