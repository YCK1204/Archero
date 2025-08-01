using Lee.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageRewardData", menuName = "Reward/StageRewardData")]
public class StageRewardData : ScriptableObject
{
    public List<StageRewardEntry> rewardEntries;

    [Tooltip("Index = 방 번호 (0=첫 방, 1=두 번째 방, …)")]
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