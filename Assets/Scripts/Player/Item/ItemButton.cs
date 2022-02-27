using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour, IPointerDownHandler
{
	private ItemManager itemManager;
	private PlayerStats playerStats;

	private ItemData itemData;

	public bool Selected { private get; set; }

	[Header("UI")]
	[SerializeField] private Image iconImage;

	private void Start()
	{
		playerStats = FindObjectOfType<PlayerStats>();
	}

	public void Initialize(ItemData itemData, ItemManager itemManager)
	{
		this.itemData = itemData;

		iconImage.sprite = itemData.Icon;

		this.itemManager = itemManager;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		AudioManager.Instance.PlayAudio("ButtonClick");

		if (itemData.Type == ItemType.Gadget)
		{
			if (!playerStats.HasEnoughMana(((GadgetItem)itemData).manaCost))
				return;
		}

		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (Selected)
				itemManager.DeselectItem();
			else
				itemManager.SelectItem(itemData, transform.GetSiblingIndex());

			Selected = !Selected;
		}
	}
}