using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestFeverDirect : DirectBase {

	public Vector3 pos;

	bool check = false;

	public override void Initialize()
	{
		base.Initialize();

		check = false;
	}

	public override void Progress()
	{
		mainTime += Time.deltaTime;

		if(!check)
		{
			//transforms[0].localPosition = Vector3.Lerp(resetPos[0],Vector3.zero,mainTime);

			if(mainTime >= 1f)
			{
				mainTime = 0f;
				check = true;
			}
		}
		else
		{
			//transforms[0].localPosition = Vector3.Lerp(Vector3.zero,pos,mainTime);

			if(mainTime >= 1f)
			{
				DirectEnd();
				PlayerManager.instance.target.feverBase.Initialize();
			}
		}
	}
}
