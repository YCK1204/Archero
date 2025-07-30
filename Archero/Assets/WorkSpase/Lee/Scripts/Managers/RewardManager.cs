using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lee.Scripts
{
    public class RewardManager : MonoBehaviour
    {
        [Header("생성한 RewardData 에셋들")]
        public List<RewardData> rewardDatas;

        private Dictionary<RewardType, RewardData> dataDict;

        void Awake()
        {
            dataDict = rewardDatas.ToDictionary(d => d.type, d => d);
        }

        // 모든 보상 타입 반환
        public List<RewardType> AllRewardTypes =>rewardDatas.Select(d => d.type).ToList();

        // 특정 보상타입 반환
        public RewardData GetData(RewardType type)
        {
            if (dataDict.TryGetValue(type, out var data))
             return data;
            Debug.LogError($"RewardManager: {type}에 매핑된 RewardData가 없습니다.");
            return null;
        }
    }

}