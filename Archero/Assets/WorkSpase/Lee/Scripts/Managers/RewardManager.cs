using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lee.Scripts
{
    public class RewardManager : MonoBehaviour
    {
        [Header("생성한 RewardData 에셋들")]
        [SerializeField]
         private List<RewardData> rewardDatas = new List<RewardData>();

        void Awake()
        {
            // 인스펙터 비어있으면 Resources 폴더에서 로드
            if (rewardDatas == null || rewardDatas.Count == 0)
            {
                var loaded = Resources.LoadAll<RewardData>("Data");
                rewardDatas = new List<RewardData>(loaded);
                Debug.Log($"RewardManager: {rewardDatas.Count}개의 RewardData 로드");
            }
        }

        // 모든 보상 타입 반환
        public List<RewardData> AllRewardDatas => rewardDatas;

    }

}