using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPlayer : PlayerBase {

	public SpecialMove spObject;
	
	public override void Release()
	{

	}

	public override void ActiveSpecialMove()
	{
		if(GetSpecialCheck())
		{
			SetSpecial(true);
			GameObjectManager.instance.bulletManager.DisableAllObjects();
			spObject.Active(5f);
		}
	}

	public override void BulletActive()
	{
		Vector3 pos = tp.position;
		pos.y += 0.5f;
		BulletManager.instance.ObjectActive(this,pos,35f,1f,90f,false,false,BulletBase.BulletTeam.Player)
					.SetAnimation(13,false);
	}

}
