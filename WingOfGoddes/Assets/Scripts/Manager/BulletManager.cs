using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : LinkProgressManagerBase<BulletManager> {

	public GameObject origin;
	public int bulletCount = 0;

	private ObjectLink<BulletBase> disableFront;
	private ObjectLink<BulletBase> activeFront;
	private ObjectLink<BulletBase> activeBack;

	public override void Initialize()
	{
		instance = this;

		CreateObjects(100);
	}

	public override void Progress(float deltaTime)
	{
		ObjectLink<BulletBase> link = activeFront;
		ObjectLink<BulletBase> _front = link;

		if(link == null)
			return;

		while(true)
		{
			if(link.me.progressCheck && link.me.gameObject.activeSelf)
				link.me.Progress(deltaTime);
				
			if(!link.me.gameObject.activeSelf)
			{

				if(link == activeFront)
				{
					ObjectLink<BulletBase> save = link;
					activeFront = link.back;
					_front = link.back;
					link = link.back;

					save.back = disableFront;
					disableFront = save;
				}
				else
				{
					ObjectLink<BulletBase> save = link;
					_front.back = link.back;

					if(_front.back == null)
						activeBack = _front;
					link = link.back;

					save.back = disableFront;
					disableFront = save;
				}

				if(link == null)
					break;
				else
					continue;

				
			}
			else if(link != null)
			{
				if(link.back == null)
				{
					break;
				}
				_front = link;
				link = link.back;
			}
		}

		return;
	}

	public override void Release()
	{

	}

	public BulletBase ObjectActive(
		ObjectBase shot,Vector2 pos, float sp,float atk,float ang,bool guid,
		bool scObj = false, Define.BulletTeam t = Define.BulletTeam.Enemy)
	{
		if(disableFront == null)
		{
			return null;
		}

		++bulletCount;

		disableFront.me.SetBullet(shot,pos,sp,atk,ang,guid,scObj,t);

		ObjectLink<BulletBase> save = disableFront.back;
		disableFront.back = null;

		if(activeFront == null)
		{
			activeFront = disableFront;
			activeBack = activeFront;
		}
		else
		{
			activeBack.back = disableFront;
			activeBack = disableFront;
		}

		disableFront = save;

		return activeBack.me;
	}

	public void DisableAllObjects()
	{
		ObjectLink<BulletBase> link = activeFront;

		if(link == null)
			return;

		while(true)
		{
			//if(!link.me.IsScoreObject())
				link.me.DisableBullet();
			ObjectLink<BulletBase> save = link;
			
			link = link.back;

			save.back = disableFront;
			disableFront = save;

			if(link == null)
			{
				activeFront = null;
				activeBack = null;
				break;
			}
		}
	}

	public void SetActiveFirst(ObjectLink<BulletBase> link)
	{
		link.back = activeFront;
		activeFront = link;
	}

	public void CreateObjects(int count)
	{
		instance = this;

		if(instance == null)
			Debug.Log("check");

		ObjectLink<BulletBase> prev = null;
		for(int i = 0; i < count; ++i)
		{
			BulletBase b = GameObject.Instantiate(origin,Vector3.zero,Quaternion.identity).GetComponent<BulletBase>();

			b.Initialize();
			b.gameObject.name = i.ToString();

			ObjectLink<BulletBase> link = new ObjectLink<BulletBase>(b);
			if(i == 0)
				disableFront = link;
			else
				prev.back = link;
			
			b.gameObject.SetActive(false);
			prev = link;
		}
	}

	public void DeleteAllExitObjects()
	{
		ObjectLink<BulletBase> link = activeFront;

		if(link == null)
		{
			return;
		}
		while(true)
		{
			link.me.DeleteExitObjects();	

//			ObjectLink<BulletBase> save = link;
			
			link = link.back;

			if(link == null)
			{
				break;
			}
		}

	}

	public void CollisionCheck(ObjectBase obj,Define.BulletTeam targetTeam)
	{
		ObjectLink<BulletBase> link = activeFront;

		if(link == null)
			return;

		while(true)
		{
			link.me.CopyList();

			if(link.me.gameObject.activeSelf)
			{
				if(link.me.IsTeam(targetTeam))
				{
					link.me.CollisionCheck(obj);
					// if(obj.Collision(link.me))
					// {
					// 	//Debug.Log(obj.GetColliderInfo().radius);
					// 	link.me.ColliseionActive(obj);
					// }
				}
			}

			//link.me.DeleteExitObjects();

			link = link.back;

			if(link == null)
			{
				break;
			}
		}
	}

	public void CollisionCheck()
	{
		ObjectLink<BulletBase> link = activeFront;

		if(link == null)
			return;

		while(true)
		{
			link.me.CopyList();

// 			if(!link.me.IsScoreObject() && link.me.gameObject.activeSelf)
// 			{
// 				if(link.me.IsEnemyBullet())
// 					PlayerManager.instance.CollisionCheck(link.me);
// 				else if(link.me.IsPlayerBullet())
// 				{
// //					EnemyManager.instance.CollisionCheck(link.me);
// 					//ItemManager.instance.CollisionCheck(link.me);
// 				}
// 			}

			link = link.back;

			if(link == null)
			{
				break;
			}
		}
	}

}
