using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy2 : EnemyBase {

	public override void SetEnemy(Vector3 pos,PathDatabase.PathInfo p, EnemyDatabase.EnemyInfo e) //임시
	{
		SetDead(false);
		SetMaxHp(100);
		tp.position = pos;

		gameObject.SetActive(true);
	}

	public override void Move()
	{

	}

	float fire = 0f;
	public override void Shot()
	{
		fire += Time.deltaTime;

		if(fire >= 1f)
		{
			fire = 0f;

			if(!EnemyManager.instance.attack)
				return;
			for(int i = 0; i < 360; i += 10)
			{
				GameObjectManager.instance.bulletManager.ObjectActive(tp.position,20f,1f,i).
					SetAnimation(GameObjectManager.instance.effectManager.spriteContainer.aniSet[0]);
			}
		}
	}
}
