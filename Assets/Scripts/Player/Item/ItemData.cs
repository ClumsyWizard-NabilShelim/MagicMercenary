using System.Collections;
using UnityEngine;


public enum ItemType
{
	None,
	Weapon,
	Gadget,
	InfiniteResource
}

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Base Item")]
public class ItemData : ScriptableObject
{
	public string Name;
	public ItemType Type;
	public NoiseAreaSize NoiseSize;
	public Sprite Icon;
	public GameObject Item;
}