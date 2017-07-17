using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class WeaponFactory : MonoBehaviour {
	public static WeaponFactory instance;

	public static List<GunInfo> gunInfoList = new List<GunInfo>();
	public static List<MeleeWeaponInfo> meleeWeaponInfoList = new List<MeleeWeaponInfo> ();

	private static void SetGeneralWeaponInfo(WeaponInfo weapon, XmlNode node)
	{
		weapon.weaponId = int.Parse(node.SelectSingleNode ("Id").InnerText);
		weapon.weaponName = node.SelectSingleNode ("Name").InnerText;
		weapon.weaponSprite = Resources.Load ("Images/Bullet/" + weapon.weaponName) as Sprite;
		weapon.damage = int.Parse(node.SelectSingleNode ("Damage").InnerText);
		weapon.attackDelay = float.Parse(node.SelectSingleNode ("AttackDelay").InnerText);
	}

	private static void LoadWeaponsInfo()
	{
		var doc = XmlManager.GetXmlDocument ("WeaponXml");
		if (null == doc) {
			Debug.LogError ("Doc is Null");
			return;
		}
		var gunNodes = doc.SelectNodes ("WeaponSet/GunInfo");

		foreach (XmlNode node in gunNodes) {
			var gunInfo = new WeaponInfo ();
			SetGeneralWeaponInfo (gunInfo, node);

			gunInfo = new GunInfo ();
			((GunInfo)gunInfo).maxAmmo = int.Parse(node.SelectSingleNode ("MaxAmmo").InnerText);
			((GunInfo)gunInfo).ammo = ((GunInfo)gunInfo).maxAmmo;
			((GunInfo)gunInfo).magazine = int.Parse(node.SelectSingleNode ("Magazine").InnerText);
			((GunInfo)gunInfo).usingBullet = Resources.Load ("Prefabs/Bullets/" + node.SelectSingleNode ("UsingBullet").InnerText) as GameObject;

			gunInfoList.Add ((GunInfo)gunInfo);
		}

		var meleeWeaponNodes = doc.SelectNodes ("WeaponSet/MeleeWeaponInfo");

		foreach (XmlNode node in meleeWeaponNodes)
		{
			var meleeWeaponInfo = new WeaponInfo ();
			SetGeneralWeaponInfo (meleeWeaponInfo, node);

			meleeWeaponInfo = new MeleeWeaponInfo ();
			((MeleeWeaponInfo)meleeWeaponInfo).attackableRange = float.Parse (node.SelectSingleNode ("Range").InnerText);
			foreach (XmlNode comboNode in node.SelectNodes("ComboAnimationName"))
			{
				((MeleeWeaponInfo)meleeWeaponInfo).comboAnimList.Add (comboNode.InnerText);
			}

			meleeWeaponInfoList.Add ((MeleeWeaponInfo)meleeWeaponInfo);
		}
	}

	public GunInfo GetGunInfo(int weaponId)
	{
		var savedWeapon = gunInfoList [weaponId];
		var newWeapon = new GunInfo ();
		newWeapon.weaponId = savedWeapon.weaponId;
		newWeapon.weaponName = savedWeapon.weaponName;
		newWeapon.weaponSprite = savedWeapon.weaponSprite;
		newWeapon.damage = savedWeapon.damage;
		newWeapon.attackDelay = savedWeapon.damage;
		((GunInfo)newWeapon).maxAmmo = ((GunInfo)savedWeapon).maxAmmo;
		((GunInfo)newWeapon).ammo = ((GunInfo)savedWeapon).ammo;
		((GunInfo)newWeapon).magazine = ((GunInfo)savedWeapon).magazine;
		((GunInfo)newWeapon).usingBullet = ((GunInfo)savedWeapon).usingBullet;
		return newWeapon;
	}

	public MeleeWeaponInfo GetMeleeWeaponInfo (int weaponId)
	{
		var savedWeapon = meleeWeaponInfoList [weaponId];
		var newWeapon = new MeleeWeaponInfo ();
		newWeapon.weaponId = savedWeapon.weaponId;
		newWeapon.weaponName = savedWeapon.weaponName;
		newWeapon.weaponSprite = savedWeapon.weaponSprite;
		newWeapon.damage = savedWeapon.damage;
		newWeapon.attackDelay = savedWeapon.damage;
		newWeapon.attackableRange = savedWeapon.attackableRange;
		newWeapon.comboAnimList = savedWeapon.comboAnimList;
		return newWeapon;
	}
}
