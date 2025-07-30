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

        public List<RewardType> AllRewardTypes =>rewardDatas.Select(d => d.type).ToList();


    }

}