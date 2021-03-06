﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ObjectBase {

	public Vector3 dir;
	public float speed;

	public ItemEffectBase effectBase;

	public bool collisionReady = false;
	//public ItemManager.ItemEffect effectBase;

	private float time = 0.5f;
	private float aliveTime = 5f;

	private int collisionPoint = -1; //left 0 up 1 right 2 down 3

	public override void Initialize()
	{
		GetTransform();
		GetCollider();

		ValueInit();
	}

	public override void Progress()
	{
        Movement();

        if (!collisionReady)
        {
            if (time <= 0f)
                collisionReady = true;
            else
                time -= Time.deltaTime;
        }
        else
        {
            aliveTime -= Time.deltaTime;
            if (aliveTime <= 0f)
                Disable();
        }
	}

	public override void Release()
	{

	}

	public void Active(ItemEffectBase effect,Vector3 pos)
	{
		effectBase = effect;
		tp.position = pos;
		gameObject.SetActive(true);
		ValueInit();
	}

	public virtual void CollisionCheck(BulletBase bullet)
	{
		if(bullet.ColliseionActive(this) == 0)
		{
			GameObjectManager.instance.effectManager.ObjectActive(tp.position,GameObjectManager.instance.effectManager.spriteContainer.aniSet[1]);
		}
		if(isDead)
		{
			effectBase.Effect();
			Disable();
		}
	}

	public void Disable()
	{
		gameObject.SetActive(false);
	}

	//Vector3[] dirInfo = {new Vector3(-1,1),new Vector3(1,1),new Vector3(-1,-1),new Vector3(1,-1)};
	public virtual void Movement()
	{
		tp.position += dir * speed * Time.deltaTime;

		EdgeCheck();
	}

	public virtual void EdgeCheck() //화면 끝 체크
	{
		if(tp.position.x > 13f && collisionPoint != 2)
		{
			dir.x *= -1f;

			collisionPoint = 2;
		}
		if(tp.position.x < -13f && collisionPoint != 0)
		{
            dir.x *= -1f;

			collisionPoint = 0;
		}
		if(tp.position.y > 23f && collisionPoint != 1)
		{
			dir.y *= -1f;

			collisionPoint = 1;
		}
		if(tp.position.y < -23f && collisionPoint != 3)
		{
			dir.y *= -1f;

            collisionPoint = 3;
		}
	}

	public virtual void ValueInit()
	{
		SetMaxHp(3);
		SetDead(false);

		time = 1f;
		time = 1f;
		aliveTime = 3f;

		collisionReady = false;
        collisionPoint = -1;

        dir = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f)).normalized;
        //dir = new Vector3(0f, -1f);

		speed = Random.Range(7f,10f);
	}
}
