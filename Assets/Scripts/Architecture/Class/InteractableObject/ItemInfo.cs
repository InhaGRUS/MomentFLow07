using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemInfo : InteractableObject {
	public ItemType objectType;
	public int itemId;
	public Sprite itemSprite;
	public string itemName;
	public Actor owner;
	public int haveAmount;
}
