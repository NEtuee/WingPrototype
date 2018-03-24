using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMove : ObjectBase {

	public Transform[] points;
	public float time;
	public float shotTime;

	public int len;

	private float shot = 0;

	public override void Initialize()
	{
		len = points.Length;
		gameObject.SetActive(false);
	}

	public override void Progress()
	{
		time -= Time.deltaTime;
		shot += Time.deltaTime;

		if(time <= 0f)
		{
			gameObject.SetActive(false);
			PlayerManager.instance.target.SetSpecialCheck(false);
		}
		
		if(shot >= shotTime)
		{
			shot = 0f;
			GameObjectManager.instance.bulletManager.ObjectActive(points[Random.Range(0,len)].position
				,50f,3f,0,false,BulletBase.BulletTeam.Player).SetPenetrate(true).SetAnimation(GameObjectManager.instance.effectManager.spriteContainer.aniSet[2]);
		}
	}

	public override void Release()
	{

	}

	public void Active(float t)
	{
		time = t;
		shot = 0f;
		gameObject.SetActive(true);
	}
}
