using System;
using System.Collections;
using System.Collections.Generic;
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

        // ��� ���� Ÿ�� ��ȯ
        public List<StageRewardData> AllRewardDatas => rewardDatas;

        public void ShowRewardSelection(ESkillGrade grade, ESkillCategory category, int pickCount)
        {

        }

        private void OnRewardChosen(StageRewardEntry entry)
        {
            // 1) ��ų ����
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