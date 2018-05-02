using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectBase : MonoBehaviour {

	protected Transform tp;
	protected ColliderBase colliderBase;

	protected float maxHp = 0f;
	protected float hp = 0f;

	public bool progressCheck = true;
	protected bool destroy = false;
	protected bool isDead = false;
	protected bool isImmortal = false;

	public abstract void Initialize();
	public abstract void Progress(float deltaTime);
	public abstract void Release();

	public float GetMaxHP() {return maxHp;}
	public float GetCurrHP() {return hp;}
	public void SetMaxHP(float value) {maxHp = value;}

	public void SetDead(bool value) {isDead = value;}
	public void SetImmortal(bool value) {isImmortal = value;}
	public bool IsDead() {return isDead;}
	public bool IsImmortal() {return isImmortal;}
	public bool IsDestroy() {return destroy;}
	public bool ProgressCheck() {return progressCheck;}

	public void SetCollider() {colliderBase = GetComponent<ColliderBase>();}
	public void SetColliderFromChild() {colliderBase = GetComponentInChildren<ColliderBase>();}
	public ColliderBase GetColliderBase() {return colliderBase;}
	public void SetTransform() {tp = transform;}
	public Transform SetTransformFormChild(string name)
	{
		int index = tp.childCount;
		for(int i = 0; i < index; ++i)
		{
			Transform tra = tp.GetChild(i);
			if(tra.name == name)
			{
				return tra;
			}
		}

		return null;
	}

	public void IncreaseHP(float value) {MathEx.Increaser(ref hp,maxHp,value);}
	public void DecreaseHP(float value) 
	{
		if(IsImmortal())
			return;
		MathEx.Decreaser(ref hp,maxHp,value);
		if(hp <= 0)
			SetDead(true);
	}

	public virtual bool Collision(ColliderBase obj)
	{
		ColliderBase me = GetColliderBase();

		if(me.GetColliderType() == obj.GetColliderType())
		{
			if(me.GetColliderType() == Define.ColliderType.Box)
			{
				return MathEx.IntersectRect(me.GetCollisinoInfo(tp.position),obj.GetCollisinoInfo(obj.transform.position));
			}
			else
			{
				return MathEx.IntersectCircle(me.GetCollisinoInfo(tp.position),obj.GetCollisinoInfo(obj.transform.position));
			}
		}
		else
		{
			if(me.GetColliderType() == Define.ColliderType.Box)
			{
				return MathEx.IntersectRectCircle(obj.GetCollisinoInfo(obj.transform.position),me.GetCollisinoInfo(tp.position),me.GetBound());
			}
			else
			{
				return MathEx.IntersectRectCircle(me.GetCollisinoInfo(obj.transform.position),obj.GetCollisinoInfo(tp.position),obj.GetBound());
			}
		}
	}

	public virtual bool Collision(ObjectBase obj)
	{
		//ColliderBase me = GetColliderBase();
		ColliderBase other = obj.GetColliderBase();

		return Collision(other);
	// 	if(me.GetColliderType() == other.GetColliderType())
	// 	{
	// 		if(me.GetColliderType() == Define.ColliderType.Box)
	// 		{
	// 			return MathEx.IntersectRect(me.GetCollisinoInfo(tp.position),other.GetCollisinoInfo(obj.tp.position));
	// 		}
	// 		else
	// 		{
	// 			return MathEx.IntersectCircle(me.GetCollisinoInfo(tp.position),other.GetCollisinoInfo(obj.tp.position));
	// 		}
	// 	}
	// 	else
	// 	{
	// 		if(me.GetColliderType() == Define.ColliderType.Box)
	// 		{
	// 			return MathEx.IntersectRectCircle(other.GetCollisinoInfo(obj.tp.position),me.GetCollisinoInfo(tp.position),me.GetBound());
	// 		}
	// 		else
	// 		{
	// 			return MathEx.IntersectRectCircle(me.GetCollisinoInfo(obj.tp.position),other.GetCollisinoInfo(tp.position),other.GetBound());
	// 		}
	// 	}
	// }
	}

}
