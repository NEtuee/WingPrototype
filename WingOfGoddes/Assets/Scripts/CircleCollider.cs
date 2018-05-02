using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollider : ColliderBase {

	public float radius = 1f;

	public override Define.RectF GetBound() {float save = radius * .5f; return new Define.RectF(save,save,save,save);}
	public override Define.RectF GetCollisinoInfo(Vector2 pos) 
	{
		collisionInfo.min = pos;
		collisionInfo.max.x = radius;

		return collisionInfo;
	}
	public override Define.ColliderType GetColliderType() {return Define.ColliderType.Circle;}
}
