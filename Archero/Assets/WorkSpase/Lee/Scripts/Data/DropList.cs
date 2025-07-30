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
        public int valueInt; // 경험치 수치, 골드 수치, 체력 회복량 등
        public float valueFloat; // 경험치 수치, 골드 수치, 체력 회복량 등
    }

}