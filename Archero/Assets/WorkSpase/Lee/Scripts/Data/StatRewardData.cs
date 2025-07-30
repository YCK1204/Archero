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
    // 필요한 스탯을 추가하세요
}
[CreateAssetMenu(fileName = "StatRewardData", menuName = "Reward/StatBonus")]
public class StatRewardData : RewardData
{
    [Header("Stat 보상 설정")]
    [Tooltip("어떤 스탯을 증가시킬지 선택")]
    public StatType statType;

    [Header("기본 퍼센트 증가 비율")]
    [Tooltip("예: 0.1 = 현재 스탯의 10% 만큼 추가")]
    public float percentageIncrease = 0.1f;

    [Header("현재 스탯값에 따른 추가 보정 커브")]
    public AnimationCurve extraCurve = AnimationCurve.Constant(0f, 100f, 0.05f);
}

