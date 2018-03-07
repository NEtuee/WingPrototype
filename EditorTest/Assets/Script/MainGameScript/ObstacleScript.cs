using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : ObjectBase {

	public override void Initialize()
	{
		GetTransform();
		GetCollider();
	}

	public override void Progress()
	{
		GameObjectManager.instance.bulletManager.CollisionCheck(this,BulletBase.BulletTeam.Enemy);
	}

	public override void Release()
	{

	}

}
