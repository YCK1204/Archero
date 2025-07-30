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

        public List<RewardType> AllRewardTypes =>rewardDatas.Select(d => d.type).ToList();


    }

}