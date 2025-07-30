using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lee.Scripts
{
    public enum RewardType
    {
        Stat,
        Skill,
    }


    [CreateAssetMenu(fileName = "RewardData", menuName = "Reward/Reward Data")]
    public class RewardData : ScriptableObject
    {
        [Header("타입설정")]
        public RewardType type;
        [Header("Prefab 연결")]
        public GameObject prefab;
        [Header("이름")]
        public string rewarName;
        [Header("(선택)보상 설명")]
        [TextArea] public string description;  // (선택) 보상 설명
        public float baseWeight = 1f;

    }

}