using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lee.Scripts
{
    public class RewardData : ScriptableObject
    {
        [Header("Prefab 연결")]
        public GameObject prefab;

        [Header("이름")]
        public string rewarName;

        [Header("(선택)보상 설명")]
        [TextArea] public string description;  // (선택) 보상 설명

        [Header("기본 드롭 가중치")]
        [Tooltip("값이 클수록 뽑힐 확률이 높아짐")]
        public float baseWeight = 1f;
    }
}