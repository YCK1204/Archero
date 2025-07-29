using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Equippable/Armor")]
public class ArmorItemData : EquippableItemData
{
    public int DefensePower; // °ø°Ý·Â
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