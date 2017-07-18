using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MeleeWeaponInfo : WeaponInfo {

	public float attackableRange;
	public List<string> comboAnimList = new List<string>();
}
