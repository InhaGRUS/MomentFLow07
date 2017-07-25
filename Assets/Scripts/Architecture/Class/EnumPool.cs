using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DynamicObjectType
{
	Actor,
	InteractableObject,
	Bullet
}

public enum HumanType
{
	Player,
	Enemy,
	NPC
}

public enum ItemType
{
	Potion,
	Ammo,
	Key,
	SkillScroll,
	PuzzlePiece
}
	

public enum AnimationStateInfoEnum
{
	Enter,
	Progress,
	Exit
}

public enum EquipWeaponType
{
	Gun,
	MeleeWeapon
}

public enum AimState
{
	Normal,
	Bounce
}

public class EnumPool {
}
