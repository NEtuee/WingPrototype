using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : ObjectBase {

	public float speed = 0f;
	public bool constant = false;

	protected Vector3 origin;

	public PathDatabase.PathInfo pathInfo;
	public EnemyDatabase.EnemyInfo enemyInfo;

	private PathDatabase.LineInfo posOrigin;
	private PathDatabase.LineInfo targetPos;

	private float moveTime = 0f;
	private float stayTime = 0f;
	private int lineCode = 0;

	public override void Initialize()
	{
		GetTransform();
		GetCollider();
	}
	public override void Progress()
	{
		if(stayTime != 0f)
		{
			stayTime -= Time.deltaTime;
			if(stayTime <= 0f)
				stayTime = 0f;
		}
		else
		{
			Move();
		}

		Shot();
	}
	public override void Release(){}

	public void InitValues()
	{
		speed = pathInfo.speed;
		constant = pathInfo.constantSpeed;
		stayTime = pathInfo.startPoint.stayTime;

		SetMaxHp(enemyInfo.hp);
		SetDead(false);

		moveTime = 0f;
		lineCode = 0;

		tp.position = pathInfo.startPoint.point;
		posOrigin = pathInfo.startPoint;
		targetPos = pathInfo.lines[lineCode++];
	}

	public virtual void SetEnemy(Vector3 pos,PathDatabase.PathInfo p, EnemyDatabase.EnemyInfo e) //임시
	{
		pathInfo = p;
		enemyInfo = e;
		
		InitValues();

		gameObject.SetActive(true);
	}

	public virtual void DisableEnemy()
	{
		EnemyManager.instance.count--;

		gameObject.SetActive(false);
	}
	public virtual void CollisionCheck(BulletBase bullet)
	{
		// hp -= bullet.attack;
		// if(hp <= 0f)
		// 	DisableEnemy();
		if(bullet.ColliseionActive(this) == 0)
		{
			GameObjectManager.instance.effectManager.ObjectActive(tp.position,GameObjectManager.instance.effectManager.spriteContainer.aniSet[1]);
			if(!PlayerManager.instance.target.GetFeverEnabled() && !bullet.feverAttack)
				PlayerManager.instance.target.IncreaseFever(1f);
		}
		if(isDead)
		{
			bullet.KillWish(tp.position);
			ItemDrop();
			DisableEnemy();
		}
	}

	public void ItemDrop()
	{
		if(MathEx.GetRandom(100))
		{
			ItemManager.instance.ObjectActive(0,tp.position);
		}
	}

	public virtual void Move()
	{
		if(constant)
			moveTime += speed * Time.deltaTime / Vector3.Distance(posOrigin.point,targetPos.point);
		else
			moveTime += targetPos.speed * Time.deltaTime;

		tp.position = MathEx.GetEaseFormula(posOrigin,targetPos,moveTime,targetPos.type);

		if(moveTime >= 1f)
		{
			if(lineCode >= pathInfo.lines.Count)
			{
				DisableEnemy();
				return;
			}

			tp.position = MathEx.GetEaseFormula(posOrigin,targetPos,1f,pathInfo.lines[lineCode].type);
			moveTime = 0f;
			posOrigin = targetPos;
			targetPos = pathInfo.lines[lineCode++];

			stayTime = posOrigin.stayTime;
		}
	}
	public virtual void Shot()
	{

	}
	
}
