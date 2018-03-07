using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelAttack : BulletBase {

	class CollInfo
	{
		public ObjectBase target;
		public float time = 0f;

		public CollInfo(){}
		public CollInfo(ObjectBase _target, float _time = 0f) {target = _target; time = _time;}
	}

	List<CollInfo> collObjects = new List<CollInfo>();

	public float attackTerm = 0.3f;
	public float stayTime = 0f;

	public float moveTime = 0f;
	public Vector3 targetPos;
	public Vector3 startPos;

	public bool isReady = true;
	public bool returning = false;

	public void Active(Vector3 pos)
	{
		if(!isReady)
			return;

		isReady = false;

		tp.position = PlayerManager.instance.target.tp.position;
		startPos = tp.position;
		targetPos = pos;
		targetPos.x = 16f;

		moveTime = 0f;

		returning = false;

		//gameObject.SetActive(true);
	}

	public override void Initialize()
	{
		GetTransform();
		GetCollider();

		attack = 1f;

		feverAttack = true;

		//gameObject.SetActive(false);
	}

	public override void Progress()
	{
		if(isReady)
		{
			return;
		}

		CopyList();

		EnemyManager.instance.CollisionCheck(this);
		ItemManager.instance.CollisionCheck(this);

		DeleteExitObjects();

		moveTime += Time.deltaTime;

		if(returning)
		{
			targetPos = PlayerManager.instance.target.tp.position;

			tp.position = MathEx.EaseInCubicVector2(startPos,targetPos,moveTime);

			if(moveTime >= 1f)
			{
				isReady = true;
				//gameObject.SetActive(false);
			}
		}
		else
		{
			tp.position = MathEx.EaseOutCubicVector2(startPos,targetPos,moveTime);
			if(moveTime >= 1f)
			{
				tp.position = targetPos;
				if(moveTime - 1f >= stayTime)
				{
					moveTime = 0f;
					startPos = targetPos;
					returning = true;
				}
			}
		}
	}

	public void Attack(ObjectBase obj)
	{
		obj.DecreaseHp(attack);
	}

	public override void CopyList()
	{
		exitObjects.Clear();
		for(int i = 0; i < collObjects.Count; ++i)
		{
			exitObjects.Add(collObjects[i].target);
		}
	}

	public override void DeleteCollisionList()
	{
		collObjects.Clear();
	}

	public override void DeleteExitObjects()
	{
		for(int i = 0; i < exitObjects.Count; ++i)
		{
			//충돌 후
			for(int j = 0; j < collObjects.Count;)
			{
				if(collObjects[j].target == exitObjects[i])
				{
					collObjects.RemoveAt(j);
				}
				else
					++j;
			}
			//collisionObjects.Remove(exitObjects[i]);
		}
	}

	public override int ColliseionActive(ObjectBase obj)
	{
		for(int i = 0; i < collObjects.Count; ++i)
		{
			if(collObjects[i].target == obj)
			{
				//충돌중
				collObjects[i].time += Time.deltaTime;
				if(collObjects[i].time >= attackTerm)
				{
					collObjects[i].time = 0f;
					Attack(obj);
					exitObjects.Remove(obj);

					return 0;
				}

				exitObjects.Remove(obj);
				return 1;
			}
		}

		//최초 충돌
		Attack(obj);
		collObjects.Add(new CollInfo(obj));

		return 0;
	}
}
