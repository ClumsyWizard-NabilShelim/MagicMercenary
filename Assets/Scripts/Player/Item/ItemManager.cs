using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ClumsyWizard;

//public class InventoryItem
//{
//	public int Count;
//	public ItemData ItemData;

//	public InventoryItem(int count, ItemData itemData)
//	{
//		Count = count;
//		ItemData = itemData;
//	}
//}

public class ItemManager : MonoBehaviour
{
	[SerializeField] private WeaponManager weaponManager;
	[SerializeField] private List<ItemData> itemDatas = new List<ItemData>();
	private Dictionary<string, ItemData> items = new Dictionary<string, ItemData>();
	[SerializeField] private List<ItemButton> itemButtons = new List<ItemButton>();
	private int currentButtonIndex;

	[SerializeField] private GameObject placeableRange;
	private Vector2 placeableRangeDefaultSize;

	private ItemData currentItemData;
	private GameObject currentItem;
	private ThrowableItem throwableItem;

	private List<KeyColor> keyColors = new List<KeyColor>();

	private void Start()
	{
		placeableRange.SetActive(false);

		for (int i = 0; i < itemDatas.Count; i++)
		{
			ItemData itemData = itemDatas[i];

			if(itemData.Type == ItemType.Weapon)
				weaponManager.AddWeapon(itemData);

			AddItem(itemData);
		}

		foreach (ItemData item in items.Values)
		{
			itemButtons[currentButtonIndex].gameObject.SetActive(true);
			itemButtons[currentButtonIndex].Initialize(item, this);
			currentButtonIndex++;
		}

		currentButtonIndex = 0;
		placeableRangeDefaultSize = placeableRange.transform.localScale;
	}

	private void Update()
	{
		if (currentItemData != null)
		{
			if (currentItemData.Type == ItemType.Weapon)
			{
				NoiseManager.Instance.ShowNoiseArea(currentItemData.NoiseSize, transform.position);
			}
			else
			{
				NoiseManager.Instance.ShowNoiseArea(currentItemData.NoiseSize);

				if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
				{
					if (throwableItem.Throw())
					{
						ClearCurrentItem();
					}
				}
			}
		}
	}

	public void SelectItem(ItemData data, int index)
	{
		itemButtons[currentButtonIndex].Selected = false;
		currentButtonIndex = index;

		if (currentItem != null)
		{
			if (currentItemData != null)
			{
				if (currentItemData.Type != ItemType.Weapon)
					Destroy(currentItem);
			}
			else
			{
				Destroy(currentItem);
			}
		}

		currentItemData = data;

		if (currentItemData.Type == ItemType.Weapon)
		{
			currentItem = weaponManager.GetWeapon(data.Name);
		}
		else
		{
			weaponManager.DeactivateCurrentWeapon();

			currentItem = Instantiate(data.Item, transform);
			throwableItem = currentItem.GetComponent<ThrowableItem>();
			placeableRange.SetActive(true);
			placeableRange.transform.localScale = MathUtility.ConvertPhysicsScaleToTransformScale(throwableItem.GetPlaceableRange());

			if (currentItemData.Type == ItemType.Gadget)
				currentItem.GetComponent<ElementalGadget>().ItemData = (GadgetItem)data;
		}
	}

	public void DeselectItem()
	{
		NoiseManager.Instance.CloseNoiseArea();

		if (currentItem != null)
		{
			if (currentItemData.Type == ItemType.Weapon)
				currentItem.SetActive(false);
			else
				Destroy(currentItem);
		}

		ClearCurrentItem();
	}

	private void ClearCurrentItem()
	{
		placeableRange.SetActive(false);
		itemButtons[currentButtonIndex].Selected = false;
		currentButtonIndex = 0;

		currentItemData = null;
		currentItem = null;
		throwableItem = null;
	}

	private void AddItem(ItemData data)
	{
		if (items.ContainsKey(data.Name))
			return;
		
		items.Add(data.Name, data);
	}

	public void AddKey(KeyColor color)
	{
		if(!keyColors.Contains(color))
			keyColors.Add(color);
	}

	public bool HasKey(KeyColor color)
	{
		return keyColors.Contains(color);
	}
}