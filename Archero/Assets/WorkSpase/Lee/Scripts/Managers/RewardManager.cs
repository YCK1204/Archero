using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lee.Scripts
{
    public enum RewardType
    {
        HPUp,
        DamageUp,
        Shield,
        SpeedBoost,
        GoldBonuss,
    }
    public struct RewardPrefab
    {
        public RewardType type;      // enum 키
        public GameObject prefab;    // 연결할 프리팹 (버튼·아이콘이 포함된 UI Prefab)
    }

    public class RewardManager : MonoBehaviour
    {
        [Header("보상목록")]
        public List<RewardPrefab> rewardPrefabsList;

        private Dictionary<RewardType, GameObject> _prefabDict;

        GameObject playerStat;


        private void Awake()
        {
            _prefabDict = rewardPrefabsList.ToDictionary(x => x.type, x => x.prefab);
            playerStat = GameObject.FindGameObjectWithTag("Player");
        }

        // 전체 보상 리스트 반환
        public List<RewardType> AllRewardTypes =>rewardPrefabsList.Select(x => x.type).ToList();

        // 특정 보상Prefab 반환
        public GameObject GetPrefab(RewardType type)
        {
            if (_prefabDict.TryGetValue(type, out var prefab))
                return prefab;
            Debug.LogError($"RewardManager: {type} 에 Prefab이 없습니다.");
            return null;
        }
    }

}