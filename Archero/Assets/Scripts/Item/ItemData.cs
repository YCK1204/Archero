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

[CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Collectable/Potion")]
public class PotionItemData : CollectableItemData
{
    public int HealAmount;
}

[CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Collectable/EXP")]
public class EXPItemData : CollectableItemData
{
    public int EXPAmount; // 회복량
}

public abstract class EquippableItemData : BaseItemData
{
    public abstract void Equip();
    public abstract void UnEquip();
}


[CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Equippable/Weapon")]
public class WeaponItemData : EquippableItemData
{
    public int AttackPower; // 공격력
    public override void Equip()
    {
    }
    public override void UnEquip()
    {
    }
    //public override void Equip(Player player)
    //{
    //}
    //public override void UnEquip(Player player)
    //{
    //}
}
[CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Equippable/Armor")]
public class ArmorItemData : EquippableItemData
{
    public int DefensePower; // 공격력
    public override void Equip()
    {
    }
    public override void UnEquip()
    {
    }
    //public override void Equip(Player player)
    //{
    //}
    //public override void UnEquip(Player player)
    //{
    //}
}