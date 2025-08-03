using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        public void ShowReward(ESkillGrade grade, ESkillCategory category, int pickCount)
        {
            // 모든 스킬 후보들을 가져오기 (grade 제한 제거하여 모든 등급이 섞여서 나오도록)
            var candidates = rewardDatas.SelectMany(so => so.rewardEntries).Where(e => e.skillCategory == category).ToList();

            //가중치를 이용한 랜덤 선택(중복제거)
            var picks = ReawrdSampling(candidates, pickCount);

            //섞기
            Shuffle(picks);

           switch(category)
            {
                case ESkillCategory.LevelUp:
                    {
                        // common 방 전용 UI
                        GameManager.UI.ShowPopUpUI<RewardSelect_PopUpUI>(commonUIPath).Initialize(picks, ChoseReward);
                    }
                    break;

                case ESkillCategory.Valkyrie:
                    {
                        // 발키리 방 전용 UI
                        GameManager.UI.ShowPopUpUI<VkrRewardSelect_PopUpUI>(VkrUIPath).Initialize(picks, ChoseReward);
                    }
                    break;

                case ESkillCategory.Angel:
                    {
                        // 천사 방 전용 UI
                        GameManager.UI.ShowPopUpUI<AngleRewardSelect_PopUpUI>(AngleUIPath).Initialize(picks, ChoseReward);
                    }
                    break;

                case ESkillCategory.Devil:
                    {
                        // 악마 방 전용 UI
                        GameManager.UI.ShowPopUpUI<DevilRewardSelect_PopUpUI>(DevilUIPath).Initialize(picks, ChoseReward);
                    }
                    break;
            };
        }

        // UI 결정 버튼 클릭시 적용되는 함수
        private void ChoseReward(StageRewardEntry entry)
        {
            var skill = SkillManager.Instance.GetSkillInfo(entry.skillEffectID, entry.skillGrade, entry.skillCategory);
            if (skill != null) SkillManager.Instance.SelectSkill(skill);
        }

        private List<StageRewardEntry> ReawrdSampling(List<StageRewardEntry> source, int count)
        {
            var pool = new List<StageRewardEntry>(source);
            var result = new List<StageRewardEntry>();

            for(int i = 0; i < count && pool.Count > 0; i++)
        {
                float totalWeight = pool.Sum(e => e.baseWeight);
                float roll = UnityEngine.Random.Range(0f, totalWeight);
                float acc = 0f;

                foreach (var e in pool)
                {
                    acc += e.baseWeight;
                    if (roll <= acc)
                    {
                        result.Add(e);
                        pool.Remove(e);
                        break;
                    }
                }
            }
            return result;
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