using Lee.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageRewardData", menuName = "Reward/StageRewardData")]
public class StageRewardData : ScriptableObject
{
    public List<StageRewardEntry> rewardEntries;

    [Tooltip("Index = �� ��ȣ (0=ù ��, 1=�� ��° ��, ��)")]
    public StageRewardEntry GetRewardForStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= rewardEntries.Count)
            return null;
        return rewardEntries[stageIndex];
    }

    public Skill GetSkillReward(StageRewardEntry entry)
    {
        return GameManager.SkillReward.GetSkillInfo(entry.skillEffectID, entry.skillGrade, entry.skillCategory);
    }
}