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

public abstract class EquippableItemData : BaseItemData
{
    public abstract void Equip();
    public abstract void UnEquip();
}
