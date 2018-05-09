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

	protected List<ObjectBase> collisionObjects = new List<ObjectBase>();
	protected List<ObjectBase> exitObjects = new List<ObjectBase>();

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

	public Transform GetTransform(){return tp;}
	public Vector3 GetPosition(){return tp.position;}

	public void IncreaseHP(float value) {MathEx.Increaser(ref hp,maxHp,value);}
	public void DecreaseHP(float value) 
	{
		if(IsImmortal())
			return;
		MathEx.Decreaser(ref hp,maxHp,value);
		if(hp <= 0)
			SetDead(true);
	}

	public virtual void CollisionEnter(ObjectBase obj){}
	public virtual void CollisionStay(ObjectBase obj){}
	public virtual void CollisionExit(ObjectBase obj){}

	public virtual void CopyList()
	{
		exitObjects.Clear();
		for(int i = 0; i < collisionObjects.Count; ++i)
		{
			exitObjects.Add(collisionObjects[i]);
		}
	}

	public virtual void DeleteObjectInCollisionList(ObjectBase obj)
	{
		for(int i = 0; i < collisionObjects.Count; ++i)
		{
			if(collisionObjects[i] == obj)
			{
				collisionObjects.RemoveAt(i);
			}
		}
	}

	public virtual void DeleteCollisionList()
	{
		collisionObjects.Clear();
	}

	public virtual void DeleteExitObjects()
	{
		for(int i = 0; i < exitObjects.Count; ++i)
		{
			//충돌 후
			CollisionExit(exitObjects[i]);
			//exitObjects[i].CollisionExit(this);
			collisionObjects.Remove(exitObjects[i]);
		}
	}

	public virtual int CollisionActive(ObjectBase obj)
	{
		for(int i = 0; i < collisionObjects.Count; ++i)
		{
			if(collisionObjects[i] == obj)
			{
				//충돌중
				CollisionStay(obj);
				exitObjects.Remove(obj);

				return 1;
			}
		}

		//최초 충돌
		CollisionEnter(obj);
		collisionObjects.Add(obj);

		return 0;
	}

	public virtual bool CollisionCheck(ObjectBase obj)
	{
		if(obj.gameObject.activeSelf)
		{
			CopyList();
			obj.CopyList();

			if(Collision(obj))
			{
				CollisionActive(obj);
				//obj.CollisionActive(this);

				return true;
			}
		}

		return false;
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
				return MathEx.IntersectRectCircle(me.GetCollisinoInfo(tp.position),obj.GetCollisinoInfo(obj.transform.position),obj.GetBound());
			}
		}
	}

	public virtual bool Collision(ObjectBase obj)
	{
		ColliderBase other = obj.GetColliderBase();

		return Collision(other);
	}

}
