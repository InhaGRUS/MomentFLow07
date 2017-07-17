using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanInfo {
	public string humanName;

	public bool isTimeImmune = false;

	public float hp;
	public float maxHp;
	public float mp;
	public float maxMp;
	public HumanType humanType;
}
