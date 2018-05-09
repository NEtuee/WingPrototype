using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTester : ObjectBase {

	public override void Initialize()
	{
		SetTransform();
		SetCollider();
	}

	public override void Progress(float deltaTime)
	{
		BulletManager.instance.CollisionCheck(this,Define.BulletTeam.Enemy);
	}

	public override void Release()
	{

	}
}
