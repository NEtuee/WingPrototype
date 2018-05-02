using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDamage : BulletBase {

	class CollInfo
	{
		public ObjectBase target;
		public float time = 0f;

		public CollInfo(){}
		public CollInfo(ObjectBase _target, float _time = 0f) {target = _target; time = _time;}
	}

	List<CollInfo> collObjects = new List<CollInfo>();

	public float attackTerm = 0.3f;

	public bool isReady = false;

	private float activeTime = 0f;
	private bool directStop = false;

	public void Active(float atk, bool isFever,float time,bool stop = false, float term = 0.1f)
	{
		if(isReady)
			return;

		isReady = true;

		attack = atk;
		feverAttack = isFever;

		directStop = GameRunningTest.instance.directStop = stop;

		attackTerm = term;
		activeTime = time;

		//gameObject.SetActive(true);
	}

	public override void Initialize()
	{
		GetTransform();
		GetCollider();

		//attack = 1f;
		//gameObject.SetActive(false);
	}

	public override void Progress()
	{
		if(!isReady)
		{
			return;
		}

		activeTime -= Time.deltaTime;
		if(activeTime <= 0f)
		{
			isReady = false;

			if(directStop)
				GameRunningTest.instance.directStop = false;

			DeleteExitObjects();
		}

		CopyList();

		EnemyManager.instance.CollisionCheck(this);
		//ItemManager.instance.CollisionCheck(this);

		DeleteExitObjects();
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
