using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Equippable/Weapon")]
public class WeaponItemData : EquippableItemData
{
	public int AttackPower; // °ø°Ý·Â
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