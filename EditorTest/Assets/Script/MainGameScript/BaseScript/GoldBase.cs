using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBase : ObjectBase {

	public int gold = 10;

	public Vector3 targetPos;
	public bool move = false;

	Vector3 origin;
	Vector3 dir;
	float time = 0f;
	float speed = 8f;
	float friction = 5f;

	public void Active(Vector3 pos,Vector3 target,int g = 10,bool m = false)
	{
		tp.position = pos;
		origin = pos;
		targetPos = target;

		gold = g;
		move = m;

		dir = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f));

		time = 0f;
		speed = 8f;

		gameObject.SetActive(true);
	}

	public void Move() {move = true; speed = 3f;}

	public override void Initialize()
	{
		GetTransform();
	}

	public override void Progress()
	{
		if(move)
		{
			time += speed * Time.deltaTime;
			if(time >= 1f)
			{
				time = 1f;
				tp.position = MathEx.EaseOutQuadVector2(origin,targetPos,time);

				GetGold();

				gameObject.SetActive(false);
			}

			tp.position = MathEx.EaseOutQuadVector2(origin,targetPos,time);

		}
		else
		{
			tp.position += dir * speed * Time.deltaTime;
			speed -= friction * Time.deltaTime;
			if(speed <= 0)
				speed = 0;
			
			origin = tp.position;
		}
	}

	public override void Release()
	{

	}

	public void GetGold()
	{
		//do stuff
	}

}
