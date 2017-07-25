using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : ItemInfo {
	public float healAmount;

	public void DrinkUp ()
	{
		owner.humanInfo.hp = Mathf.Clamp (owner.humanInfo.hp + healAmount, 0, owner.humanInfo.maxHp);
	}

	#region implemented abstract members of InteractableObject
	public override bool IsSatisfied (Actor challenger)
	{
		return true;
	}
	public override void DoInteract (Actor challenger)
	{
		if (null == challenger.equipmentInfo.GetHaveItemByName (itemName)) {
			challenger.equipmentInfo.nowHaveItemInfoList.Add ((ItemInfo)this);
			owner = challenger;
		}
	}
	public override void CancelInteract (Actor challenger)
	{
		
	}
	#endregion

	#region implemented abstract members of DynamicObject

	public override void SaveObject ()
	{
		throw new System.NotImplementedException ();
	}

	public override void LoadObject ()
	{
		throw new System.NotImplementedException ();
	}

	#endregion
	
}
