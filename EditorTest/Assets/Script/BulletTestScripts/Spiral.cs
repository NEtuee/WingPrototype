using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiral : MonoBehaviour {

	public float timeLimit = 0.05f;

	public float time = 9999f;

	public float angler = 0f;

	void Update () 
	{
		time += Time.deltaTime;
		angler += 120 * Time.deltaTime;
		angler -= angler >= 360f ? 360f : 0f;
		if(time < timeLimit)
			return;

		time = 0f;

		for(int i = 0; i < 36; ++i)
		{
			BulletManager.instance.ObjectActive(null,Vector2.zero,10f,1f,i * 10f + Mathf.Sin(angler * Mathf.Deg2Rad) * 20f,true,false).
					SetAnimation(DatabaseContainer.instance.spriteDatabase.aniSet[0],false);
		}
	}
}
