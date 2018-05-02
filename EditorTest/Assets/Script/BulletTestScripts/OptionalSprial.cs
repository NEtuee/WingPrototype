using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionalSprial : MonoBehaviour {

	public delegate void ShotFunc();

	public ShotFunc shotFunc;

	public Vector3 shotPos;

	public int count = 12;
	public int ani = 15;

	public float speed = 10f;
	public float delay = 0.5f;
	public float angleFactor = 45f;

	public float angleAccel = 10f;

	public float randomRangeFactor = 10f;

	private float angle = 0f;
	private float betweenAngle = 0f;
	private float time = 0f;
	private float random = 0f;

	private Vector3 originPos = new Vector3(-1f,-1f,-1f);

	public void SetDelegate(ShotFunc func)
	{
		shotFunc = func;
	}

	public void ValueInit(Vector3 shot, int ct, int an, float sp,float dela, float angFac,float angAcc,float randRangeFac)
	{
		shotPos = shot;
		count = ct;
		ani = an;
		speed = sp;
		delay = dela;
		angleFactor = angFac;
		angleAccel = angAcc;
		randomRangeFactor = randRangeFac;

		Init();
	}

	public void Init()
	{
		betweenAngle = 360f / (float)count;

		time = 0f;
		angle = 0f;
		random = 0f;
	}

	public void Progress()
	{
		time += Time.deltaTime;

		random = Random.Range(-randomRangeFactor,randomRangeFactor);

		if(time >= delay)
		{
			time = 0f;

			if(shotFunc != null)
				shotFunc();

			Vector3 pos = shotPos;
			if(pos == originPos)
				pos = transform.position;

			for(int i = 0; i < count; ++i)
				BulletManager.instance.ObjectActive(null,pos,speed,1f,angle + betweenAngle * (float)i + random,false,false)
								.SetAnimation(DatabaseContainer.instance.spriteDatabase.aniSet[ani],false).SetAngleAccel(angleAccel);
		}

		angle += angleFactor * Time.deltaTime;
	}
}
