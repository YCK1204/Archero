using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lee.Scripts
{
    public enum DropItemType { EXP, GOLD, HEAL }
    [CreateAssetMenu(menuName = "DropItem/DropItemData")]
    public class DropItemData : ScriptableObject
    {
        public DropItemType itemType;
        public string itemName;
        public Sprite icon;
        public GameObject prefab;

        [Header("Drop Settings")]
        public int valueInt; // ����ġ ��ġ, ��� ��ġ, ü�� ȸ���� ��
        public float valueFloat; // ����ġ ��ġ, ��� ��ġ, ü�� ȸ���� ��
    }

}