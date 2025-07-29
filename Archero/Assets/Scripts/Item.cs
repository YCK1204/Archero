using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public BaseItemData itemData;
    Animator _animator => itemData.Animator;
    Sprite _icon => itemData.Icon;
    string _name => itemData.Name;
    ItemType _type => itemData.Type;
    public void Use()
    {
        switch (_type)
        {
            case ItemType.Collectable:
                CollectableItemData collectableItem = itemData as CollectableItemData;
                collectableItem.Collect();
                break;
            case ItemType.Equippable:
                EquippableItemData equippableItem = itemData as EquippableItemData;
                // equip ¶Ç´Â unequip toggle·Î
                break;
        }
    }
}
