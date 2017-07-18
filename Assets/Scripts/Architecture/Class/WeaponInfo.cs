using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponInfo {

	public int weaponId;
	public string weaponName;
	public Sprite weaponSprite;
	// Equip 될 때 정해진다.
	public Actor owner;

	public float damage;
	public float attackDelay;

}
