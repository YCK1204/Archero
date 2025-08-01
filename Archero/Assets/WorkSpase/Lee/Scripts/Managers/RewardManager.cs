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

        [Header("��ų �ִ� ����")]
        public int maxSkillLevel = 3;

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

        // �÷��̾ ������ �ִ� ��ų�� Ž���� ��ų���� int�� ��ȯ�ϴ� ���� �ʿ�
/*
        public List<StageRewardData> GetRandomRewards(int count)// ��ų���� ����� �������� ����(Func<RewardData, int> getSkillLevel, int count)
        {
            // ��ų ���� ���͸�
           // var skillCandidates = rewardDatas.OfType<SkillRewardData>().Where(d => getSkillLevel(d) < maxSkillLevel).Cast<RewardData>().ToList();

            // ��ų ����
            var skillCandidates = rewardDatas.OfType<SkillRewardData>().Cast<RewardData>().ToList();
            // ��� ���� ���� �ĺ�
            var statCandidates = rewardDatas.OfType<StatRewardData>().Cast<RewardData>().ToList();

            // �Ѵ� ����
            Shuffle(skillCandidates);
            Shuffle(statCandidates);

            //�� ��
            var picks = new List<StageRewardData>();

            // ��ų���� ���� �ִ� Count���� �߰�
            picks.AddRange(skillCandidates.Take(count));

            // ��ų������ �����ϸ� ���Ⱥ������� ä���
            if (picks.Count < count)
                picks.AddRange(statCandidates.Take(count - picks.Count));

            // �ٽ� ����
            Shuffle(picks);
            return picks;
        }
*/
        private void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        // �÷��̾� ���� ���� ���
        public int CalculateStatBonus(StageRewardEntry data, int currentStat)
        {
            float bonus = currentStat * data.percentageIncrease;        // ���� �÷��̾� ���ȿ��� x�ۼ�Ʈ��ŭ ���(ex. ���罺���� 10�ۼ�Ʈ ���)
            bonus += data.extraCurve.Evaluate(currentStat) * currentStat;   //ex.Evaluate(75) = 0.15 * 75
            return Mathf.RoundToInt(bonus); // ��ȯ�� int�� �Ҽ��� ù°¥������ �ݿø���
        }
    }











}