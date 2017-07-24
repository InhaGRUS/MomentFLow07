using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideInfo : MonoBehaviour {

	public List<DynamicObject> nearDynamicObjList = new List<DynamicObject>();
	public List<Actor> nearEnemyObjList = new List<Actor>(); 

	public void OnTriggerEnter (Collider col)
	{
		var dynamicObj = DynamicObject.GetDynamicObject (col);
		if (null != dynamicObj)
		{
			if (!nearDynamicObjList.Contains (dynamicObj)) {
				nearDynamicObjList.Add (dynamicObj);
				SortDynamicListByDistance ();
			}
			return;
		}

		var enemyObj = Actor.GetActor (col);
		if (null != enemyObj && enemyObj.humanInfo.humanType == HumanType.Enemy)
		{
			if (!nearEnemyObjList.Contains (enemyObj)) {
				nearEnemyObjList.Add (enemyObj);
				SortEnemyListByDistance ();
			}
			return;
		}
	}

	public void OnTriggerStay (Collider col)
	{
		var dynamicObj = DynamicObject.GetDynamicObject (col);
		if (null != dynamicObj)
		{
			if (!nearDynamicObjList.Contains (dynamicObj)) {
				nearDynamicObjList.Add (dynamicObj);
				SortDynamicListByDistance ();
			}
			return;
		}

		var enemyObj = Actor.GetActor (col);
		if (null != enemyObj  && enemyObj.humanInfo.humanType == HumanType.Enemy)
		{
			if (nearEnemyObjList.Contains (enemyObj)) {
				nearEnemyObjList.Remove (enemyObj);
				SortEnemyListByDistance ();
			}
			return;
		}
	}

	public void OnTriggerExit (Collider col)
	{
		var dynamicObj = DynamicObject.GetDynamicObject (col);
		if (null != dynamicObj)
		{
			if (nearDynamicObjList.Contains (dynamicObj)) {
				nearDynamicObjList.Remove (dynamicObj);
				SortDynamicListByDistance ();
			}
			return;
		}

		var enemyObj = Actor.GetActor (col);
		if (null != enemyObj && enemyObj.humanInfo.humanType == HumanType.Enemy)
		{
			if (!nearEnemyObjList.Contains (enemyObj)) {
				nearEnemyObjList.Add (enemyObj);
				SortEnemyListByDistance ();
			}
			return;
		}
	}

	void SortDynamicListByDistance ()
	{
		nearDynamicObjList.Sort (delegate (DynamicObject x, DynamicObject y) {
			if (null == x)
			{
				nearDynamicObjList.Remove(x);
				return 0;
			}
			if (null == y)
			{
				nearDynamicObjList.Remove (y);
				return 0;
			}
			var dis01 = Vector3.Distance (transform.position, x.transform.position);
			var dis02 = Vector3.Distance (transform.position, y.transform.position);
			if (dis01 > dis02) {
				return 1;
			}
			else
				if (dis01 < dis02) {
					return -1;
				}
			return 0;
		});
	}

	void SortEnemyListByDistance ()
	{
		nearEnemyObjList.Sort (delegate (Actor x, Actor y) {
			var dis01 = Vector3.Distance (transform.position, x.transform.position);
			var dis02 = Vector3.Distance (transform.position, y.transform.position);
			if (dis01 > dis02) {
				return 1;
			}
			else
				if (dis01 < dis02) {
					return -1;
				}
			return 0;
		});
	}

	public DynamicObject FindNearestDynamicObject ()
	{
		if (nearDynamicObjList.Count != 0)
		{
			return nearDynamicObjList [0];
		}
		return null;
	}

	public DynamicObject FindNearestEnemyTypeObject ()
	{
		return null;
	}
}
