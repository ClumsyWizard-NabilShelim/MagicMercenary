using System;
using UnityEngine;

public enum WeaponType
{
	None,
	Melee,
	Ranged
}

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Item/Weapon")]
public class WeaponItem : ItemData
{
	public WeaponType WeaponType;
}