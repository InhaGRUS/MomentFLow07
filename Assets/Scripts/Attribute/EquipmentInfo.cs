using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInfo : MonoBehaviour {

	public Actor owner;

	public List<WeaponInfo> haveWeapoinInfo = new List<WeaponInfo>();
	//public List<ItemInfo> haveItemInfoList = new List<ItemInfo>();

	public int nowEquipWeaponIndex = 0;
	public WeaponInfo nowEquipWeaponInfo;

	// Use this for initialization
	void Start () {
		owner = Actor.GetActor<EquipmentInfo> (this);	
	}

	//public string TryToPickUpItem ()
	public void SwapWeapon (int toIndex) 
	{
		if (toIndex >= 0 && toIndex < haveWeapoinInfo.Count)
		{
			nowEquipWeaponInfo = haveWeapoinInfo [toIndex];
			nowEquipWeaponIndex = toIndex;
		}
	}
}
