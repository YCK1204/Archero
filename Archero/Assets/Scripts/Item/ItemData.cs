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
    public string Name; // ������ �̸�
    public Sprite Icon; // ������ ������
    public Animator Animator; // ������ �ִϸ��̼�(���� ���� ����)
    public ItemType Type; // ������ Ÿ��
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
    public int EXPAmount; // ȸ����
}

public abstract class EquippableItemData : BaseItemData
{
    public abstract void Equip();
    public abstract void UnEquip();
}


[CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Equippable/Weapon")]
public class WeaponItemData : EquippableItemData
{
    public int AttackPower; // ���ݷ�
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
    public int DefensePower; // ���ݷ�
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