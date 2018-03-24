using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : ObjectBase {

	public enum BulletTeam
	{
		Player,
		Enemy,
	}

	public BulletTeam team;
	public float speed;
	public float attack;
	public float angle;
	public bool guided;
	public float lifeTime;

	public SpriteContainer.AnimationSet animationSet;
	public bool anime = false;

	public bool feverAttack = false;
	public bool penetrate = false;
	public bool active = false;

	public List<ObjectBase> collisionObjects = new List<ObjectBase>();
	public List<ObjectBase> exitObjects = new List<ObjectBase>();

	public SpriteRenderer sprRenderer;

	protected Vector3 direction;

	public override void Initialize()
	{
		GetTransform();
		GetCollider();

		sprRenderer = GetComponent<SpriteRenderer>();
	}

	private float aniTime = 0f;
	private int aniCount = 0;
	public override void Progress()
	{
		tp.position += speed * direction * Time.deltaTime;

		if(anime)
		{
			aniTime += Time.deltaTime;
			if(aniTime >= 0.0625)
			{
				sprRenderer.sprite = animationSet.sprites[aniCount++];
				aniTime = 0f;

				if(aniCount > animationSet.aniInfo[0].end)
					aniCount = 0;
			}
		}

		if(lifeTime != -1)
		{
			lifeTime -= Time.deltaTime;
			if(lifeTime <= 0f)
				DisableBullet();
		}

		if(tp.position.x > 30f || tp.position.x < -30f || tp.position.y > 20f || tp.position.y < -20f)
		{
			DisableBullet();
		}
		else if(tp.position.x > 24f || tp.position.x < -24f || tp.position.y > 14f || tp.position.y < -14f)
		{
			if(active)
				DisableBullet();
		}
		else if(tp.position.x < 24f && tp.position.x > -24f && tp.position.y < 14f && tp.position.y > -14f && !active)
		{
			active = true;
		}
	}

	public override void Release()
	{

	}

	public virtual void CopyList()
	{
		exitObjects.Clear();
		for(int i = 0; i < collisionObjects.Count; ++i)
		{
			exitObjects.Add(collisionObjects[i]);
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
			collisionObjects.Remove(exitObjects[i]);
		}
	}

	public virtual void KillWish(Vector3 pos)
	{
		// Debug.Log("nothing");
	}

//	bool check = false;
	public virtual int ColliseionActive(ObjectBase obj)
	{
		if(!penetrate)
		{
			if(!gameObject.activeSelf)
				Debug.Log("asdf");
			obj.DecreaseHp(attack);
			DisableBullet();


			return 0;
		}
		else
		{
//			check = false;

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
			obj.DecreaseHp(attack);
			collisionObjects.Add(obj);

			return 0;
		}
	}

	public BulletBase SetBullet(Vector2 pos, float s,float a,float ang, bool g ,BulletTeam t = BulletTeam.Enemy) //스프라이트 번호 etc
	{
		tp.position = pos;
		speed = s;
		attack = a;
		angle = ang;
		team = t;

		feverAttack = false;
		penetrate = false;
		anime = false;
		guided = g;

		active = false;

		if(guided)
			direction = PlayerManager.instance.GetDirection(tp.position);
		else
			direction = new Vector3(Mathf.Cos((angle * Mathf.Deg2Rad)),Mathf.Sin((angle * Mathf.Deg2Rad)));

		tp.localRotation = Quaternion.Euler(new Vector3(0f,0f,angle));

		gameObject.SetActive(true);

		SetLifeTime();

		return this;
	}

	public BulletBase SetRadius(float value)
	{
		GetColliderInfo().SetRadius(value);

		return this;
	}

	public BulletBase SetAnimation(SpriteContainer.AnimationSet anim)
	{
		animationSet = anim;
		aniTime = 0f;
		aniCount = 0;

		sprRenderer.sprite = animationSet.sprites[aniCount++];
		anime = true;

		return this;
	}

	public BulletBase SetPenetrate(bool value)
	{
		penetrate = value;
		return this;
	}

	public BulletBase SetLifeTime(float value = -1)
	{
		lifeTime = value;

		return this;
	}

	public void DisableBullet()
	{
		if(!gameObject.activeSelf)
			Debug.Log("Check");
		GameObjectManager.instance.bulletManager.bulletCount--;
		gameObject.SetActive(false);
		DeleteExitObjects();
	}

	public bool IsEnemyBullet() {return team == BulletTeam.Enemy;}
	public bool IsPlayerBullet() {return team == BulletTeam.Player;}
	public bool IsTeam(BulletTeam t) {return team == t;}
}
