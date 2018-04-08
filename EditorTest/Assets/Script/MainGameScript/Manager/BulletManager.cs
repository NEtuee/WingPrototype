using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

	public BulletBase origin;
	public int bulletCount = 0;

	public class ObjectLink
	{
		public BulletBase me;
		public ObjectLink back;

		public ObjectLink(){}
		public ObjectLink(BulletBase b)
		{
			me = b;
			back = null;
		}

		public void InitObjects()
		{
			me.Initialize();
		}
	}

	private ObjectLink disableFront;
	private ObjectLink activeFront;
	private ObjectLink activeBack;

	public void SetActiveFirst(ObjectLink link)
	{
		link.back = activeFront;
		activeFront = link;
	}

	public void CreateObjects(int count)
	{
		ObjectLink prev = null;
		for(int i = 0; i < count; ++i)
		{
			BulletBase b = GameObject.Instantiate(origin,Vector3.zero,Quaternion.identity).GetComponent<BulletBase>();

			b.Initialize();
			b.gameObject.name = i.ToString();

			ObjectLink link = new ObjectLink(b);
			if(i == 0)
				disableFront = link;
			else
				prev.back = link;
			
			b.gameObject.SetActive(false);
			prev = link;
		}
	}

	public bool Progress()
	{
		ObjectLink link = activeFront;
		ObjectLink _front = link;

		if(link == null)
			return false;

		// int count = 0;

		while(true)
		{
			// if(++count > 100)
			// {
			// 	Debug.Log("loop err");
			// 	break;
			// }

			if(link.me.progressCheck && link.me.gameObject.activeSelf)
				link.me.Progress();
				
			if(!link.me.gameObject.activeSelf)
			{

				if(link == activeFront)
				{
					ObjectLink save = link;
					activeFront = link.back;
					_front = link.back;
					link = link.back;

					save.back = disableFront;
					disableFront = save;
				}
				else
				{
					ObjectLink save = link;
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
			// else
			// {
			// 	break;
			// }
		}

		return true;
	}

	public void DisableAllObjects()
	{
		ObjectLink link = activeFront;

		if(link == null)
			return;

		while(true)
		{
			link.me.DisableBullet();
			ObjectLink save = link;
			
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

	public BulletBase ObjectActive(ObjectBase sht,Vector2 pos,float speed,float attack,float angle, bool scObj, bool guided = false,BulletBase.BulletTeam team = BulletBase.BulletTeam.Enemy)
	{
		if(disableFront == null)
		{
			return null;
		}

		++bulletCount;

		disableFront.me.SetBullet(sht,pos,speed,attack,angle,guided,scObj, team).SetRadius(0.1f);

		ObjectLink save = disableFront.back;
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

	public BulletBase ObjectActive(ObjectBase sht, EnemyDatabase.EnemyInfo enemyInfo,float attack, EnemyDatabase.BulletInfo info, Vector2 pos, bool scObj, bool guided, BulletBase.BulletTeam team = BulletBase.BulletTeam.Enemy)
	{
		if(disableFront == null)
		{
			return null;
		}

		++bulletCount;

		disableFront.me.SetBullet(sht,pos + enemyInfo.shotPoint[info.shotPoint],info.speed,attack,info.angle,guided,scObj,team).SetRadius(0.1f);

		ObjectLink save = disableFront.back;
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

	public void ObjectDisable(ObjectLink prev,ObjectLink target)
	{
		if(target == activeFront)
		{
			activeFront = target.back;
			target.back = disableFront;
			disableFront = target;
		}
		else
		{
			prev.back = target.back;
			target.back = disableFront;
			disableFront = target;
		}
	}
	public bool ObjectIsNull() {return activeFront == null;}

	public void CollisionCheck(ObjectBase obj,BulletBase.BulletTeam team)
	{
		ObjectLink link = activeFront;

		if(link == null)
			return;

		while(true)
		{
			link.me.CopyList();

			if(link.me.gameObject.activeSelf)
			{
				if(link.me.IsTeam(team))
				{
					if(obj.Collision(link.me))
					{
						//Debug.Log(obj.GetColliderInfo().radius);
						link.me.ColliseionActive(obj);
					}
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
		ObjectLink link = activeFront;

		if(link == null)
			return;

		while(true)
		{
			link.me.CopyList();

			if(!link.me.IsScoreObject() && link.me.gameObject.activeSelf)
			{
				if(link.me.IsEnemyBullet())
					PlayerManager.instance.CollisionCheck(link.me);
				else if(link.me.IsPlayerBullet())
				{
					EnemyManager.instance.CollisionCheck(link.me);
					ItemManager.instance.CollisionCheck(link.me);
				}

				link.me.DeleteExitObjects();
			}

			link = link.back;

			if(link == null)
			{
				break;
			}
		}
	}

}
