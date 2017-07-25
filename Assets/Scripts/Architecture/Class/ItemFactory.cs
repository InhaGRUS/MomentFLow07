using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ItemFactory {
	public static bool isLoaded;
	public static List<ItemInfo> itemInfoList = new List<ItemInfo>();
	public static List<Potion> potionInfoList = new List<Potion>();

	private static void SetGeneralItemInfo (ItemInfo item, XmlNode node)
	{
		item.itemId = int.Parse (node.SelectSingleNode ("Id").InnerText);
		item.itemName = node.SelectSingleNode ("Name").InnerText;
		item.itemSprite = Resources.Load ("Images/Item/" + item.itemName) as Sprite;
	}

	private static void LoadItemsInfo ()
	{
		var doc = XmlManager.GetXmlDocument ("ItemXml");
		if (null == doc) {
			Debug.LogError ("Doc is Null");
			return;
		}
		var potionNodes = doc.SelectNodes ("ItemSet/Potion");

		foreach (XmlNode node in potionNodes)
		{
			var potion = new Potion ();
			SetGeneralItemInfo (potion, node);

			potion.healAmount = float.Parse (node.SelectSingleNode ("Amount").InnerText);

			potionInfoList.Add (potion);
		}
		isLoaded = true;
	}

	public static Potion GetPotionInfo (int potionId)
	{
		if (!isLoaded)
			LoadItemsInfo ();
		var savedPotion = potionInfoList [potionId];
		var newPotion = new Potion ();
		newPotion.itemId = savedPotion.itemId;
		newPotion.itemName = savedPotion.itemName;
		newPotion.itemSprite = savedPotion.itemSprite;
		newPotion.healAmount = savedPotion.healAmount;

		return newPotion;
	}
}
