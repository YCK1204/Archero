using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ItemType
{
    Collectable,
    Equippable,
}

public abstract class BaseItemData : ScriptableObject
{
    public string Name; // 아이템 이름
    public Sprite Icon; // 아이템 아이콘
    public Animator Animator; // 아이템 애니메이션(없을 수도 있음)
    public ItemType Type; // 아이템 타입
}

public abstract class CollectableItemData : BaseItemData
{
    public void Collect()
    {
        OnCollected?.Invoke();
    }
    //public void Collect(Player player)
    //{
    // OnCollected?.Invoke(player);
    //}
    public UnityEvent OnCollected;
    //public UnityEvent<Player> OnCollected;
}

public abstract class EquippableItemData : BaseItemData
{
    public abstract void Equip();
    public abstract void UnEquip();
}
