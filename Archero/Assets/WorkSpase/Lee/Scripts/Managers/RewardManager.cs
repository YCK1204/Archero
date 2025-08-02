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
        [Header("������ RewardData ���µ�")]
        [SerializeField] List<StageRewardData> rewardDatas;
        string commonUIPath = "Prefabs/UI/RewardUI";
        string VkrUIPath = "Prefabs/UI/VkrRewardUI";
        string AngleUIPath = "Prefabs/UI/AngleRewardUI";
        string DevilUIPath = "Prefabs/UI/DevilRewardUI";

        void Awake()
        {
            // �ν����� ��������� Resources �������� �ε�
            if (rewardDatas == null || rewardDatas.Count == 0)
            {
                var loaded = Resources.LoadAll<StageRewardData>("Data");
                rewardDatas = new List<StageRewardData>(loaded);
                Debug.Log($"RewardManager: {rewardDatas.Count}���� StageRewardData �ε�");
            }
        }

        public void ShowReward(ESkillGrade grade, ESkillCategory category, int pickCount)
        {
            // ��� ��ų ��Ʈ�� ����
            var candidates = rewardDatas.SelectMany(so => so.rewardEntries).Where(e => e.skillGrade == grade && e.skillCategory == category).ToList();

            //����ġ�� ���� ���� ���ø�(�ߺ�����)
            var picks = ReawrdSampling(candidates, pickCount);

            //����
            Shuffle(picks);

           switch(category)
            {
                case ESkillCategory.LevelUp:
                    {
                        // common �� ���� UI
                        GameManager.UI.ShowPopUpUI<RewardSelect_PopUpUI>(commonUIPath).Initialize(picks, ChoseReward);
                    }
                    break;

                case ESkillCategory.Valkyrie:
                    {
                        // ��Ű�� �� ���� UI
                        GameManager.UI.ShowPopUpUI<VkrRewardSelect_PopUpUI>(VkrUIPath).Initialize(picks, ChoseReward);
                    }
                    break;

                case ESkillCategory.Angel:
                    {
                        // õ�� �� ���� UI
                        GameManager.UI.ShowPopUpUI<AngleRewardSelect_PopUpUI>(AngleUIPath).Initialize(picks, ChoseReward);
                    }
                    break;

                case ESkillCategory.Devil:
                    {
                        // �Ǹ� �� ���� UI
                        GameManager.UI.ShowPopUpUI<DevilRewardSelect_PopUpUI>(DevilUIPath).Initialize(picks, ChoseReward);
                    }
                    break;
            };
        }

        // UI ���� ��ư Ŭ���� ����Ǵ� �Լ�
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