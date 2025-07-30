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
        public RewardType type;      // enum Ű
        public GameObject prefab;    // ������ ������ (��ư���������� ���Ե� UI Prefab)
    }

    public class RewardManager : MonoBehaviour
    {
        [Header("������")]
        public List<RewardPrefab> rewardPrefabsList;

        private Dictionary<RewardType, GameObject> _prefabDict;

        GameObject playerStat;


        private void Awake()
        {
            _prefabDict = rewardPrefabsList.ToDictionary(x => x.type, x => x.prefab);
            playerStat = GameObject.FindGameObjectWithTag("Player");
        }

        // ��ü ���� ����Ʈ ��ȯ
        public List<RewardType> AllRewardTypes =>rewardPrefabsList.Select(x => x.type).ToList();

        // Ư�� ����Prefab ��ȯ
        public GameObject GetPrefab(RewardType type)
        {
            if (_prefabDict.TryGetValue(type, out var prefab))
                return prefab;
            Debug.LogError($"RewardManager: {type} �� Prefab�� �����ϴ�.");
            return null;
        }
    }

}