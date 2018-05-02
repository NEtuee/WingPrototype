using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFeverEndDirect : DirectBase {

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

			if(mainTime >= 0.5f)
			{
				mainTime = 0f;
				check = true;
			}
		}
		else
		{
			//transforms[0].localPosition = Vector3.Lerp(Vector3.zero,pos,mainTime);

			if(mainTime >= 0.5f)
			{
				DirectEnd(false);
				PlayerManager.instance.ActiveGlobalDamage(1f);
				CameraControll.instance.Shake(0.7f,2.2f,35f);
			}
		}
	}
}
