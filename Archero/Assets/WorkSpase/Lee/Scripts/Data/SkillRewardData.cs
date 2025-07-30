using Lee.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Dash,
    Arrow,
    // 필요한 스킬추가 입력
}
[CreateAssetMenu(fileName = "SkillRewardData", menuName = "Reward/SkillUpgrade")]
public class SkillRewardData : RewardData
{
    [Header("Skill Type 설정")]
    [Tooltip("어떤 스킬을 강화할지 선택")]
    public SkillType skillType;

    [Tooltip("레벨 증가치 (보통 +1)")]
    public int levelGain = 1;

    [Tooltip("이 레벨(포함) 이상이면 더 이상 보상으로 안 나옵니다.")]
    public int maxLevel = 3;
}
