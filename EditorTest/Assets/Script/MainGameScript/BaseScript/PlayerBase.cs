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

	public DirectBase feverDirect;
	public FeverBase feverBase;

	public GlobalDamage globalDamage;

	protected Vector2 touchOrigin;
	protected Vector3 playerOrigin;

	protected AttackType attackType;

	protected ObjectBase nearEnemy;

	protected float fireRate = 1f;
	protected float feverGague = 0f;
	protected float specialMoveTime = 0f;

	protected float attack = 1f;
	protected float fever = 0f;
	protected float special = 0f;
	protected float fire = 0f;

	protected float nearDist = 0f;

	//protected float moveDist = 0f;
	//protected float moveAccel = 0f;

	protected bool isMove = false;
	protected bool feverEnabled = false;
	protected bool feverCheck = false;
	protected bool specialEnabled = false; 
	protected bool specialCheck = false; 

	protected Transform mainSprite;

#region MovementVars
	Touch touchs;
	Touch movementTouch;
	Touch atkTouch;

	protected List<Vector3> movePosList = new List<Vector3>();

	protected int moveLimit = 10;

	protected float speed = 80f;
	protected float moveTime = 0f;
	protected float time = 0f;

	protected Vector3 posOrigin;
	protected Vector3 targetPos;
#endregion
#region ObjectBaseFuncs
	public override void Initialize()
	{
		GetTransform();
		GetChildCollider();

		SetFire(0.07f);
		SetFever(100f);
		SetMaxHp(10);
		SetSpecial(10f);

		SetImmortal(true);

        feverBase.FirstInit();

		mainSprite = GetTransform("Sprite");
		
		InitTouchId();
	}

	public override void Progress()
	{
		TouchCheck();		//키 체크
		IdleAnimation();	//아이들 애니메이션
		NearEnemyIsDead();	//근처의 적이 죽었는가 체크

		Move();				//움직이기

		if(fire < fireRate)
			fire = Increaser(fire,fireRate,Time.deltaTime);

		if(!GetSpecialCheck())
		{
			IncreaseSpecial(Time.deltaTime);
		}

		if(GetSpecialEnabled())
		{
			special = 0f;
			SetSpecial(false);
		}

		if(attackType == AttackType.Shooting)
			Attack();
		else if(attackType == AttackType.Fever)
		{
			DecreaseFever(8 * Time.deltaTime);
			//FeverAttack();
		}
	}

	public override void Release(){}
