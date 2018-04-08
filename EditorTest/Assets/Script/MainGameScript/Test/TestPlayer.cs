using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPlayer : PlayerBase {

	public TrailCollider attackObject;
	public SpecialMove spObject;

	Touch touchs;
	Touch movementTouch;
	Touch atkTouch;

	public List<Vector3> movePosList = new List<Vector3>();

	public int moveLimit = 10;

	public float speed = 5f;
	private float moveTime = 0f;
	public float time = 0f;

	private Vector3 posOrigin;
	private Vector3 targetPos;


	public override void Initialize()
	{
		GetTransform();
		GetChildCollider();

		SetFire(0.07f);
		SetFever(100f);
		SetMaxHp(10);
		SetSpecial(10f);

		SetImmortal(true);

		mainSprite = GetTransform("Sprite");
		
		InitTouchId();
	}
	public override void Progress()
	{
		TouchCheck();
		IdleAnimation();

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

		if(Input.GetKeyDown(KeyCode.A))
			SetAttack(true);
		if(Input.GetKeyUp(KeyCode.A))
			SetAttack(false);

		if(fire < fireRate)
			fire = Increaser(fire,fireRate,Time.deltaTime);

		attackObject.progressCheck = GetFeverEnabled();

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
	public override void Release()
	{

	}

	public void TouchCheck()
	{
#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0))
		{
			// if(Input.mousePosition.x > Screen.width / 3)
			// {
			// 	if(GetFeverEnabled())
			// 	{
			// 		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			// 	 	pos.z = 0;
			// 	 	attackObject.transform.position = pos;
			// 		isAttack = true;

			// 		attackObject.Active();
			// 	}
			// }
			// else
			{
				isMove = true;
				isAttack = true;
				moveTime = 0f;
				playerOrigin = tp.position;
				touchOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				if(movePosList.Count == 0)
					posOrigin = playerOrigin;

			}
		}
		else if(Input.GetMouseButton(0) && isMove)
		{
			Vector3 vec = new Vector3((touchOrigin.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x),
											(touchOrigin.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y));

			// if(val >= 1f)
			// {
			// 	Debug.Log("check");
			// 	tp.position = playerOrigin - vec;
			// }
			//else 
			if(/*moveTime >= moveUpdateTime &&*/ movePosList.Count < moveLimit)
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
				//moveTime = 0f;
			}

			moveTime += Time.deltaTime;

			// Vector2 pos = new Vector2(playerOrigin.x - (touchOrigin.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x)
			// 						,playerOrigin.y - (touchOrigin.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y));

			// //moveDist = pos.y - tp.position.y;
			// //isUp = tp.position.y > pos.y ? true : false;

			// tp.position = pos;
		}
		else if(Input.GetMouseButton(0) && isAttack && GetFeverEnabled())
		{
			FeverAttack(Input.mousePosition);
		}
		else if(Input.GetMouseButtonUp(0))
		{
			isMove = false;
			isAttack = false;
			//moveDist = 0f;

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
					// if(touchs.position.x > Screen.width / 3) //공격
					// {
					// 	MobileDebugger.instance.AddLine("오른쪽 터치");
					// 	MobileDebugger.instance.SetKeep("공격 눌림 : " + Input.touchCount,1);

					// 	if(GetFeverEnabled())
					// 	{
					// 		atkTouch = touchs;
					// 		Vector3 pos = Camera.main.ScreenToWorldPoint(touchs.position);
				 	// 		pos.z = 0;
				 	// 		attackObject.transform.position = pos;

					// 		attackObject.Active();
					// 		isAttack = true;
					// 	}
					// }
					// else
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
					if(touchs.fingerId == atkTouch.fingerId)
					{
						MobileDebugger.instance.SetKeep("공격 움직임 : " + touchs.fingerId,1);

						FeverAttack(touchs.position);
					}
					else if(touchs.fingerId == movementTouch.fingerId)
					{
						MobileDebugger.instance.SetKeep("움직임 움직임 : " + touchs.fingerId,2);

						Move(touchs.position);
					}
				}
				else if(touchs.phase == TouchPhase.Ended)
				{
					if(touchs.fingerId == atkTouch.fingerId)
					{
						MobileDebugger.instance.SetKeep("공격 안눌림",1);
						atkTouch.fingerId = -1;
					}
					else if(touchs.fingerId == movementTouch.fingerId)
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

	public void InitTouchId()
	{
		movementTouch.fingerId = -1;
		atkTouch.fingerId = -1;
	}

	public override void Move(Vector2 touchPos)
	{
		Vector2 pos = new Vector2(playerOrigin.x - (touchOrigin.x - Camera.main.ScreenToWorldPoint(touchPos).x)
								,playerOrigin.y - (touchOrigin.y - Camera.main.ScreenToWorldPoint(touchPos).y));

		tp.position = pos;
	}
	public override void Attack()
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
	public override void FeverAttack(Vector2 touchPos)
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(touchPos);
				 		pos.z = 0;
		attackObject.tp.position = pos;

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


}
