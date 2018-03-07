using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour {

	public static CameraControll instance;

	public float angle = 0f;
	public float factor = 1f;
	public float speed = 360f;
	public float decreaseSpeed = 1f;

	public bool shake = false;

	public void Start()
	{
		instance = this;
	}

	public void Update()
	{
		if(shake)
		{
			angle += speed * Time.deltaTime;
			angle -= angle >= 360f ? 360 : 0;

			factor -= decreaseSpeed * Time.deltaTime;

			transform.position = new Vector3(Mathf.Sin(angle) * factor,Mathf.Cos(angle / 10) * factor,-10f);
			if(factor <= 0f)
			{
				transform.position = new Vector3(0f,0f,-10f);
				shake = false;
			}
		}
	}

	public void Shake(float f,float d = 1f,float s = 360f)
	{
		factor = f;
		decreaseSpeed = d;
		speed = s;

		angle = 0f;

		shake = true;
	}
}
