using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCircleCollider : MonoBehaviour {

	public float radius;

	public void SetRadius(float value) {radius = value;}

	public Define.RectF CircleToBox()
	{
		return new Define.RectF(-radius,radius,radius,-radius);
	}
}
