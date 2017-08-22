using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rect3D : MonoBehaviour {
	public float widht = 1;
	public float height = 1;
	public float depth = 1;

	public Vector3 size = Vector3.one * 2;
	public Vector3 extents = Vector3.one;

	public Vector3 centerPosition;

	public Vector3 maxPoint = Vector3.one; // always centerposition.xyz + extents.xyz
	public Vector3 minPoint = -Vector3.one; // always centerposition.xyz - extents.xyz

	public Color renderColor = new Color (0.5529f, 0.9921f, 0.6313f, 0.0196f);

	public Rect[] objectFace = new Rect[6];

	public Rect3D (Vector3 center, float widht, float height, float depth)
	{
		this.centerPosition = center;
		this.widht = widht;
		this.height = height;
		this.depth = depth;

		this.extents = new Vector3 (widht * 0.5f, height * 0.5f, depth * 0.5f);
		this.maxPoint = new Vector3 (centerPosition.x + extents.x, centerPosition.y + extents.y + centerPosition.z + extents.z);
		this.minPoint = new Vector3 (centerPosition.x - extents.x, centerPosition.y - extents.y, centerPosition.z - extents.z);

		/* Set Faces
		 * Face Direction based on Look Front State
		 */
		//UpFace
		var center2D = new Vector2 (center.x, maxPoint.y);
		var size2D = new Vector2 (widht, depth);
		objectFace [0] = new Rect (center2D, size2D);
		//FrontFace
		center2D = new Vector2 (center.x, center.y);
		size2D = new Vector2 (widht, height);
		objectFace [1] = new Rect (center2D, size2D);
		//DownFace
		center2D = new Vector2 (center.x, minPoint.y);
		size2D = new Vector2 (widht, depth);
		objectFace [2] = new Rect (center2D, size2D);
		//BackFace
		center2D = new Vector2 (center.x, center.y);
		size2D = new Vector2 (widht, height);
		objectFace [3] = new Rect (center2D, size2D);
		//LeftFace
		center2D = new Vector2 (minPoint.x, center.y);
		size2D = new Vector2 (depth, height);
		objectFace [4] = new Rect (center2D, size2D);
		//RightFace
		center2D = new Vector2 (maxPoint.x, center.y);
		size2D = new Vector2 (depth, height);
		objectFace [5] = new Rect (center2D, size2D);
		//End Set Faces

		this.renderColor = new Color (0.5529f, 0.9921f, 0.6313f, 0.0196f);
	}
	public Rect3D (Vector3 center, float widht, float height, float depth, Color color) : this(center, widht, height, depth)
	{
		this.renderColor = color;
	}
	public Rect3D (Vector3 center, Vector3 size) : this (center, size.x, size.y, size.z)
	{
		
	}
	public Rect3D (Vector3 center, Vector3 size, Color color) : this (center, size.x, size.y, size.z, color)
	{
	}

	public bool IsContainPoint (Vector3 point)
	{
		if (point.x > maxPoint.x && point.x < minPoint.x)
		{
			return false;
		}
		if (point.y > maxPoint.y && point.y < minPoint.y)
		{
			return false;
		}
		if (point.z > maxPoint.z && point.z < minPoint.z)
		{
			return false;
		}
		return true;
	}

	public Rect GetFaceRect (FaceName faceName)
	{
		switch (faceName)
		{
		case FaceName.upFace:
			return objectFace [0];
		case FaceName.frontFace:
			return objectFace [1];
		case FaceName.downFace:
			return objectFace [2];
		case FaceName.backFace:
			return objectFace [3];
		case FaceName.leftFace:
			return objectFace [4];
		}
		return objectFace [5];
	}

	public Vector3 GetNearestPoint (Vector3 point)
	{
		if (IsContainPoint (point)) {
			Vector3[] boundVector = new Vector3 [6];
			boundVector [0] = new Vector3 (maxPoint.x, point.y, point.z);
			boundVector [1] = new Vector3 (minPoint.x, point.y, point.z);
			boundVector [2] = new Vector3 (point.x, maxPoint.y, point.z);
			boundVector [3] = new Vector3 (point.x, minPoint.y, point.z);
			boundVector [4] = new Vector3 (point.x, point.y, maxPoint.z);
			boundVector [5] = new Vector3 (point.x, point.y, minPoint.z);

			int index = 0;
			float minDis = Vector3.Distance (point, boundVector [0]);
			for (int i = 1; i < 6; i++) {
				var tmpDis = Vector3.Distance (point, boundVector [i]);
				if (minDis > tmpDis) {
					index = i;
					minDis = tmpDis;
				}
			}
			return boundVector [index];
		}
			
		List<int> checkableList = new List<int> ();
		for (int i = 0; i < 6; i++)
		{
			if (GetFaceRect ((FaceName)i).Contains(point))
			{
				checkableList.Add (i);
			}
		}

		if (checkableList.Count != 0)
		{
			
		}
		return Vector3.one * -10000;
	}
}
