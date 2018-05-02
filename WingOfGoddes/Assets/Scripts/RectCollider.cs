using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectCollider : ColliderBase {

	public override Define.RectF GetCollisinoInfo(Vector2 pos) 
	{
		collisionInfo.min = bound.min + pos; 
		collisionInfo.max = bound.max + pos;
		return collisionInfo;
	}
	public override Define.ColliderType GetColliderType() {return Define.ColliderType.Box;}
}
