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

        private Dictionary<RewardType, RewardData> dataDict;

        void Awake()
        {
            if (rewardDatas == null || rewardDatas.Count == 0)
            {
                // Resources/Rewards 폴더 안에 있는 모든 RewardData를 로드
                var loaded = GameManager.Resource.LoadAll<RewardData>("Data");
                if (loaded != null && loaded.Length > 0)
                {
                    rewardDatas = loaded.ToList();
                    Debug.Log($"RewardManager: Resources에서 {loaded.Length}개 RewardData 로드");
                }
                else
                {
                    Debug.LogError("RewardManager: Resources/Rewards 폴더에 RewardData 에셋이 없습니다!");
                    // 이후 빈 리스트라도 남겨두고 종료
                    rewardDatas = new List<RewardData>();
                }
            }
            dataDict = new Dictionary<RewardType, RewardData>();
            foreach (var data in rewardDatas)
            {
                if (data == null)
                {
                    Debug.LogWarning("RewardManager: rewardDatas 목록에 null 항목이 있습니다. 건너뜁니다.");
                    continue;
                }
                if (dataDict.ContainsKey(data.type))
                {
                    Debug.LogWarning($"RewardManager: 중복된 RewardType({data.type}) 발견. 첫 번째만 사용합니다.");
                    continue;
                }
                dataDict.Add(data.type, data);
            }
        }

        // 모든 보상 타입 반환
        public List<RewardType> AllRewardTypes => dataDict.Keys.ToList();

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