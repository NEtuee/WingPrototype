using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMove2 : ObjectBase {

	public float time;


	public override void Initialize()
	{

	}

	public override void Progress()
	{
		time -= Time.deltaTime;

		if(time <= 0f)
		{
			gameObject.SetActive(false);
			PlayerManager.instance.target.SetSpecialCheck(false);
		}
	
	}

	public override void Release()
	{

	}

	public void Active(float t)
	{
		time = t;
		gameObject.SetActive(true);
	}
}
