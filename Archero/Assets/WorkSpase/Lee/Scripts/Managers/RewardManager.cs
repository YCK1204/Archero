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

        void Awake()
        {
            // �ν����� ��������� Resources �������� �ε�
            if (rewardDatas == null || rewardDatas.Count == 0)
            {
                var loaded = Resources.LoadAll<RewardData>("Data");
                rewardDatas = new List<RewardData>(loaded);
                Debug.Log($"RewardManager: {rewardDatas.Count}���� RewardData �ε�");
            }
        }

        // ��� ���� Ÿ�� ��ȯ
        public List<RewardData> AllRewardDatas => rewardDatas;

    }

}