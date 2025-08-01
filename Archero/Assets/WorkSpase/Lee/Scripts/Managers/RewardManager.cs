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

        [Header("스킬 최대 레벨")]
        public int maxSkillLevel = 3;

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

        // 플레이어가 가지고 있는 스킬들 탐색후 스킬레벨 int값 반환하는 로직 필요
/*
        public List<StageRewardData> GetRandomRewards(int count)// 스킬레벨 생길시 구문으로 변경(Func<RewardData, int> getSkillLevel, int count)
        {
            // 스킬 보상만 필터링
           // var skillCandidates = rewardDatas.OfType<SkillRewardData>().Where(d => getSkillLevel(d) < maxSkillLevel).Cast<RewardData>().ToList();

            // 스킬 보상
            var skillCandidates = rewardDatas.OfType<SkillRewardData>().Cast<RewardData>().ToList();
            // 모든 스탯 보상 후보
            var statCandidates = rewardDatas.OfType<StatRewardData>().Cast<RewardData>().ToList();

            // 둘다 셔플
            Shuffle(skillCandidates);
            Shuffle(statCandidates);

            //덱 픽
            var picks = new List<StageRewardData>();

            // 스킬보상 부터 최대 Count까지 추가
            picks.AddRange(skillCandidates.Take(count));

            // 스킬보상이 부족하면 스탯보상으로 채우기
            if (picks.Count < count)
                picks.AddRange(statCandidates.Take(count - picks.Count));

            // 다시 섞기
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

        // 플레이어 스탯 보상량 계산
        public int CalculateStatBonus(StageRewardEntry data, int currentStat)
        {
            float bonus = currentStat * data.percentageIncrease;        // 현재 플레이어 스탯에서 x퍼센트만큼 계산(ex. 현재스탯의 10퍼센트 계산)
            bonus += data.extraCurve.Evaluate(currentStat) * currentStat;   //ex.Evaluate(75) = 0.15 * 75
            return Mathf.RoundToInt(bonus); // 반환형 int인 소수점 첫째짜리까지 반올림함
        }
    }











}