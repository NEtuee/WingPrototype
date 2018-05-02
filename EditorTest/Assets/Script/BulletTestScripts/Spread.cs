using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spread : MonoBehaviour {

	public Vector3 shotPos;

	public int ani = 15;

	public float angle = 270f;
	public float randomRangeFactor = 30f;

	public float minSpeed = 10f;
	public float maxSpeed = 20f;

	public float delay = 3f;

	public int count = 10;

	public bool guided = false;

	private float time = 0f;

	private Vector3 originPos = new Vector3(-1f,-1f,-1f);

	public void ValueInit(Vector3 pos,int an,float ang,float randRangeFac,float min,float max,float del,int ct, bool guid)
	{
		shotPos = pos;
		ani = an;
		angle = ang;
		randomRangeFactor= randRangeFac;
		minSpeed = min;
		maxSpeed = max;
		delay = del;
		count = ct;
		guided = guid;

		Init();
	}

	public void Init()
	{
		time = 0f;
	}

	public void Shot()
	{
		if(guided)
		{
			Vector3 direction = PlayerManager.instance.GetDirection(transform.position);
			angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		}

		Vector3 stPos = shotPos;
		if(shotPos == originPos)
			stPos = transform.position;

		for(int i = 0; i < count; ++i)
		{
			float randomAngle = angle + Random.Range(-randomRangeFactor,randomRangeFactor);
			float speed = Random.Range(minSpeed,maxSpeed);
			BulletManager.instance.ObjectActive(null,stPos,speed,1f,randomAngle,false)
							.SetAnimation(ani,false);
		}
	}

	public void Progress()
	{
		time += Time.deltaTime;

		if(time >= delay)
		{
			time = 0f;

			Vector3 stPos = shotPos;
			if(shotPos == originPos)
				stPos = transform.position;

			if(guided)
			{
				Vector3 direction = PlayerManager.instance.GetDirection(stPos);
				angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			}

			for(int i = 0; i < count; ++i)
			{
				float randomAngle = angle + Random.Range(-randomRangeFactor,randomRangeFactor);
				float speed = Random.Range(minSpeed,maxSpeed);

				BulletManager.instance.ObjectActive(null,stPos,speed,1f,randomAngle,false)
								.SetAnimation(ani,false);
			}
		}
	}
}
