using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EquipmentInfo : MonoBehaviour {

	public Actor owner;

	public KeyCode[] gunKey;
	public KeyCode meleeWeaponKey;

	public List<int> defaultHaveGunId = new List<int> ();
	public List<int> defaultHaveMeleeWeaponId = new List<int> ();

	public List<GunInfo> nowHaveGunInfo = new List<GunInfo> ();
	public List<MeleeWeaponInfo> nowHaveMeleeWeaponInfo = new List<MeleeWeaponInfo> ();
	public int meleeWeaponIndex = 0;
	public List<ItemInfo> nowHaveItemInfoList = new List<ItemInfo>();

	public KeyCode nowEquipWeaponKey;
	public WeaponInfo nowEquipWeaponInfo;
	public EquipWeaponType nowEquipWeaponType;

	// Use this for initialization
	void Awake () {
		owner = Actor.GetActor<EquipmentInfo> (this);	
		EquipDefaultWeapon ();
	}

	public void Update()
	{
		SwapWeapon ();
	}

	void EquipDefaultWeapon ()
	{
		for (int i = 0; i < defaultHaveMeleeWeaponId.Count; i++) {
			nowHaveMeleeWeaponInfo.Add (WeaponFactory.GetMeleeWeaponInfo (defaultHaveMeleeWeaponId [i]));
			nowHaveMeleeWeaponInfo [nowHaveMeleeWeaponInfo.Count - 1].owner = owner;
		}
		for (int i = 0; i < defaultHaveGunId.Count; i++) {
			nowHaveGunInfo.Add (WeaponFactory.GetGunInfo (defaultHaveGunId [i]));
			nowHaveGunInfo [nowHaveGunInfo.Count - 1].owner = owner;
		}

		if (nowHaveGunInfo.Count != 0) {
			SwapWeapon<GunInfo> (0);
		} else if (nowHaveMeleeWeaponInfo.Count != 0) {
			SwapWeapon<MeleeWeaponInfo> (0);
		}
	}

	public ItemInfo GetHaveItemByName (string itemName)
	{
		ItemInfo item = null;
		nowHaveItemInfoList.ForEach (delegate(ItemInfo obj) {
			if (obj.itemName == itemName)	
			{
				item = obj;
			}
		});
		return item;
	}

	//public string TryToPickUpItem ()
	public void SwapWeapon () 
	{
		if (Input.GetKeyDown (meleeWeaponKey))
		{
			try
			{
				if (nowEquipWeaponKey == meleeWeaponKey)
				{
					if (meleeWeaponIndex + 1 < nowHaveMeleeWeaponInfo.Count)
					{
						meleeWeaponIndex += 1;
					}
					else
					{
						meleeWeaponIndex = 0;
					}
				}
				nowEquipWeaponInfo = (WeaponInfo)nowHaveMeleeWeaponInfo [meleeWeaponIndex];
				nowEquipWeaponKey = meleeWeaponKey;
				nowEquipWeaponType = EquipWeaponType.MeleeWeapon;
			}
			catch (Exception e) {

			}
			return;
		}
		for (int i = 0; i < gunKey.Length; i++)
		{
			if (Input.GetKeyDown (gunKey[i]))
			{
				try{
					nowEquipWeaponInfo = (WeaponInfo)nowHaveGunInfo [i];
					nowEquipWeaponKey = gunKey [i];
					nowEquipWeaponType = EquipWeaponType.Gun;
				}
				catch (Exception e) {

				}
				return;
			}
		}
	}

	public void SwapWeapon <T> (int index) where T : WeaponInfo
	{
		if (typeof(T) == typeof(GunInfo)) {
			try
			{
				nowEquipWeaponInfo = (WeaponInfo)nowHaveGunInfo [index];
				nowEquipWeaponKey = gunKey [index];
				nowEquipWeaponType = EquipWeaponType.Gun;
			}
			catch (Exception e) {

			}
		}
		else if (typeof(T) == typeof(MeleeWeaponInfo)) {
			try
			{
				nowEquipWeaponInfo = (WeaponInfo)nowHaveMeleeWeaponInfo [index];
				nowEquipWeaponKey = meleeWeaponKey;
				nowEquipWeaponType = EquipWeaponType.MeleeWeapon;
			}
			catch (Exception e) {

			}
		}
		else {
			Debug.LogError ("Can't Swap Weapon To : " + typeof(T).ToString());
		}
	}
}
