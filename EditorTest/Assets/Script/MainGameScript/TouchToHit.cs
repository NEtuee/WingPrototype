using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchToHit : BulletBase {

	public float middleAttack = 6.5f;
	public float edgeAttack = 3f;

	public void Active()
	{
		EnemyManager.instance.CollisionCheck(this);
		ItemManager.instance.CollisionCheck(this);
		DeleteCollisionList();

		//progressCheck = true;
	}

	public override void KillWish(Vector3 pos)
	{
		GameObjectManager.instance.bulletManager.ObjectActive
		(pos,0,10,0,false,BulletTeam.Player).
		SetAnimation(GameObjectManager.instance.effectManager.spriteContainer.aniSet[0]).
		SetRadius(3).
		SetPenetrate(true).
		SetLifeTime(0.1f);

		GameObjectManager.instance.effectManager.
		ObjectActive(pos,GameObjectManager.instance.effectManager.spriteContainer.aniSet[3]);

		CameraControll.instance.Shake(1f,2f);
	}

	public override int ColliseionActive(ObjectBase obj)
	{
		for(int i = 0; i < collisionObjects.Count; ++i)
		{
			if(collisionObjects[i] == obj)
			{
				//충돌중
				exitObjects.Remove(obj);
				return 1;
			}
		}

		//최초 충돌
		float dist = Vector2.Distance(tp.position,obj.tp.position);
		float atk = dist >= GetColliderInfo().radius * 0.3f ? (dist >= GetColliderInfo().radius * 0.65f ? edgeAttack : middleAttack) : attack;
		//float atk = Vector2.Distance(tp.position,obj.tp.position) >= GetColliderInfo().radius * 0.5f ? edgeAttack : attack;
		obj.DecreaseHp(atk);
			
		MobileDebugger.instance.AddLine("damage : " + atk);

		//obj.DecreaseHp(attack);
		collisionObjects.Add(obj);

		return 0;
	}

	public override void Initialize()
	{
		GetTransform();
		GetCollider();

		team = BulletTeam.Player;
		progressCheck = false;
		penetrate = true;
		feverAttack = true;
		attack = 10f;
	}

	public override void Progress()
	{
		// if(progressCheck)
		// {
		// 	progressCheck = false;
		// }
	}
}
