using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectBase : MonoBehaviour {

	public bool progressCheck = false;
	public bool destroy = false;

	public SimpleCircleCollider coll;

	public Transform tp;

	protected float maxHp = 0f;
	protected float hp = 0f;

	protected bool isDead = false;
	protected bool isImmortal = false;

	public abstract void Initialize();
	public abstract void Progress();
	public abstract void Release();

	public float GetHp() {return maxHp;}
	public float GetCurrHp() {return hp;}
	public void SetMaxHp(float value) {maxHp = value; hp = maxHp;}

	public void SetDead(bool value) {isDead = value;}
	public void SetImmortal(bool value) 
	{
		isImmortal = value;
	}
	public bool IsDead() {return isDead;}
	public bool IsImmortal() {return isImmortal;}

	public void GetTransform() {tp = transform;}
	public Transform GetTransform(string name)
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

	public void GetCollider() {coll = GetComponent<SimpleCircleCollider>();}
	public void GetChildCollider() {coll = GetComponentInChildren<SimpleCircleCollider>();}
	public SimpleCircleCollider GetColliderInfo() {return coll;}

	public float Increaser(float value,float max,float inc)
	{
		value = (value + inc) >= max ? max : (value + inc);
		return value;
	}

	public float Decreaser(float value,float dec)
	{
		value = (value - dec) <= 0 ? 0 : (value - dec);
		return value;
	}

	public void IncreaseHp(float value) {hp = Increaser(hp,maxHp,value);}
	public void DecreaseHp(float value)
	{
		if(isImmortal)
		{
			return;
		}	
		hp = Decreaser(hp,value);
		if(hp <= 0)
			SetDead(true);
	}

	public virtual bool Collision(ObjectBase obj)
	{
		float dist = GetColliderInfo().radius + obj.GetColliderInfo().radius;

		if(Vector2.Distance(tp.position,obj.tp.position) <= dist)
		{
			return true;
		}

		return false;
	}

	public bool CircleLineIntersect(Vector2 start, Vector2 end)
	{
		if(MathEx.BetweenLineAndCircle(tp.position,GetColliderInfo().radius,start,end) > 0)
		{
			return true;
		}

		return false;
	}

	public bool RectCircleIntersect(Define.RectF rect)
	{
		

		return true;
	}

	

	// public CircleCollider2D coll;

	// public void GetCollider() {coll = GetComponent<CircleCollider2D>();}
	// public void GetChildCollider() {coll = GetComponentInChildren<CircleCollider2D>();}
	// public Bounds GetBounds() {return coll.bounds;}
	// public CircleCollider2D GetColliderInfo() {return coll;}




	// public bool Collision(ObjectBase obj)
	// {
	// 	float dist = GetColliderInfo().radius + obj.GetColliderInfo().radius;

	// 	if(Vector2.Distance(tp.position,obj.tp.position) <= dist)
	// 	{
	// 		return true;
	// 	}

	// 	return false;
	// }
}
