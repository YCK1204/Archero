using Lee.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageRewardData", menuName = "Rewards/StageRewardData")]
public class StageRewardData : ScriptableObject
{
    public List<StageRewardEntry> rewardEntries;

    public StageRewardEntry GetRewardForStage(int clearCount)
    {
        return rewardEntries.Find(entry => entry.clearCount == clearCount);
    }
    public Skill GetSkillReward(StageRewardEntry entry)
    {
        return GameManager.SkillReward.GetSkillInfo(entry.skillEffectID, entry.skillGrade, entry.skillCategory);
    }
}