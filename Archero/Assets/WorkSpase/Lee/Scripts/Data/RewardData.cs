using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lee.Scripts
{
    public enum RewardType
    {
        Health,
        Damage,
        Shield,
        Speed,
        Gold,
        Exp,
        // �������� �߰��� �Է�


    }


    [CreateAssetMenu(fileName = "RewardData", menuName = "Reward/Reward Data")]
    public class RewardData : ScriptableObject
    {
        [Header("Ÿ�Լ���")]
        public RewardType type;
        [Header("Prefab ����")]
        public GameObject prefab;
        [Header("�̸�")]
        public string rewarName;
        [Header("(����)���� ����")]
        [TextArea] public string description;  // (����) ���� ����
    }

}