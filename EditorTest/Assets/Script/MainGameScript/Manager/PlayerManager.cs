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

	public void FeverDirectProgress()
	{
		target.feverDirect.Progress();
	}

	public void FeverActive()
	{
		target.feverDirect.Initialize();
		// target.feverBase.Initialize();
		// target.feverBase.feverEndDirect.Initialize();

		//=================================================check
		BulletManager.instance.ChangeScoreObjects();

		target.ActiveFever();
	}

	public void ActiveGlobalDamage(float time)
	{
		target.globalDamage.Active(10f,true,time,true);
	}

	public bool IsFever() {return target.GetFeverEnabled();}

	public bool PlayerIsDead() {return target.IsDead();}

	public void SetAttack(bool value) {target.SetAttack(value);}
	public void SetAttackType(PlayerBase.AttackType type) {target.SetAttackType(type);}

	public void SetFever(float value)
	{
		target.IncreaseFever(value);
		target.IncreaseSpecial(value);
	}

	public Vector3 GetDirection(Vector3 pos) {return (target.tp.position - pos).normalized;}

	public void CollisionCheck(BulletBase bullet)
	{
		if(bullet.Collision(target))
		{
			target.CollisionCheck(bullet);
		}
	}
}
