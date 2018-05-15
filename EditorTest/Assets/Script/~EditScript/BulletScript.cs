using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public Transform tp;
	public Vector3 dir;
	public float speed;
	public float angle;
	public float angleAccel;
	public float speedAccel;

	public void Init(float a, float s,float acc,float scc)
	{
		angle = a;
		dir = new Vector2(Mathf.Cos(a * Mathf.Deg2Rad),Mathf.Sin(a * Mathf.Deg2Rad));
		speed = s;

		angleAccel = acc;
		speedAccel = scc;
	}

	public void Update()
	{
		if(!Test.instance.staticEvent)
		{
			if(tp.position.x < -15f || tp.position.x > 15f
				|| tp.position.y < -25f || tp.position.y > 25f)
			{
				Destroy(gameObject);
			}

			angle += angleAccel * Time.deltaTime;
			speed += speedAccel * Time.deltaTime;
			
			dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),Mathf.Sin(angle * Mathf.Deg2Rad));
			tp.position += dir * speed * Time.deltaTime;
		}
	}
}