#endregion

	public virtual void Move(Vector2 touchPos)
	{
		Vector3 vec = new Vector3((touchOrigin.x - Camera.main.ScreenToWorldPoint(touchPos).x),
											(touchOrigin.y - Camera.main.ScreenToWorldPoint(touchPos).y));

		if(movePosList.Count < moveLimit)
		{
			if(movePosList.Count != 0)
			{
				if(movePosList[movePosList.Count - 1] != vec)
				{
					movePosList.Add(vec);
				}
			}
			else
			{
				movePosList.Add(vec);
				targetPos = playerOrigin - vec;
			}
		}

		moveTime += Time.deltaTime;

		// Vector2 pos = new Vector2(playerOrigin.x - (touchOrigin.x - Camera.main.ScreenToWorldPoint(touchPos).x)
		// 						,playerOrigin.y - (touchOrigin.y - Camera.main.ScreenToWorldPoint(touchPos).y));

		// tp.position = pos;
	}

	public virtual void Move()
	{
		if(movePosList.Count != 0)
		{
			float val = speed * Time.deltaTime / Vector3.Distance(posOrigin,targetPos);
			time += val;

			if(val >= 1f)
			{
				time = 0f;

				posOrigin = targetPos;
				targetPos = playerOrigin - movePosList[0];
				tp.position = Vector3.Lerp(posOrigin,targetPos,time);

				movePosList.RemoveAt(0);
			}
			else
				tp.position = Vector3.Lerp(posOrigin,targetPos,time);
			
			if(time >= 1f)
			{
				time -= 1f;

				posOrigin = targetPos;
				targetPos = playerOrigin - movePosList[0];
				tp.position = Vector3.Lerp(posOrigin,targetPos,time);

				movePosList.RemoveAt(0);
			}
		}
	}
	
	public virtual void Attack()
	{
		if(!isAttack)
			return;

		if(fire < fireRate)
			return;

		fire = 0f;

		// GameObjectManager.instance.bulletManager.ObjectActive(tp.position,40f,1f,5f,BulletBase.BulletTeam.Player);
		// GameObjectManager.instance.bulletManager.ObjectActive(tp.position,40f,1f,2.5f,BulletBase.BulletTeam.Player);
		BulletActive();
		// GameObjectManager.instance.bulletManager.ObjectActive(tp.position,40f,1f,-2.5f,BulletBase.BulletTeam.Player);
		// GameObjectManager.instance.bulletManager.ObjectActive(tp.position,40f,1f,-5f,BulletBase.BulletTeam.Player);
	}
	//public abstract void FeverAttack(Vector2 touchPos);

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

	public virtual void TouchCheck()
	{
#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0))
		{
			isMove = true;
			isAttack = true;
			moveTime = 0f;
			playerOrigin = tp.position;
			touchOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			if(movePosList.Count == 0)
				posOrigin = playerOrigin;
		}
		else if(Input.GetMouseButton(0) && isMove)
		{
			Move(Input.mousePosition);
		}
		else if(Input.GetMouseButtonUp(0))
		{
			isMove = false;
			isAttack = false;

			if(GetFeverEnabled())
				isAttack = false;
		}
#else
		MobileDebugger.instance.SetKeep("터치 : " + Input.touchCount,0);
		if(Input.touchCount > 0 && !isDead)
		{
			for(int i = 0; i < Input.touchCount; ++i)
			{
				touchs = Input.GetTouch(i);

				if(touchs.phase == TouchPhase.Began)
				{
					if(movementTouch.fingerId == -1)
					{
						MobileDebugger.instance.AddLine("왼쪽 터치");
						MobileDebugger.instance.SetKeep("움직임 눌림 : " + Input.touchCount,2);

						movementTouch = touchs;
						playerOrigin = tp.position;
				 		touchOrigin = Camera.main.ScreenToWorldPoint(touchs.position);

						isAttack = true;
					}
				}
				else if(touchs.phase == TouchPhase.Moved)
				{
					if(touchs.fingerId == movementTouch.fingerId)
					{
						MobileDebugger.instance.SetKeep("움직임 움직임 : " + touchs.fingerId,2);

						Move(touchs.position);
					}
				}
				else if(touchs.phase == TouchPhase.Ended)
				{
					if(touchs.fingerId == movementTouch.fingerId)
					{
						isAttack = false;
						MobileDebugger.instance.SetKeep("움직임 안눌림",2);
						movementTouch.fingerId = -1;
					}
				}
			}
		}
#endif

	}

	public virtual void InitTouchId()
	{
		movementTouch.fingerId = -1;
		atkTouch.fingerId = -1;
	}

	public virtual void BulletActive()
	{
		Vector3 pos = tp.position;
		pos.x += 2f;

		BulletManager.instance.ObjectActive(this,pos,100f,
			1,80f,false,false,BulletBase.BulletTeam.Player).
			SetAnimation(DatabaseContainer.instance.spriteDatabase.aniSet[8]).SetRadius(1f);
		pos.x -= 4f;
		BulletManager.instance.ObjectActive(this,pos,100f,
			1,100f,false,false,BulletBase.BulletTeam.Player).
			SetAnimation(DatabaseContainer.instance.spriteDatabase.aniSet[8]).SetRadius(1f);
		pos.x += 2f;
		pos.y += 3f;
		BulletManager.instance.ObjectActive(this,pos,100f,
			1,90f,false,false,BulletBase.BulletTeam.Player).
			SetAnimation(DatabaseContainer.instance.spriteDatabase.aniSet[5]).SetRadius(1f);

		// GameObjectManager.instance.bulletManager.ObjectActive(this,pos,100f,
		// 	SaveDataContainer.instance.saveData.GetCurrCharInfo().GetAttackLevel(),80f,false,false,BulletBase.BulletTeam.Player).
		// 	SetAnimation(GameObjectManager.instance.effectManager.spriteContainer.aniSet[8]).SetRadius(1f);
		// pos.x -= 4f;
		// GameObjectManager.instance.bulletManager.ObjectActive(this,pos,100f,
		// 	SaveDataContainer.instance.saveData.GetCurrCharInfo().GetAttackLevel(),100f,false,false,BulletBase.BulletTeam.Player).
		// 	SetAnimation(GameObjectManager.instance.effectManager.spriteContainer.aniSet[8]).SetRadius(1f);
		// pos.x += 2f;
		// pos.y += 3f;
		// GameObjectManager.instance.bulletManager.ObjectActive(this,pos,100f,
		// 	SaveDataContainer.instance.saveData.GetCurrCharInfo().GetAttackLevel(),90f,false,false,BulletBase.BulletTeam.Player).
		// 	SetAnimation(GameObjectManager.instance.effectManager.spriteContainer.aniSet[5]).SetRadius(1f);
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

	public void SetNearEnemy(ObjectBase enemy) {nearEnemy = enemy;}
	public void SetNearDist(float value) {nearDist = value;}
	public ObjectBase GetNearEnemy() {return NearEnemyCheck() ? nearEnemy : null;}
	public bool NearEnemyCheck() {return nearEnemy != null;}
	public void NearEnemyIsDead() {nearEnemy = NearEnemyCheck() ? (nearEnemy.IsDead() ? null : nearEnemy) : null;}

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
			feverBase.EndEvent();
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

	public void SetNoneAttack()
	{
		isAttack = false;
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

	public void MoveCenter()
	{
		tp.position = Vector3.Lerp(tp.position,Vector3.zero,Time.deltaTime * 4f);
	}
}
