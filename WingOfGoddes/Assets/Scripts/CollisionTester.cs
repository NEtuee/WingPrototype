using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTester : ObjectBase {

	public ColliderBase tester;

	public override void Initialize()
	{
		SetTransform();
		SetCollider();
	}

	public override void Progress(float deltaTime)
	{
		if(Collision(tester))
		{
			Debug.Log("check");
		}
	}

	public override void Release()
	{

	}
}
