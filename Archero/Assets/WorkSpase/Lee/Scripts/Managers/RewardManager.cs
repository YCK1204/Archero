using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lee.Scripts
{
    public class RewardManager : MonoBehaviour
    {
        [Header("생성한 RewardData 에셋들")]
        [SerializeField] List<StageRewardData> rewardDatas;
        string commonUIPath = "Prefabs/UI/RewardUI";
        string VkrUIPath = "Prefabs/UI/VkrRewardUI";
        string AngleUIPath = "Prefabs/UI/AngleRewardUI";
        string DevilUIPath = "Prefabs/UI/DevilRewardUI";



        void Awake()
        {
            // 인스펙터 비어있으면 Resources 폴더에서 로드
            if (rewardDatas == null || rewardDatas.Count == 0)
            {
                var loaded = Resources.LoadAll<StageRewardData>("Data");
                rewardDatas = new List<StageRewardData>(loaded);
                Debug.Log($"RewardManager: {rewardDatas.Count}개의 StageRewardData 로드");
            }
        }

        // 모든 보상 타입 반환
        public List<StageRewardData> AllRewardDatas => rewardDatas;

        public void ShowRewardSelection(ESkillGrade grade, ESkillCategory category, int pickCount)
        {

        }

        private void OnRewardChosen(StageRewardEntry entry)
        {
            // 1) 스킬 적용
            var skill = SkillManager.Instance.GetSkillInfo(entry.skillEffectID, entry.skillGrade, entry.skillCategory);
            if (skill != null) SkillManager.Instance.SelectSkill(skill);
        }


















        private void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }











}