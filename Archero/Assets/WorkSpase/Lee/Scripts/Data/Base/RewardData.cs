using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lee.Scripts
{
    public class RewardData : ScriptableObject
    {
        [Header("Prefab ����")]
        public GameObject prefab;

        [Header("�̸�")]
        public string rewarName;

        [Header("(����)���� ����")]
        [TextArea] public string description;  // (����) ���� ����

        [Header("�⺻ ��� ����ġ")]
        [Tooltip("���� Ŭ���� ���� Ȯ���� ������")]
        public float baseWeight = 1f;
    }
}