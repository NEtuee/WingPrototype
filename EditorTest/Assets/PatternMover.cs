using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternMover : MonoBehaviour {

	public enum MoveType
	{
		Create,
		Enter,
		PatternOne_One,
		PatternOne_Two,
		PatternOne_Three,
		PatternTwo_One,
		PatternTwo_Two,
		PatternTwo_Three,
		PatternTwo_Four,
	}

#region Variables
	public GameObject test;

	public MoveType type;
	public MoveType prevType;

	public Vector2 startPoint;
	public Vector2 enterPoint;
	public float enterSpeed;


	public OptionalSprial patternOne_One_OptSpiral_0;
	public OptionalSprial patternOne_One_OptSpiral_1;
	public Spread patternOne_One_spread;

	private int patternOne_One_BulletCount = 0;

	public Vector2 patternOne_One_shotPos_0;
	public Vector2 patternOne_One_shotPos_1;


	public TwistOptionalSprial patternOne_Two_TwistOptSpiral;
	public Vector2 patternOne_Two_enterPoint;
	public Vector2[] patternOne_Two_MovePath;

	private int patternOne_Two_MoveCount = 0;


	private int patternOne_Three_BulletCount = 0;


	public Vector3 patternTwo_One_ShotPos_0;
	public Vector3 patternTwo_One_ShotPos_1;
	public Spread patternTwo_One_Spread;


	public Vector2[] patternTwo_Two_firePos;
	public Vector2 patternTwo_Two_leftCirclePos;
	public Vector2 patternTwo_Two_rightCirclePos;

	public Vector2 patternTwo_Two_leftCharacterPos;
	public Vector2 patternTwo_Two_rightCharacterPos;

	public Transform patternTwo_Two_leftCircle;
	public Transform patternTwo_Two_rightCircle;


	public bool patternTwo_Three_Progress = false;
	public int patternTwo_Three_PatternCount = 0;


	private float publicTime = 0f;
	private float stayTime = 0f;
#endregion

	public void Update()
	{

		if(PlayerManager.instance.IsFever() || PlayerManager.instance.target.feverBase.feverEndDirect.IsProgressing())
			return;

		
		if(stayTime != 0)
		{
			stayTime -= Time.deltaTime;

			if(stayTime <= 0f)
			{
				stayTime = 0f;
			}
			else
				return;
		}

		if(patternTwo_Three_Progress)
		{
			patternOne_Two_TwistOptSpiral.Progress();

			if(patternTwo_Three_PatternCount >= 5f)
			{
				patternTwo_Three_Progress = false;
				type = MoveType.PatternTwo_Four;
			}
		}

		if(type == MoveType.Enter)
		{
			Enter();
		}
		else if(type == MoveType.PatternOne_One)
		{
			PatternOne_One();
		}
		else if(type == MoveType.PatternOne_Two)
		{
			PatternOne_Two();
		}
		else if(type == MoveType.PatternOne_Three)
		{
			PatternOne_Three();
		}
		else if(type == MoveType.PatternTwo_One)
		{
			PatternTwo_One();
		}
		else if(type == MoveType.PatternTwo_Two)
		{
			PatternTwo_Two();
		}
		else if(type == MoveType.PatternTwo_Three)
		{
			PatternTwo_Three();
		}
		else if(type == MoveType.PatternTwo_Four)
		{
			PatternTwo_Four();
		}
	}

#region Enter
	public void Enter()
	{
		transform.position = MathEx.EaseOutCubicVector2(startPoint,enterPoint,publicTime);

		publicTime += enterSpeed * Time.deltaTime;

		if(publicTime >= 1f)
		{
			publicTime = 0f;
			stayTime = 1.5f;

			transform.position = enterPoint;

			type = MoveType.PatternOne_One;
		}
	}
#endregion
#region PatternOne_One
	public void PatternOne_One_Init()
	{
		patternOne_One_OptSpiral_0.ValueInit(patternOne_One_shotPos_0,16,17,5f,0.5f,45,0f,15f);
		patternOne_One_OptSpiral_0.SetDelegate(IncreasePatternOneBulletCount);
		patternOne_One_OptSpiral_1.ValueInit(patternOne_One_shotPos_1,16,17,5f,0.5f,-45,0f,15f);
		patternOne_One_OptSpiral_1.SetDelegate(IncreasePatternOneBulletCount);

		patternOne_One_spread.ValueInit(new Vector3(-1f,-1f,-1f),20,270f,30f,5f,12f,3f,20,true);

		patternOne_One_BulletCount = 0;
	}
	public void PatternOne_One()
	{
		if(prevType != type)
		{
			prevType = type;
			PatternOne_One_Init();
		}

		patternOne_One_OptSpiral_0.Progress();
		patternOne_One_OptSpiral_1.Progress();

		if(patternOne_One_BulletCount >= 40)
		{
			patternOne_One_spread.Shot();
			stayTime = 1.5f;
			type = MoveType.PatternOne_Two;
		}
	}
	public void IncreasePatternOneBulletCount()
	{
		++patternOne_One_BulletCount;
	}
#endregion
#region PatternOne_Two
	public void PatternOne_Two_Init()
	{
		patternOne_Two_TwistOptSpiral.ValueInit(new Vector3(-1f,-1f,-1f),36,0,16,8f,0.3f,0f,0f);
		publicTime = 0f;
		patternOne_Two_MoveCount = -1;

		startPoint = transform.position;
		enterPoint = patternOne_Two_enterPoint;
	}
	public void PatternOne_Two()
	{
		if(prevType != type)
		{
			prevType = type;
			PatternOne_Two_Init();
		}

		transform.position = Vector3.Lerp(startPoint,enterPoint,publicTime);

		publicTime += 0.7f * Time.deltaTime;

		patternOne_Two_TwistOptSpiral.Progress();

		if(publicTime >= 1f)
		{
			if(patternOne_Two_MoveCount == patternOne_Two_MovePath.Length -1)
			{
				stayTime = 0.8f;
				type = MoveType.PatternOne_Three;

				return;
			}

			publicTime = 0f;

			transform.position = enterPoint;

			startPoint = enterPoint;
			enterPoint = patternOne_Two_MovePath[++patternOne_Two_MoveCount];

			if(patternOne_Two_MoveCount == 0)
			{
				patternOne_Two_TwistOptSpiral.ValueInit(new Vector3(-1f,-1f,-1f),36,1,17,5f,0.6f,180f,4f);
			}
		}
	}
#endregion
#region PatternOne_Three
	List<BulletBase> bulletList = new List<BulletBase>();
	public void PatternOne_Three_Init()
	{
		patternOne_One_OptSpiral_0.ValueInit(new Vector3(-1f,-1f,-1f),3,19,10f,0.1f,180f,15f,15f);
		patternOne_One_OptSpiral_0.SetDelegate(null);

		bulletList.Clear();

		patternOne_Three_BulletCount = 0;
		publicTime = 0f;
	}
	public void PatternOne_Three()
	{
		if(prevType != type)
		{
			prevType = type;
			PatternOne_Three_Init();
		}

		patternOne_One_OptSpiral_0.Progress();

		if(patternOne_Three_BulletCount < 8)
		{
			publicTime += Time.deltaTime;
			if(publicTime >= 1.5f)
			{
				++patternOne_Three_BulletCount;
				publicTime = 0f;
				for(int i = 0; i < 8; ++i)
				{
					bulletList.Add(BulletManager.instance.ObjectActive(null,transform.position,3f,1f,i * 10f + 260f,false,false)
									.SetAngleAccel(180f).SetAnimation(17,false)
									.SetAccel(0.5f));
				}

				for(int i = 0; i < 8; ++i)
				{
					bulletList.Add(BulletManager.instance.ObjectActive(null,transform.position,3f,1f,i * 10f + 210f,false,false)
									.SetAngleAccel(-180f).SetAnimation(17,false)
									.SetAccel(0.5f));
				}
			}
		}
		else
		{
			for(int i = 0; i < bulletList.Count; ++i)
			{
				bulletList[i].SetAngleAccel(0f).SetAccel(4f);
			}

			for(int i = 0; i < 8; ++i)
			{
				for(int j = 0; j < 8; ++j)
				{
					BulletManager.instance.
						ObjectActive(null,transform.position,Mathf.Lerp(7f,13f,(float)j / (float)8),1f,(float)(i * 15) + 217.5f,true,false).
						SetAnimation(DatabaseContainer.instance.spriteDatabase.aniSet[20],false).SetAccel(1f);
				}
			}

			type = MoveType.PatternTwo_One;
			stayTime = 2.5f;
		}
	}
	#endregion
#region PatternTwo_One
	public void PatternTwo_One_Init()
	{
		patternOne_One_spread.ValueInit(patternTwo_One_ShotPos_0,18,270f,0f,8f,13f,0.33f,1,true);
		patternTwo_One_Spread.ValueInit(patternTwo_One_ShotPos_1,18,270f,0f,8f,13f,0.3f,1,true);
		publicTime = 0f;
	}
	public void PatternTwo_One()
	{
		if(prevType != type)
		{
			prevType = type;
			PatternTwo_One_Init();
		}

		publicTime += Time.deltaTime;

		patternOne_One_spread.Progress();
		patternTwo_One_Spread.Progress();

		if(publicTime >= 3f)
		{
			if(patternTwo_Three_Progress)
				++patternTwo_Three_PatternCount;

			type = MoveType.PatternTwo_Two;
			stayTime = 0.5f;
		}
	}
#endregion
#region PatternTwo_Two
	private bool progressDirection = true;
	private bool direction = true; //true = left circle pos
	private bool circleRotate = false;
	private bool fireActive = false;
	private int firePosCount = 0;

	private Vector3 originPos;
	public void PatternTwo_Two_Init()
	{
		progressDirection = true;
		direction = true;
		fireActive = false;

		publicTime = 0f;
		originPos = transform.position;

		firePosCount = 0;

		circleRotate = false;
	}

	public void PatternTwo_Two()
	{
		if(prevType != type)
		{
			prevType = type;
			PatternTwo_Two_Init();
		}

		if(progressDirection)
		{
			publicTime += Time.deltaTime;
			if(direction)
			{
				if(!patternTwo_Two_leftCircle.gameObject.activeSelf)
				{
					patternTwo_Two_leftCircle.gameObject.SetActive(true);
					circleRotate = true;
				}

				if(circleRotate)
				{
					patternTwo_Two_leftCircle.position = MathEx.EaseOutQuadVector2(originPos,patternTwo_Two_leftCirclePos,publicTime);
				}

				patternTwo_Two_leftCircle.localRotation = Quaternion.Euler(0f,0f,MathEx.easeOutCubic(0f,360f,publicTime));
				transform.position = MathEx.EaseOutQuadVector2(originPos,patternTwo_Two_rightCharacterPos,publicTime);

				if(publicTime >= 1f)
				{
					transform.position = patternTwo_Two_rightCharacterPos;
					patternTwo_Two_leftCircle.position = patternTwo_Two_leftCirclePos;
					patternTwo_Two_leftCircle.localRotation = Quaternion.Euler(0f,0f,0f);
				}
			}
			else
			{
				if(!patternTwo_Two_rightCircle.gameObject.activeSelf)
				{
					patternTwo_Two_rightCircle.gameObject.SetActive(true);
					circleRotate = true;
				}

				if(circleRotate)
				{
					patternTwo_Two_rightCircle.position = MathEx.EaseOutQuadVector2(originPos,patternTwo_Two_rightCirclePos,publicTime);
				}

				patternTwo_Two_rightCircle.localRotation = Quaternion.Euler(0f,0f,MathEx.easeOutCubic(0f,360f,publicTime));
				transform.position = MathEx.EaseOutQuadVector2(originPos,patternTwo_Two_leftCharacterPos,publicTime);

				if(publicTime >= 1f)
				{
					transform.position = patternTwo_Two_leftCharacterPos;
					patternTwo_Two_rightCircle.position = patternTwo_Two_rightCirclePos;
					patternTwo_Two_rightCircle.localRotation = Quaternion.Euler(0f,0f,0f);
				}
			}

			if(publicTime >= 1f)
			{
				publicTime = 0f;
				firePosCount = 0;
				progressDirection = false;
				fireActive = true;
				stayTime = 0.3f;
			}
		}
		else
		{
			if(fireActive)
			{
				publicTime += Time.deltaTime;
				if(publicTime >= 0.1f)
				{
					publicTime = 0f;
					Vector3 pos = Vector3.Lerp(patternTwo_Two_firePos[0],patternTwo_Two_firePos[1],((float)firePosCount++) / 8);

					Vector3 dir = patternTwo_Two_firePos[0] - patternTwo_Two_firePos[1];
					dir = dir.normalized;
					float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
					ang *= direction ? 1f : -1f;

					if(direction == false)
					{
						pos.x *= -1f;
					}

					if(firePosCount == 1)
					{
						EffectManager.instance.ObjectActive(pos,21,0.02f,ang + (direction ? -90f : 90f));
					}

					Explosion exp = null;
					if(direction)
					{
						exp = Instantiate(test,pos,Quaternion.identity).GetComponent<Explosion>();
						exp.angle = (float)firePosCount * 10f;
						exp.anime = 20;
					}
					else
					{
						exp = Instantiate(test,pos,Quaternion.identity).GetComponent<Explosion>();
						exp.minus = true;
						exp.angle = (float)firePosCount * 10f;
						exp.anime = 20;
					}

					if(direction)
					{
						BulletManager.instance.ObjectActive(null,pos,8f,1f,ang + 90f,false,false).SetAnimation(17,false);
						BulletManager.instance.ObjectActive(null,pos,8f,1f,ang + 270f,false,false).SetAnimation(17,false);

						BulletManager.instance.ObjectActive(null,pos,7f,1f,ang + 160f,false,false).SetAnimation(18,false);
						BulletManager.instance.ObjectActive(null,pos,7f,1f,ang + 200f,false,false).SetAnimation(18,false);
					}
					else
					{
						BulletManager.instance.ObjectActive(null,pos,8f,1f,ang + 90f,false,false).SetAnimation(17,false);
						BulletManager.instance.ObjectActive(null,pos,8f,1f,ang + 270f,false,false).SetAnimation(17,false);

						BulletManager.instance.ObjectActive(null,pos,7f,1f,ang + 20f,false,false).SetAnimation(18,false);
						BulletManager.instance.ObjectActive(null,pos,7f,1f,ang - 20f,false,false).SetAnimation(18,false);
					}

				}

				if(firePosCount >= 8)
				{
					fireActive = false;
					if(!direction)
						stayTime = 1f;
				}
			}
			else
			{
				if(direction)
				{
					originPos = transform.position;
					progressDirection = true;
					direction = false;
					stayTime = 1f;

					circleRotate = false;
				}
				else
				{
					publicTime += 0.2f * Time.deltaTime;
					transform.position = Vector3.Lerp(transform.position,new Vector3(0f,6.3f,0f),publicTime);

					if(publicTime >= 0.5f)
					{
						if(patternTwo_Three_Progress)
						{
							++patternTwo_Three_PatternCount;
							type = MoveType.PatternTwo_One;
						}
						else
							type = MoveType.PatternTwo_Three;

						transform.position = new Vector3(0f,6.3f,0f);
					}
				}
			}
		}

	}
#endregion

	public void PatternTwo_Three()
	{
		stayTime = 0.5f;
		prevType = type;

		patternTwo_Three_PatternCount = 0;
		patternOne_Two_TwistOptSpiral.ValueInit(new Vector3(-1f,-1f,-1f),12,1,19,4f,0.5f,0f,15f);
		patternTwo_Three_Progress = true;

		type = MoveType.PatternTwo_One;
	}

#region PatternTwo_Four
	private bool firstZen = true;
	public void PatternTwo_Four_Init()
	{
		publicTime = 0f;
		firstZen = true;
	}
	public void PatternTwo_Four()
	{
		if(prevType != type)
		{
			prevType = type;
			PatternTwo_Four_Init();
		}

		if(firstZen)
		{
			float between = 360f / 24f;
			for(int i = 0; i < 24; ++i)
			{
				Explosion xp = Instantiate(test,transform.position,Quaternion.identity).GetComponent<Explosion>();
				xp.direction = new Vector3(Mathf.Cos(((between * i) * Mathf.Deg2Rad)),Mathf.Sin(((between * i) * Mathf.Deg2Rad)));
				xp.moveDist = 2f;
				xp.angle = 90f;
				xp.anime = 16;
			}

			firstZen = false;
		}
		else
		{
			publicTime += Time.deltaTime;

			if(publicTime >= 1f)
			{
				publicTime = 0f;
				float between = 360f / 12f;
				for(int i = 0; i < 12; ++i)
				{
					Explosion xp = Instantiate(test,transform.position,Quaternion.identity).GetComponent<Explosion>();
					xp.direction = new Vector3(Mathf.Cos(((between * i) * Mathf.Deg2Rad)),Mathf.Sin(((between * i) * Mathf.Deg2Rad)));
					xp.moveDist = 1.5f;
					xp.anime = 16;
				}

				for(int i = 0; i < 12; ++i)
				{
					Explosion xp = Instantiate(test,transform.position,Quaternion.identity).GetComponent<Explosion>();
					xp.direction = new Vector3(Mathf.Cos(((between * i) * Mathf.Deg2Rad)),Mathf.Sin(((between * i) * Mathf.Deg2Rad)));
					xp.moveDist = 1.5f;
					xp.angle = 180f;
					xp.anime = 16;
				}

				for(int i = 0; i < 12; ++i)
				{
					Explosion xp = Instantiate(test,transform.position,Quaternion.identity).GetComponent<Explosion>();
					xp.direction = new Vector3(Mathf.Cos(((between * i) * Mathf.Deg2Rad)),Mathf.Sin(((between * i) * Mathf.Deg2Rad)));
					xp.moveDist = 4f;
					xp.angle = 90f;
					xp.anime = 17;
				}


				between = 360f / 36f;
				for(int i = 0; i < 36; ++i)
				{
					BulletManager.instance.ObjectActive(null,transform.position,0f,1f,i * between,false).SetAnimation(20,false).SetAccel(2f);
					BulletManager.instance.ObjectActive(null,transform.position,0f,1f,i * between + between / 2f,false).SetAnimation(20,false).SetAccel(1.5f);
				}

				stayTime = 3f;
				type = MoveType.PatternOne_One;
			}
		}
	}
#endregion
}
