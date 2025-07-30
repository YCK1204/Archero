using Lee.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Health,
    Attack,
    Defense,
    Speed,
    Exp,
    Gold,
    // �ʿ��� ������ �߰��ϼ���
}
[CreateAssetMenu(fileName = "StatRewardData", menuName = "Reward/StatBonus")]
public class StatRewardData : RewardData
{
    [Header("Stat ���� ����")]
    [Tooltip("� ������ ������ų�� ����")]
    public StatType statType;

    [Header("�⺻ �ۼ�Ʈ ���� ����")]
    [Tooltip("��: 0.1 = ���� ������ 10% ��ŭ �߰�")]
    public float percentageIncrease = 0.1f;

    [Header("���� ���Ȱ��� ���� �߰� ���� Ŀ��")]
    public AnimationCurve extraCurve = AnimationCurve.Constant(0f, 100f, 0.05f);
}

