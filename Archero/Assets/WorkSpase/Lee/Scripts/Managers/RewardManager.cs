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
            // SkillManager가 초기화되었는지 확인
            if (SkillManager.Instance == null)
            {
                Debug.LogError("SkillManager가 초기화되지 않았습니다. ShowReward를 호출할 수 없습니다.");
                return;
            }

            // 모든 스킬 후보들을 가져오기 (grade 제한 제거하여 모든 등급이 섞여서 나오도록)
            var candidates = rewardDatas.SelectMany(so => so.rewardEntries).Where(e => e.skillCategory == category).ToList();

            // UI 슬롯 개수에 맞춰 보상 선택 (rewardSlots 개수만큼)
            int slotCount = GetRewardSlotCount(category);
            var picks = ReawrdSampling(candidates, slotCount);

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
            if (entry == null)
            {
                Debug.LogWarning("StageRewardEntry가 null입니다.");
                return;
            }

            if (SkillManager.Instance == null)
            {
                Debug.LogError("SkillManager.Instance가 null입니다. SkillManager가 초기화되지 않았습니다.");
                return;
            }

            var skill = SkillManager.Instance.GetSkillInfo(entry.skillEffectID, entry.skillGrade, entry.skillCategory);
            if (skill != null)
            {
                SkillManager.Instance.SelectSkill(skill);
            }
            else
            {
                Debug.LogWarning($"스킬을 찾을 수 없습니다: {entry.skillEffectID}, {entry.skillGrade}, {entry.skillCategory}");
            }
        }

        private List<StageRewardEntry> ReawrdSampling(List<StageRewardEntry> source, int count)
        {
            var result = new List<StageRewardEntry>();
            var pool = new List<StageRewardEntry>(source);

            // 요청된 개수만큼 선택
            for (int i = 0; i < count && pool.Count > 0; i++)
            {
                // pool이 비어있으면 다시 채우기
                if (pool.Count == 0)
                {
                    pool.AddRange(source);
                }

                float totalWeight = pool.Sum(e => e.baseWeight);
                float roll = UnityEngine.Random.Range(0f, totalWeight);
                float acc = 0f;

                foreach (var e in pool)
                {
                    acc += e.baseWeight;
                    if (roll <= acc)
                    {
                        result.Add(e);
                        pool.Remove(e); // 선택된 항목 제거
                        break;
                    }
                }
            }
            
            return result;
        }

        private int GetRewardSlotCount(ESkillCategory category)
        {
            // 각 카테고리별 UI의 rewardSlots 개수를 동적으로 가져오기
            string uiPath = GetUIPath(category);
            if (!string.IsNullOrEmpty(uiPath))
            {
                // 분류에 정해진 UI에 따라 Load 
                var uiPrefab = GameManager.Resource.Load<GameObject>(uiPath);
                if (uiPrefab != null)
                {
                    var rewardUI = uiPrefab.GetComponent<RewardSelectBaseUI>();
                    if (rewardUI != null)
                    {
                        // 리플렉션을 사용하여 rewardSlots 개수 가져오기
                        var rewardSlotsField = typeof(RewardSelectBaseUI).GetField("rewardSlots",  System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        
                        if (rewardSlotsField != null)
                        {
                            var rewardSlots = rewardSlotsField.GetValue(rewardUI) as List<RectTransform>;
                            if (rewardSlots != null)
                            {
                                return rewardSlots.Count;
                            }
                        }
                    }
                }
            }
            // 기본값 반환
            return 3;
        }

        private string GetUIPath(ESkillCategory category)
        {
            switch (category)
            {
                case ESkillCategory.LevelUp:
                    return commonUIPath;
                case ESkillCategory.Valkyrie:
                    return VkrUIPath;
                case ESkillCategory.Angel:
                    return AngleUIPath;
                case ESkillCategory.Devil:
                    return DevilUIPath;
                default:
                    return commonUIPath;
            }
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