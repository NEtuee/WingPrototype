using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPillar : BulletBase {

	public SimpleRectCollider rectCollider;

	public int state = 0;
	public float time = 0f;
	public float stayTime = 1f;

	public void Active(Vector3 pos,float stay)
	{
		tp.position = pos;

		state = 0;
		time = 0f;

		stayTime = stay;

		AnimationInit();

		gameObject.SetActive(true);
	}

	public override void Initialize()
	{
		sprRenderer = GetComponent<SpriteRenderer>();
		rectCollider = GetComponent<SimpleRectCollider>();

		GetTransform();

		SetAnimation(GameObjectManager.instance.effectManager.spriteContainer.aniSet[6]);
		SetPenetrate(true);
		SetAttack(0.1f);
	}

	public override void Progress()
	{
		bool b = Animation(state);

		time += Time.deltaTime;

		if(time >= stayTime)
		{
			state = 1;
		}

		if(state == 0)
		{
			PlayerManager.instance.CollisionCheck(this);
			//EnemyManager.instance.CollisionCheck(this);
			DeleteCollisionList();
		}

		if(b)
		{
			gameObject.SetActive(false);
		}
	}

	public override bool Collision(ObjectBase obj)
	{
		if(MathEx.IntersectRectCircle(obj.tp.position,tp.position,obj.GetColliderInfo().radius,rectCollider.rect))
			return true;

		return false;
	}
	
}
