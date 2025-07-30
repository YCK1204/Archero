using Lee.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Dash,
    Arrow,
    // �ʿ��� ��ų�߰� �Է�
}
[CreateAssetMenu(fileName = "SkillRewardData", menuName = "Reward/SkillUpgrade")]
public class SkillRewardData : RewardData
{
    [Header("Skill Type ����")]
    [Tooltip("� ��ų�� ��ȭ���� ����")]
    public SkillType skillType;

    [Tooltip("���� ����ġ (���� +1)")]
    public int levelGain = 1;

    [Tooltip("�� ����(����) �̻��̸� �� �̻� �������� �� ���ɴϴ�.")]
    public int maxLevel = 3;
}
