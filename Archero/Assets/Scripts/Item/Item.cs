using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    BaseItemData itemData;
    public Animator Animator => itemData.Animator;
    public Sprite Icon => itemData.Icon;
    public string Name => itemData.Name;
    ItemType Type => itemData.Type;
    public Item(BaseItemData data)
    {
        itemData = data;
    }
    /// <summary>
    /// 아이템을 타입에 따라 사용합니다.
    /// </summary>
    /// <param name = "player">플레이어</param>
    public void Use(/*Player player*/)
    {
        switch (Type)
        {
            case ItemType.Collectable:
                CollectableItemData collectableItem = itemData as CollectableItemData;
                collectableItem.Collect();
                //collectableItem.Collect(player);
                break;
            case ItemType.Equippable:
                EquippableItemData equippableItem = itemData as EquippableItemData;
                // equip 또는 unequip toggle로
                break;
        }
    }
}
