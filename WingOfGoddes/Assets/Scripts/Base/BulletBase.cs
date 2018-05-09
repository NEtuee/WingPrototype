using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : ObjectBase {

	public Define.BulletTeam team;

	public float speed;
	public float attack;
	public float angle;
	public bool guided;
	public float lifeTime;

	public KillWishBase[] killWish;

	public SpriteRenderer sprRenderer;

	public ObjectBase shooter = null;

	protected bool feverAttack = false;
	protected bool penetrate = false;
	protected bool active = false;

	protected bool isScoreObj = false;

	protected bool scoreObj = false;
	protected bool scoreStay = false;
	protected bool rotationLock = false;
	
	protected float scoreTime = 0f;
	protected float angleAccel = 0f;
	protected float accel = 0f;
	protected float prevAngle = 0f;

	protected Vector3 direction;
	protected Vector3 scoreStartPos;

	public override void Initialize()
	{
		SetTransform();
		SetCollider();
	}

	public override void Progress(float deltaTime)
	{
		if(shooter != null)
			if(shooter.IsDead())
				SetScoreObject();

		if(guided && team == Define.BulletTeam.Player)
		{
			if(PlayerManager.instance.target.GetNearEnemy() != null)
			{
				direction = (PlayerManager.instance.target.GetNearEnemy().GetPosition() - tp.position).normalized;

				if(!rotationLock)
				{
					angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
					tp.localRotation = Quaternion.Euler(new Vector3(0f,0f,angle));
				}
			}
		}

		if(team == Define.BulletTeam.Enemy)
		{
			angle += angleAccel * Time.deltaTime;
			speed += accel * Time.deltaTime;

			if(prevAngle != angle)
			{
				prevAngle = angle;
				direction = new Vector3(Mathf.Cos((angle * Mathf.Deg2Rad)),Mathf.Sin((angle * Mathf.Deg2Rad)));	

				if(!rotationLock)
				{
					tp.localRotation = Quaternion.Euler(new Vector3(0f,0f,angle));
				}
			}
		}

		tp.position += speed * direction * deltaTime;

		if(lifeTime != -1)
		{
			lifeTime -= Time.deltaTime;
			if(lifeTime <= 0f)
				DisableBullet();
		}

		if(tp.position.x > 20f || tp.position.x < -20f || tp.position.y > 30f || tp.position.y < -30f)
		{
			DisableBullet();
		}
		else if(tp.position.x > 14f || tp.position.x < -14f || tp.position.y > 24f || tp.position.y < -24f)
		{
			if(active)
				DisableBullet();
		}
		else if(tp.position.x < 14f && tp.position.x > -14f && tp.position.y < 24f && tp.position.y > -24f && !active)
		{
			active = true;
		}
		//tp.position += Vector3.left * deltaTime;
	}

	public override void Release(){}

	public bool IsTeam(Define.BulletTeam t) {return team == t;}

	public void SetScoreObject()
	{
		scoreObj = true;
		// StageClearInfo.instance.obtScore += 100;
		// scoreStartPos = tp.position;

		// tp.rotation = Quaternion.identity;

		// SetAnimation(DatabaseContainer.instance.spriteDatabase.aniSet[2]);
	}


	public BulletBase SetBullet(
		ObjectBase shot,Vector2 pos, float sp,float atk,float ang,bool guid,
		bool scObj, Define.BulletTeam t)
	{
	 	shooter = shot;

	 	tp.position = pos;
	 	speed = sp;
	 	attack = atk;
	 	angle = ang;
		prevAngle = angle;
	 	team = t;

	 	feverAttack = false;
	 	penetrate = false;

	 	guided = guid;

	 	active = false;

	 	scoreObj = false;
	 	scoreStay = true;
	 	isScoreObj = scObj;
	 	scoreTime = 0f;

	 	rotationLock = false;

		angleAccel = 0f;
	 	accel = 0f;

		killWish = null;

		if(guided)
		{
			if(team == Define.BulletTeam.Enemy)
				direction = PlayerManager.instance.GetDirection(tp.position);
		 	else
		 	{
				float rad = angle * Mathf.Deg2Rad;
		 		direction = new Vector3(Mathf.Cos((rad)),Mathf.Sin((rad)));
		 	}
		}
		else
			direction = new Vector3(Mathf.Cos((angle * Mathf.Deg2Rad)),Mathf.Sin((angle * Mathf.Deg2Rad)));

	 	tp.localRotation = Quaternion.Euler(new Vector3(0f,0f,angle));

	 	gameObject.SetActive(true);

	 	SetLifeTime();

		return this;
	}

	public void killWishInitialize()
	{
		for(int i = 0; i < killWish.Length; ++i)
		{
			killWish[i].Initialize();
		}
	}

	public void KillWishProgress()
	{
		for(int i = 0; i < killWish.Length; ++i)
		{
			killWish[i].Progress();
		}
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
			Debug.Log("already disable");
		//BulletManager.instance.bulletCount--;
		DeleteCollisionList();
		gameObject.SetActive(false);
	}

	public override void CollisionEnter(ObjectBase obj)
	{
		obj.DecreaseHP(attack);

		if(obj.IsDead())
		{
			if(killWish != null)
			{
				KillWishProgress();
			}
		}
		if(!penetrate)
		{
			DisableBullet();
		}
	}
}