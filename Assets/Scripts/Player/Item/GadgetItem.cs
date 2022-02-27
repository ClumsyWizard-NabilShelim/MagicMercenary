using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gadget Item", menuName = "Item/Gadget")]
public class GadgetItem : ItemData
{
	public int manaCost;
	public float effectRange;
}