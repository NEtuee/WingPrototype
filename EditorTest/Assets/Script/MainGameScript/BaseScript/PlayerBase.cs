using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : ObjectBase {

	public enum PlayerState
	{
		//미정 
	}
	public enum AttackType
	{
		Shooting,
		Fever,
	}

	public bool isAttack = false;

	protected Vector2 touchOrigin;
	protected Vector2 playerOrigin;

	protected AttackType attackType;

	protected float fireRate = 1f;
	protected float feverGague = 0f;
	protected float specialMoveTime = 0f;

	protected float attack = 1f;
	protected float fever = 0f;
	protected float special = 0f;
	protected float fire = 0f;

	//protected float moveDist = 0f;
	//protected float moveAccel = 0f;

	protected bool isMove = false;
	protected bool feverEnabled = false;
	protected bool feverCheck = false;
	protected bool specialEnabled = false; 
	protected bool specialCheck = false; 

	protected Transform mainSprite;

	public override void Initialize(){}
	public override void Progress(){}
	public override void Release(){}

	public abstract void Move(Vector2 touchPos);
	public abstract void Attack();
	public abstract void FeverAttack(Vector2 touchPos);

	float xSize = 0f;
	public virtual void IdleAnimation() 
	{
		xSize += 5 * Time.deltaTime;
		xSize = xSize >= 360f ? xSize - 360f : xSize;

		float sin = 1f + (Mathf.Sin(xSize) / 80f);
		Vector3 size = new Vector3(1f,sin,1f);
		mainSprite.localScale = size;

		// if(moveDist != 0f)
		// 	moveAccel += moveDist * Time.deltaTime * 40f;
		// if(moveAccel != 0f)
		// {
		// 	moveAccel = Mathf.Lerp(moveAccel,0f,2.5f * Time.deltaTime);
		// 	tp.rotation = Quaternion.Euler(new Vector3(0f,0f,(moveAccel * 3f)));
		// }

		// if(!isMove)
		// {
		// 	moveDist -= moveDist * Time.deltaTime;
		// 	if(moveDist <= 0f)
		// 		moveDist = 0f;
		// }
	}

	public float GetFever() {return feverGague;}
	public float GetSpecial() {return specialMoveTime;}
	public float GetAttack() {return attack;}
	public float GetFire() {return fireRate;}

	public float GetCurrFever() {return fever;}
	public float GetCurrFeverGague() {return fever / feverGague;}
	public float GetCurrSpecial() {return special;}
	public float GetCurrFire() {return fire;}

	public bool GetFeverEnabled() {return feverEnabled;}
	public bool GetFeverCheck() {return feverCheck;}
	public bool GetSpecialEnabled() {return specialEnabled;}
	public bool GetSpecialCheck() {return specialCheck;}
	public bool GetPlayerDead() {return isDead;}

	public void SetFever(float value) {feverGague = value;}
	public void SetSpecial(float value) {specialMoveTime = value;}
	public void SetFire(float value) {fireRate = value;}

	public void SetAttackType(AttackType type) {attackType = type;}
	public void SetAttack(bool value) {isAttack = value;}
	public void SetFever(bool value) {feverEnabled = value;}
	public void SetFeverCheck(bool value) {feverCheck = value;}
	public void SetSpecial(bool value) {specialEnabled = value;}
	public void SetSpecialCheck(bool value) {specialCheck = value;}

	public void IncreaseFever(float value) 
	{
		fever = Increaser(fever,feverGague,value);
		if(fever >= feverGague)
			SetFeverCheck(true);
	}
	public void IncreaseSpecial(float value)
	{
		special = Increaser(special,specialMoveTime,value);
		if(special >= specialMoveTime)
			SetSpecialCheck(true);
	}

	public void DecreaseFever(float value)
	{
		fever = Decreaser(fever,value);
		if(fever < feverGague)
			SetFeverCheck(false);
		if(fever <= 0)
		{
			SetFever(false);
			SetAttack(false);
			AttackTypeCheck();
		}
	}
	public void DecreaseSpecial(float value)
	{
		special = Decreaser(special,value);
		if(special < specialMoveTime)
			SetSpecial(false);
	}

	public void AttackTypeCheck()
	{
		if(feverEnabled && attackType == AttackType.Shooting)
			SetAttackType(AttackType.Fever);
		else if(!feverEnabled && attackType == AttackType.Fever)
			SetAttackType(AttackType.Shooting);
	}

	public void ActiveFever()
	{
		if(GetFeverCheck())
		{
			SetFever(true);
			AttackTypeCheck();
		}
	}

	public virtual void ActiveSpecialMove()
	{
		if(GetSpecialCheck())
		{
			SetSpecial(true);
		}
	}

	public virtual void CollisionCheck(BulletBase bullet)
	{
//		Debug.Log("PlayerColl");
		bullet.ColliseionActive(this);
	}
}
