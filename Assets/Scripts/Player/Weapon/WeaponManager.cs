using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClumsyWizard;

public class WeaponManager : MonoBehaviour
{
	public struct WeaponMetaData
	{
		public WeaponType Type;
		public GameObject Weapon;
		public WeaponBase WeaponBase;

		public WeaponMetaData(WeaponType type, GameObject weapon)
		{
			Type = type;
			Weapon = weapon;
			WeaponBase = weapon.GetComponent<WeaponBase>();
		}
	}

	[SerializeField] private SpriteRenderer parentRenderer;
	private Dictionary<string, WeaponMetaData> weapons = new Dictionary<string, WeaponMetaData>();
	private bool aim;
	private Animator animator;
	private Camera cam;
	private GameObject currentActiveWeapon;

	private void Start()
	{
		animator = GetComponentInParent<Animator>();
		cam = Camera.main;
	}

	public void AddWeapon(ItemData data)
	{
		if(!weapons.ContainsKey(data.Name))
		{
			GameObject weapon = Instantiate(data.Item, transform);
			weapon.SetActive(false);
			weapons.Add(data.Name, new WeaponMetaData(((WeaponItem)data).WeaponType, weapon));
			weapon.GetComponent<ChildYSort>().ParentRenderer = parentRenderer;
		}
	}

	private void Update()
	{
		if (aim)
			RotateToMouse();
		else
			transform.localRotation = Quaternion.Euler(0, 0, 0);
	}

	private void RotateToMouse()
	{
		Vector2 diff = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		transform.rotation = Quaternion.Euler(0, 0, MathUtility.GetZRotationFromVector(diff));
	}

	public GameObject GetWeapon(string weaponName)
	{
		WeaponMetaData data = weapons[weaponName];

		DeactivateCurrentWeapon();

		currentActiveWeapon = data.Weapon;
		currentActiveWeapon.SetActive(true);

		if (data.Type == WeaponType.Melee)
		{
			data.WeaponBase.OnAttack = () =>
			{
				animator.SetTrigger("MeleeAttack");
			};
		}
		else if(data.Type == WeaponType.Ranged)
		{
			aim = true;
		}

		return currentActiveWeapon;
	}

	public void DeactivateCurrentWeapon()
	{
		aim = false;

		if (currentActiveWeapon != null)
			currentActiveWeapon.SetActive(false);
	}
}