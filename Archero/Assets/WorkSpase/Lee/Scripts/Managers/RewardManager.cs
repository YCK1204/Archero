using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lee.Scripts
{
    public class RewardManager : MonoBehaviour
    {
        [Header("������ RewardData ���µ�")]
        public List<RewardData> rewardDatas;

        private Dictionary<RewardType, RewardData> dataDict;

        void Awake()
        {
            dataDict = rewardDatas.ToDictionary(d => d.type, d => d);
        }

        // ��� ���� Ÿ�� ��ȯ
        public List<RewardType> AllRewardTypes =>rewardDatas.Select(d => d.type).ToList();

        // Ư�� ����Ÿ�� ��ȯ
        public RewardData GetData(RewardType type)
        {
            if (dataDict.TryGetValue(type, out var data))
             return data;
            Debug.LogError($"RewardManager: {type}�� ���ε� RewardData�� �����ϴ�.");
            return null;
        }
    }

}