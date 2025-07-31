using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Collectable/Potion")]
public class PotionItemData : CollectableItemData
{
    public int HealAmount;
    PotionItemData()
    {
        OnCollected += OnCollect;
    }
    void OnCollect()
    {
        // 플레이어에게 경험치 추가
        //Player player = FindObjectOfType<Player>();
        //if (player != null)
        //{
        //    player.Heal(HealAmount);
        //}
    }
}
