using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffrentSpeed : MonoBehaviour {

	public float timeLimit = 0.05f;
	public float minSpeed = 5f;
	public float maxSpeed = 10f;

	public int bulletCount = 10;

	public float angle = 0f;

	public bool guided = false;

	private float time = 9999f;

	void Update () 
	{
		time += Time.deltaTime;
		if(time < timeLimit)
			return;

		time = 0f;

		if(guided)
		{
			Vector3 direction = PlayerManager.instance.GetDirection(transform.position);
			angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		}

		for(int i = 0; i < bulletCount; ++i)
		{
			BulletManager.instance.
					ObjectActive(null,transform.position,Mathf.Lerp(minSpeed,maxSpeed,(float)i / (float)bulletCount),1f,angle,true,false).
					SetAnimation(DatabaseContainer.instance.spriteDatabase.aniSet[15],false);
		}

		angle += 10;
		angle -= angle >= 360f ? 360f : 0f;
	}
}
