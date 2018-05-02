using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColliderBase : MonoBehaviour {

	public Define.RectF bound = new Define.RectF();
	public Vector2 offset;

	protected Define.RectF collisionInfo = new Define.RectF();

	public void SetCollider(Define.RectF rect, Vector2 offs)
	{
		bound = rect;
		offset = offs;
	}
	public virtual Define.RectF GetBound() {return bound;}
	public abstract Define.RectF GetCollisinoInfo(Vector2 pos);
	public abstract Define.ColliderType GetColliderType();
}
