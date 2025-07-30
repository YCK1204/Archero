using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lee.Scripts
{
    public class RewardManager : MonoBehaviour
    {
        [Header("������ RewardData ���µ�")]
        [SerializeField]
         private List<RewardData> rewardDatas = new List<RewardData>();

        private Dictionary<RewardType, RewardData> dataDict;

        void Awake()
        {
            if (rewardDatas == null || rewardDatas.Count == 0)
            {
                // Resources/Rewards ���� �ȿ� �ִ� ��� RewardData�� �ε�
                var loaded = GameManager.Resource.LoadAll<RewardData>("Data");
                rewardDatas = new List<RewardData>(loaded);
            }

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