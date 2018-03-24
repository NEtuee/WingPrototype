using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collTest : MonoBehaviour {

	public SimpleCircleCollider col1;
	public SimpleRectCollider col2;

	void Update () {
		if(MathEx.IntersectRectCircle(col1.transform.position,col2.transform.position,col1.radius,col2.rect))
		{
			Debug.Log("check");
		}
	}
}
