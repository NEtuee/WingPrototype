using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistOptionalSprial : MonoBehaviour {

	public Vector3 shotPos;

	public int count = 12;
	public int twistCount = 5;

	public int ani = 15;

	public float speed = 10f;
	public float delay = 0.5f;
	public float angleFactor = 45f;
	public float twistAngle = 5f;

	public bool half = false;

	private int shotCount = 0;
	private float angle = 0f;
	private float betweenAngle = 0f;
	private float time = 0f;

	private Vector3 originPos = new Vector3(-1f,-1f,-1f);

	public void ValueInit(Vector3 shot, int ct,int tct, int an, float sp,float dela, float angFac,float twist)
	{
		shotPos = shot;
		count = ct;
		twistCount = tct;
		ani = an;
		speed = sp;
		delay = dela;
		angleFactor = angFac;
		twistAngle = twist;

		Init();
	}

	public void Init()
	{
		betweenAngle = 360f / (float)count;

		shotCount = 0;
		time = 0f;
		angle = 0f;

		half = false;
	}

	public void Progress()
	{
		time += Time.deltaTime;

		if(time >= delay)
		{
			time = 0f;

			++shotCount;

			float calcAngle = angle;
			float calcSpeed = speed;
			if(shotCount > twistCount && twistCount != 0)
			{
				shotCount = 0;
				calcAngle += half ? betweenAngle / 2f : twistAngle;
				calcSpeed += speed;
			}

			Vector3 pos = shotPos;
			if(pos == originPos)
				pos = transform.position;

			for(int i = 0; i < count; ++i)
				BulletManager.instance.ObjectActive(null,pos,calcSpeed,1f,calcAngle + betweenAngle * (float)i,false,false)
								.SetAnimation(DatabaseContainer.instance.spriteDatabase.aniSet[ani],false);

		}

		angle += angleFactor * Time.deltaTime;
	}
}
