using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : ObjectBase {

	public static PlayerManager instance;
	public PlayerBase target;


	public override void Initialize()
	{
		instance = this;
		target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBase>();
		target.Initialize();
	}

	public override void Progress()
	{

	}

	public override void Release()
	{

	}

	public void PlayerProgress()
	{
		target.Progress();
	}

	public bool PlayerIsDead() {return target.IsDead();}

	public void SetAttack(bool value) {target.SetAttack(value);}
	public void SetAttackType(PlayerBase.AttackType type) {target.SetAttackType(type);}

	public void SetFever(float value)
	{
		target.IncreaseFever(value);
		target.IncreaseSpecial(value);
	}

	public void CollisionCheck(BulletBase bullet)
	{
		if(target.Collision(bullet))
		{
			target.CollisionCheck(bullet);
		}
	}
}
