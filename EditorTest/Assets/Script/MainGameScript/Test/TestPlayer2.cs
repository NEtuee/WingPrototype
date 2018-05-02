using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPlayer2 : PlayerBase {

	public SpecialMove2 spObject;

	public override void ActiveSpecialMove()
	{
		if(GetSpecialCheck())
		{
			SetSpecial(true);
			GameObjectManager.instance.bulletManager.DisableAllObjects();
			spObject.Active(3f);
		}
	}

	public override void BulletActive()
	{
		Vector3 pos = tp.position;

		pos.y += 3f;
		GameObjectManager.instance.bulletManager.ObjectActive(this,pos,100f,
			SaveDataContainer.instance.saveData.GetCurrCharInfo().GetAttackLevel(),90f,false,false,BulletBase.BulletTeam.Player).
			SetAnimation(GameObjectManager.instance.effectManager.spriteContainer.aniSet[9]).SetRadius(1f);
	}

}
