using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public Transform tp;
	public Vector3 dir;
	public float speed;

	public void Init(float a, float s)
	{
		a = a * Mathf.Deg2Rad;
		dir = new Vector2(Mathf.Cos(a),Mathf.Sin(a));
		speed = s;
	}

	public void Update()
	{
		if(!Test.instance.staticEvent)
		{
			if(tp.position.x < -25f || tp.position.x > 25f
				|| tp.position.y < -15f || tp.position.y > 15f)
			{
				Destroy(gameObject);
			}

			tp.position += dir * speed * Time.deltaTime;
		}
	}
}
