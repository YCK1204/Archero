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
                if (loaded != null && loaded.Length > 0)
                {
                    rewardDatas = loaded.ToList();
                    Debug.Log($"RewardManager: Resources���� {loaded.Length}�� RewardData �ε�");
                }
                else
                {
                    Debug.LogError("RewardManager: Resources/Rewards ������ RewardData ������ �����ϴ�!");
                    // ���� �� ����Ʈ�� ���ܵΰ� ����
                    rewardDatas = new List<RewardData>();
                }
            }
            dataDict = new Dictionary<RewardType, RewardData>();
            foreach (var data in rewardDatas)
            {
                if (data == null)
                {
                    Debug.LogWarning("RewardManager: rewardDatas ��Ͽ� null �׸��� �ֽ��ϴ�. �ǳʶݴϴ�.");
                    continue;
                }
                if (dataDict.ContainsKey(data.type))
                {
                    Debug.LogWarning($"RewardManager: �ߺ��� RewardType({data.type}) �߰�. ù ��°�� ����մϴ�.");
                    continue;
                }
                dataDict.Add(data.type, data);
            }
        }

        // ��� ���� Ÿ�� ��ȯ
        public List<RewardType> AllRewardTypes => dataDict.Keys.ToList();

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